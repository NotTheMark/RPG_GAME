using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace RPGProjekt
{
    public class Karakter
    {
        public string KarakterNev { get; private set; }
        public int Szint { get; private set; }
        public int EletEro { get; private set; }
        public int HarciKepesseg { get; private set; }
        public string Felszereles { get; private set; }

        public Karakter(string nev, int szint = 1, int eletEro = 100, int harciKepesseg = 10, string felszereles = "Alap kard")
        {
            KarakterNev = nev;
            Szint = szint;
            EletEro = eletEro;
            HarciKepesseg = harciKepesseg;
            Felszereles = felszereles;
        }

        public void Szintlepes()
        {
            Szint++;
            EletEro += 20;
            HarciKepesseg += 5;
            Console.WriteLine($"{KarakterNev} szintet lépett! Új szint: {Szint}");
        }

        public void Tamadas(Karakter celpont)
        {
            if (EletEro > 0)
            {
                Console.WriteLine($"{KarakterNev} megtámadta {celpont.KarakterNev}-t!");
                celpont.Sebzodik(HarciKepesseg);
            }
            else
            {
                Console.WriteLine($"{KarakterNev} nem tud támadni, mert nincs életben.");
            }
        }

        public void Vedekezes()
        {
            Console.WriteLine($"{KarakterNev} védekezik, csökkentett sebzést kap a következő támadásból.");
        }

        public void Gyogyitas()
        {
            EletEro += 15;
            Console.WriteLine($"{KarakterNev} gyógyított magán. Új élet: {EletEro}");
        }

        public void FelszerelesCsere(string ujFelszereles)
        {
            Felszereles = ujFelszereles;
            Console.WriteLine($"{KarakterNev} új felszerelést kapott: {Felszereles}");
        }

        public void Sebzodik(int sebzes)
        {
            EletEro -= sebzes;
            if (EletEro <= 0)
            {
                EletEro = 0;
                Console.WriteLine($"{KarakterNev} elesett a harcban.");
            }
            else
            {
                Console.WriteLine($"{KarakterNev} {sebzes} sebzést kapott. Hátralévő élet: {EletEro}");
            }
        }
        public void MentesFajlba()
        {
            string fajlNev = KarakterNev + ".txt";
            string tartalom = $"Név: {KarakterNev}\nSzint: {Szint}\nÉleterő: {EletEro}\nHarci képesség: {HarciKepesseg}\nFelszerelés: {Felszereles}";
            File.WriteAllText(fajlNev, tartalom);
            Console.WriteLine($"{KarakterNev} adatai elmentve: {fajlNev}");
        }
        public static List<Karakter> BetoltesFajlbol()
        {
            List<Karakter> betoltottHosok = new List<Karakter>();
            string[] fajlok = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");

            foreach (var fajl in fajlok)
            {
                string[] sorok = File.ReadAllLines(fajl);
                if (sorok.Length >= 5)
                {
                    string nev = sorok[0].Split(':')[1];
                    int szint = int.Parse(sorok[1].Split(':')[1]);
                    int eletEro = int.Parse(sorok[2].Split(':')[1]);
                    int harciKepesseg = int.Parse(sorok[3].Split(':')[1]);
                    string felszereles = sorok[4].Split(':')[1];

                    Karakter ujHos = new Karakter(nev, szint, eletEro, harciKepesseg, felszereles);
                    betoltottHosok.Add(ujHos);
                }
            }
            Console.WriteLine($"{betoltottHosok.Count} karakter betöltve a fájlokból.");
            return betoltottHosok;
        }

    }

    class Program
    {
        static List<Karakter> hosok = new List<Karakter>();
        static void Main(string[] args)
        {
            hosok = Karakter.BetoltesFajlbol();

            bool start = true;
            bool menu = false;
            int hosindex = 0;
            while (start)
            {
                if(hosok.Count > 0)
                {
                    foreach (var hos in hosok)
                    {
                        Console.WriteLine("Betöltött hősök:" + string.Join(",", hos.KarakterNev));
                    }
                    start = false;
                    menu = true;
                }
            }
            while (menu == true)
            {
                Console.WriteLine("Mit szeretnél tenni?\n1-Hozzáadás (karakter)\n2-Támadás\n3-Védekezés\n4-Gyógyítás\n5-Szintlépés\n6-Kalandozás(felszereléscseréhez vezethet)\n7-Mentés\n8-Kilépés");
                int bead = Convert.ToInt32(Console.ReadLine());
                if (bead == 1)
                {
                    Console.WriteLine("Adj hozzá karaktert! Mi legyen a neve?: ");
                    string karakternev = Console.ReadLine();
                    Karakter ujhos = new Karakter(karakternev);
                    hosok.Add(ujhos);
                }
                else if (bead == 2)
                {
                    Console.Clear();
                    if (hosok.Count < 2)
                    {
                        Console.WriteLine(hosok[0].KarakterNev.ToString() + " szörnyeket vadászott.");
                    }
                    else if (hosok.Count >= 2)
                    {
                        Console.WriteLine("Melyik hős támad? (karakternev)");
                        string tamadohos = Console.ReadLine();
                        foreach (var hos in hosok)
                        {
                            if (hos.KarakterNev == tamadohos)
                            {
                                Console.Clear();
                                Console.WriteLine("Támadó hős kiválasztva.");
                            }

                        }
                        Console.WriteLine("Melyik hőst? (karakternev)");
                        string vedohos = Console.ReadLine();
                        foreach (var hos in hosok)
                        {
                            if (hos.KarakterNev == vedohos)
                            {
                                Console.Clear();
                                Console.WriteLine("Védő hős kiválasztva.");
                            }
                        }
                        int thosindex = 0;
                        int vhosindex = 0;
                        foreach (var hos in hosok)
                        {
                            if (hos.KarakterNev == tamadohos)
                            {
                                thosindex = hosok.IndexOf(hos);
                            }
                            if (hos.KarakterNev == vedohos)
                            {
                                vhosindex = hosok.IndexOf(hos);
                            }
                        }
                        hosok[thosindex].Tamadas(hosok[vhosindex]);
                        Console.WriteLine(tamadohos + " és " + vedohos + " harcoltak!");
                    }
                }
                else if (bead == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Melyik hőst? (karakternev)");
                    string valasztotthos = Console.ReadLine();
                    foreach (var hos in hosok)
                    {
                        if (hos.KarakterNev == valasztotthos)
                        {
                            hosindex = hosok.IndexOf(hos);
                            hosok[hosindex].Vedekezes();
                        }
                    }
                }
                else if (bead == 4)
                {
                    Console.Clear();
                    Console.WriteLine("Melyik hőst? (karakternev)");
                    string valasztotthos = Console.ReadLine();
                    foreach (var hos in hosok)
                    {
                        if (hos.KarakterNev == valasztotthos)
                        {
                            hosindex = hosok.IndexOf(hos);
                            hosok[hosindex].Gyogyitas();
                        }
                    }

                }
                else if (bead == 5)
                {
                    Console.Clear();
                    Console.WriteLine("Melyik hőst? (karakternev)");
                    string valasztotthos = Console.ReadLine();
                    foreach (var hos in hosok)
                    {
                        if (hos.KarakterNev == valasztotthos)
                        {
                            hosindex = hosok.IndexOf(hos);
                            hosok[hosindex].Szintlepes();
                        }
                    }
                }
                else if (bead == 6)
                {
                    Console.Clear();
                    Console.WriteLine("Melyik hős? (karakternev)");
                    string valasztotthos = Console.ReadLine();
                    foreach (var hos in hosok)
                    {
                        if (hos.KarakterNev == valasztotthos)
                        {
                            hosindex = hosok.IndexOf(hos);
                            hosok[hosindex].FelszerelesCsere("Mágikus Kard");
                        }
                    }
                }
                else if (bead == 7)
                {
                    Console.Clear();
                    foreach (var hos in hosok)
                    {
                        hos.MentesFajlba();
                        Console.WriteLine(hos.KarakterNev + " mentve fájlba.");
                    }
                }
                else if (bead == 8)
                {
                    Console.Clear();
                    Console.WriteLine("Kilépés sikeres.");
                    menu = false;
                }
                Console.ReadLine();
                Console.Clear();
            }
            Console.ReadLine();
        }
    }
}
