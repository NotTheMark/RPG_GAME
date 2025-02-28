using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.XPath;
namespace RPGProjekt
{
    public class Karakter
    {
        public string KarakterNev { get; private set; }
        public int Szint { get; private set; }
        public int EletEro { get; private set; }
        public int HarciKepesseg { get; private set; }
        public string Felszereles { get; private set; }
        public int Xp { get; private set; }
        public int Penz { get; private set; }
        public int Energia { get; private set; }
        public bool KatakombaKulcs { get; set; }
        public bool KatakombaTerkepp { get; set; }
        public int Szerencse { get; set; }
        public bool Halott => EletEro <= 0;
        private const int MaxEnergia = 100;
        public Karakter(string nev, int szint = 1, int eletEro = 100, int harciKepesseg = 10, string felszereles = "Fakard", int xp = 0, int penz = 0, int energia = 100)
        {
            KarakterNev = nev;
            Szint = szint;
            EletEro = eletEro;
            HarciKepesseg = harciKepesseg;
            Felszereles = felszereles;
            Xp = xp;
            Penz = penz;
            Energia = energia;

            KatakombaKulcs = false;
            KatakombaTerkepp = false;
            Szerencse = 0;
        }
        public void tapasztalat()
        {
            Xp++;
        }
        public void tapasztalatReset()
        {
            Xp = 0;
        }

        public void EnergiaUjra()
        {
            Energia = Math.Min(Energia + 5, MaxEnergia);
        }
        public bool EnergiaVesztes(int mennyiseg)
        {
            if (Energia >= mennyiseg)
            {
                Energia -= mennyiseg;
                return true;
            }
            return false;
        }
        public void Szintlepes()
        {
            if(Szint > 14)
            {
                Szint = 14;
            }
            else if(Szint < 14)
            {
                Szint++;
                EletEro += 5;
                HarciKepesseg += 5;
            }   
        }
        public void Tamadas(Karakter celpont)
        {
            celpont.Sebzodik(HarciKepesseg);
        }
        public void Vedekezes()
        {
            Console.WriteLine($"{KarakterNev} védekezik, csökkentett sebzést kap a következő támadásból.");
            EletEro += 10;
        }

