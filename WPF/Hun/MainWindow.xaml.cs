using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RPGProjekt
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer frissitoTimer;
        public int kalandok = 0;
        private List<Karakter> hosok = new List<Karakter>();
        public MainWindow()
        {
            InitializeComponent();
            hosok = Karakter.BetoltesFajlbol();
            lstKarakterek.ItemsSource = hosok;

            frissitoTimer = new DispatcherTimer();
            frissitoTimer.Interval = TimeSpan.FromSeconds(0.5);
            frissitoTimer.Tick += KarakterInfoFrissit;
            frissitoTimer.Start();
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
                int esely = szam.Next(1, 101);
                kalandok += 1;
                szintfel.Content = "Szintlépés (" + kalandok + "/5)";

                switch (esely)
                {
                    case int n when (n <= 35):
                        int kezd_hp = karakter.EletEro;
                        karakter.VaratlanHarc();
                        if (karakter.EletEro == kezd_hp)
                        {
                            MessageBox.Show($"{karakter.KarakterNev} elindult és kalandozás közben egy vad cica megtámadta, de a hatalmas tudásának köszönetően nem sérült meg a harcban", "Váratlan Harc", MessageBoxButton.OK);
                        }
                        else if (karakter.EletEro < kezd_hp - 30)
                        {
                            MessageBox.Show($"{karakter.KarakterNev} elindult és kalandozás közben egy vad cica megtámadta, Nehezen tudta csak legyőzni és a küzdelem után {karakter.EletEro} HP-ja maradt", "Váratlan Harc", MessageBoxButton.OK);
                        }
                        else if (karakter.EletEro < kezd_hp - 20)
                        {
                            MessageBox.Show($"{karakter.KarakterNev} elindult és kalandozás közben egy vad cica megtámadta, Könnyedén tduta legyőzni és a küzdelem után {karakter.EletEro} HP-ja maradt", "Váratlan Harc", MessageBoxButton.OK);
                        }
                        EllenorizHalal(karakter);
                        break;
                    case int n when (n > 93):
                        karakter.FelszerelesCsereSiker();
                        MessageBox.Show($"{karakter.KarakterNev} új felszerelést kapott: {karakter.Felszereles}", "Sikeres Kaland!", MessageBoxButton.OK);
                        karakter.tapasztalat();
                        break;

                    default:
                        MessageBox.Show($"{karakter.KarakterNev} elment kalandozni, de nem talált semmit.", "Sikertelen Kaland!", MessageBoxButton.OK);
                        karakter.tapasztalat();
                        break;
                }

                if (kalandok == 5)
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

        private void KarakterInfoFrissit(object sender, EventArgs e)
        {
            if (lstKarakterek.SelectedItem is Karakter karakter)
            {
            lblSzint.Content = "Szint: " + karakter.Szint;
            lblElet.Content = "Élet: " + karakter.EletEro;
            lblTapasztalat.Content = "Tapasztalat: " + karakter.Xp;
            lblFelszereles.Content = "Felszerelés: " + karakter.Felszereles;
            lblPenz.Content = "Pénz: " + karakter.Penz;
            }
            lstKarakterek.Items.Refresh();
            
        }

        private void Fejvadaszat(object sender, RoutedEventArgs e)
        {

        }

        private void PiacMegnyit(object sender, RoutedEventArgs e)
        {

        }
    }     
}
