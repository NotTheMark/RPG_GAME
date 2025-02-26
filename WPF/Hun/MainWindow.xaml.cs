using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RPGProjekt
{
    public partial class MainWindow : Window
    {
        public int kalandok = 0;
        private List<Karakter> hosok = new List<Karakter>();
        public MainWindow()
        {
            InitializeComponent();
            hosok = Karakter.BetoltesFajlbol();
            lstKarakterek.ItemsSource = hosok;
        }
        private void FrissitLista()
        {
            lstKarakterek.Items.Refresh();
            lstKarakterek.ItemsSource = hosok;
        }
        private void HozzaadKarakter(object sender, RoutedEventArgs e)
        {
            string nev = Microsoft.VisualBasic.Interaction.InputBox("Add meg a karakter nevét:", "Új karakter");
            if (!string.IsNullOrWhiteSpace(nev))
            {
                Karakter ujHos = new Karakter(nev);
                hosok.Add(ujHos);
                FrissitLista();
                MessageBox.Show($"{nev} hozzáadva a karakterekhez!", "Siker");
            }
        }
        private void Tamadas(object sender, RoutedEventArgs e)
        {
            if (hosok.Count < 2)
            {
                MessageBox.Show("Legalább két karakter szükséges a támadáshoz!", "Sikertelen Támadás!", MessageBoxButton.OK);
                return;
            }
            TamadasAblak tamadasAblak = new TamadasAblak(hosok);
            if (tamadasAblak.ShowDialog() == true)
            {
                Karakter tamado = tamadasAblak.KivalasztottTamado;
                Karakter celpont = tamadasAblak.KivalasztottCelpont;

                if (tamado == null || celpont == null)
                    return;
                tamado.Tamadas(celpont);
                MessageBox.Show($"{tamado.KarakterNev} megtámadta {celpont.KarakterNev}-t!\n {celpont.KarakterNev} új életereje: {celpont.EletEro}", "Sikeres Támadás", MessageBoxButton.OK);
                FrissitLista();
                EllenorizHalal(celpont);
            }
        }
        private void Gyogyitas(object sender, RoutedEventArgs e)
        {
            if (lstKarakterek.SelectedItem is Karakter karakter)
            {
                karakter.Gyogyitas();
                FrissitLista();
                MessageBox.Show($"{karakter.KarakterNev} gyógyított magán!\nÚj élet: {karakter.EletEro}", "Gyógyítás", MessageBoxButton.OK);
            }
        }
        private void Szintlepes(object sender, RoutedEventArgs e)
        {
            if (lstKarakterek.SelectedItem is Karakter karakter)
            {
                karakter.Szintlepes();
                FrissitLista();
                MessageBox.Show($"{karakter.KarakterNev} szintet lépett!\nÚj szint: {karakter.Szint}", "Szintlépés", MessageBoxButton.OK);
                szintfel.Content = "Szintlépés (" + kalandok + "/5)";
                szintfel.IsEnabled = false;
            }
        }
        private void FelszerelesCsere(object sender, RoutedEventArgs e)
        {
            if (lstKarakterek.SelectedItem is Karakter karakter)
            {
                Random szam = new Random();
                int esely = szam.Next(1, 7);
                
                if(esely == 6)
                {
                    karakter.FelszerelesCsereSiker();
                    MessageBox.Show($"{karakter.KarakterNev} új felszerelést kapott: {karakter.Felszereles}", "Sikeres Kaland!", MessageBoxButton.OK);
                    karakter.tapasztalat();
                    kalandok += 1;
                    szintfel.Content = "Szintlépés (" + kalandok + "/5)";
                }
                else
                {
                    MessageBox.Show($"{karakter.KarakterNev} elment kalandozni, de nem talált semmit.","Sikertelen Kaland!", MessageBoxButton.OK);
                    karakter.tapasztalat();
                    kalandok += 1;
                    szintfel.Content = "Szintlépés (" + kalandok + "/5)";
                }
                if(kalandok == 5)
                {
                    szintfel.Content = "Szintlépés (" + kalandok + "/5)";
                    szintfel.IsEnabled = true;
                    kalandok = 0;
                    karakter.tapasztalatReset();
                }
                    FrissitLista();
            }
        }
        private void Mentes(object sender, RoutedEventArgs e)
        {
            foreach (var karakter in hosok)
            {
                karakter.MentesFajlba();
                hosok = Karakter.BetoltesFajlbol();
                lstKarakterek.ItemsSource = hosok;
            }
            MessageBox.Show("Minden karakter elmentve fájlba!", "Mentés sikeres", MessageBoxButton.OK);
            
        }
        private void Kilepes(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void EllenorizHalal(Karakter celpont)
        {
            if (celpont.EletEro <= 0)
            {
                MessageBox.Show($"{celpont.KarakterNev} elesett a harcban!", "Halál", MessageBoxButton.OK);
                foreach (var karakter in hosok)
                {
                    karakter.MentesFajlba();
                    hosok = Karakter.BetoltesFajlbol();
                    lstKarakterek.ItemsSource = hosok;
                }
                FrissitLista();
            }
        }
    }
}