        public bool Vasarlas(string id,int ar)
        {
            MessageBox.Show(Convert.ToString(Penz));
            if (ar <= Penz)
            {
                switch (id)
                {
                    case "kave":
                        Energia += 5;
                        Penz -= ar;
                        break;

                    case "vbjkard":
                        HarciKepesseg += 35;
                        Felszereles = "Vak Botyján Kuruc Generális Kardja";
                        Penz -= ar;
                        break;

                    case "katakulcs":
                        KatakombaKulcs = true;
                        Penz -= ar;
                        break;

                    case "kataterkep":
                        KatakombaTerkepp = true;
                        Penz -= ar;
                        break;

                    case "talizman":
                        Szerencse += 8;
                        Penz -= ar;
                        break;

                    default:
                        break;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public void Gyogyitas()
        {
            Random szam = new Random();
            int esely =szam.Next(1, 7);
            if(EletEro > 300)
            {
                EletEro = 300;
            }
            if(esely == 1)
            {
                EletEro += 10;
            }
            else if(esely == 2)
            {
                EletEro += 15;
            }
            else if (esely == 3)
            {
                EletEro += 20;
            }
            else if (esely == 4)
            {
                EletEro += 30;
            }
            else if (esely == 5)
            {
                EletEro += 40;
            }
            else
            {
                EletEro += 50;
            }
        }
        public void FelszerelesCsereSiker()
        {
            if(Felszereles == "Mágikus Kard")
            {
                Felszereles = "Odin Legendás Kardja";
                HarciKepesseg += 15;
            }
            else if (Felszereles == "Fakard")
            {
                Felszereles = "Mágikus Kard";
                HarciKepesseg += 30;
            }
        }
        public void VaratlanHarc()
        {
            Random random_szam = new Random();
            int esely = random_szam.Next(1, 101);
            esely += Szerencse;
            if (HarciKepesseg < 35)
            {
                if (esely <= 70) 
                {
                    EletEro -= 50;
                    
                }
                else
                {
                    EletEro -= 30;
                    
                }
            }
            else if (HarciKepesseg < 45)
            {
                if (esely <= 40) 
                {
                    EletEro -= 40;
                    
                }
                else
                {
                    EletEro -= 20;
                    
                }
            }
            else if (HarciKepesseg < 60)
            {
                if (esely <= 20)
                {
                    EletEro -= 20;
                    
                }
            }
            else
            {
                
            }
        }
        public bool Fejvadaszatra_indul(bool siker_E)
        {
            if (siker_E)
            {
                Penz += 10;
                return true;
            }
            else
            {
                Penz -= 13;
                return false;
            }
        }
        public void Sebzodik(int sebzes)
        {

            EletEro -= sebzes;
            if (EletEro <= 0)
            {
                EletEro = 0;
            }
        }
        public void MentesFajlba()
        {
            string halottMappa = Path.Combine(Directory.GetCurrentDirectory(), "Halott");
            if (!Directory.Exists(halottMappa))
            {
                Directory.CreateDirectory(halottMappa);
            }
            string fajlNev = KarakterNev + ".txt";
            string tartalom = $"Név: {KarakterNev}\nSzint: {Szint}\nÉleterő: {EletEro}\nHarci képesség: {HarciKepesseg}\nFelszerelés: {Felszereles}\nXp: {Xp}\nPénz: {Penz}\nEnergia: {Energia}";
            if (EletEro == 0)
            {

                string ujFajl = Path.Combine(halottMappa, fajlNev);
                File.WriteAllText(ujFajl, tartalom);
            }
            else
            {
                File.WriteAllText(fajlNev, tartalom);
            }
            if (EletEro > 0)
            {
                string[] fajlok = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
                foreach (var fajl in fajlok)
                {
                    string[] sorok = File.ReadAllLines(fajl);
                    if (sorok.Length >= 6)
                    {
                        int eletEro = int.Parse(sorok[2].Split(':')[1]);
                        if (eletEro == 0)
                        {
                            string ujFajl = Path.Combine(halottMappa, Path.GetFileName(fajl));
                            File.Move(fajl, ujFajl);
                        }
                    }
                }
            }
        }

        public static List<Karakter> BetoltesFajlbol()
        {
            List<Karakter> betoltottHosok = new List<Karakter>();


            string[] fajlok = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");

            string halottMappa = Path.Combine(Directory.GetCurrentDirectory(), "Halott");
            if (Directory.Exists(halottMappa))
            {
                fajlok = fajlok.Concat(Directory.GetFiles(halottMappa, "*.txt")).ToArray();
            }

            foreach (var fajl in fajlok)
            {
                string[] sorok = File.ReadAllLines(fajl);
                if (sorok.Length >= 6)
                {
                    int eletEro = int.Parse(sorok[2].Split(':')[1]);

                    if (eletEro > 0)
                    {
                        string nev = sorok[0].Split(':')[1].Trim();
                        int szint = int.Parse(sorok[1].Split(':')[1]);
                        string felszereles = sorok[4].Split(':')[1].Trim();
                        int xp = int.Parse(sorok[5].Split(':')[1]);
                        int harciKepesseg = int.Parse(sorok[3].Split(':')[1]);
                        int penz = int.Parse(sorok[6].Split(":")[1]);
                        int energia = int.Parse(sorok[7].Split(":")[1]);
                        Karakter ujHos = new Karakter(nev, szint, eletEro, harciKepesseg, felszereles, xp, penz, energia);
                        betoltottHosok.Add(ujHos);
                    }
                }
            }
            return betoltottHosok;
        }

    }
}
