using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public bool Halott => EletEro <= 0;
        public Karakter(string nev, int szint = 1, int eletEro = 100, int harciKepesseg = 10, string felszereles = "Fakard", int xp = 0)
        {
            KarakterNev = nev;
            Szint = szint;
            EletEro = eletEro;
            HarciKepesseg = harciKepesseg;
            Felszereles = felszereles;
            Xp = xp;
        }
        public void tapasztalat()
        {
            Xp++;
        }
        public void tapasztalatReset()
        {
            Xp = 0;
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
            string[] fajlok = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
            foreach (var fajl in fajlok)
            {
                string[] sorok = File.ReadAllLines(fajl);
                if (sorok.Length >= 6)
                {
                    if (int.Parse(sorok[2].Split(':')[1]) == 0)
                    {
                        File.Delete(fajl);
                    }
                }
            }
            string fajlNev = KarakterNev + ".txt";
            string tartalom = $"Név: {KarakterNev}\nSzint: {Szint}\nÉleterő: {EletEro}\nHarci képesség: {HarciKepesseg}\nFelszerelés: {Felszereles}\nXp: {Xp}";
            File.WriteAllText(fajlNev, tartalom);
        }
        public static List<Karakter> BetoltesFajlbol()
        {
            List<Karakter> betoltottHosok = new List<Karakter>();
            string[] fajlok = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
            foreach (var fajl in fajlok)
            {
                string[] sorok = File.ReadAllLines(fajl);
                if (sorok.Length >= 6)
                {
                    if (int.Parse(sorok[2].Split(':')[1]) != 0)
                    {
                        string nev = sorok[0].Split(':')[1].Trim();
                        int szint = int.Parse(sorok[1].Split(':')[1]);
                        int eletEro = int.Parse(sorok[2].Split(':')[1]);
                        int harciKepesseg = int.Parse(sorok[3].Split(':')[1]);
                        string felszereles = sorok[4].Split(':')[1].Trim();
                        int xp = int.Parse(sorok[5].Split(':')[1]);
                        Karakter ujHos = new Karakter(nev, szint, eletEro, harciKepesseg, felszereles,xp);
                        betoltottHosok.Add(ujHos);
                    }
                    else
                    {
                        File.Delete(fajl);
                    }
                }
            }
            return betoltottHosok;
        }
    }
}
