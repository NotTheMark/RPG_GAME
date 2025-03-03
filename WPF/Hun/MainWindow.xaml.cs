using RPG_nagymarci_WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace RPGProjekt
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer frissitoTimer;
        private DispatcherTimer energiaTimer;
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


            energiaTimer = new DispatcherTimer();
            energiaTimer.Interval = TimeSpan.FromSeconds(30);
            energiaTimer.Tick += EnergiaFrissit;
            energiaTimer.Start();
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
        void FrissitKep(int hp)
        {
            int spriteIndex = 0;

            if (hp > 75) spriteIndex = 0;      
            else if (hp > 50) spriteIndex = 1; 
            else if (hp > 25) spriteIndex = 2; 
            else spriteIndex = 3;              

            double spriteWidth = 1.0 / 4;
            double xOffset = spriteIndex * spriteWidth; 



            SpriteBrush.Viewbox = new Rect(xOffset, 0, spriteWidth, 1);
            SpriteBrush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
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
                if (!karakter.EnergiaVesztes(20))
                {
                    MessageBox.Show("Nincs elég energiád a kalandozáshoz!", "Energiahiány", MessageBoxButton.OK);
                    return;
                }

                Terkep terkepAblak = new Terkep(this,karakter); 
                terkepAblak.Show();

                kalandok += 1;
                szintfel.Content = "Szintlépés (" + kalandok + "/5)";

                if (kalandok == 5)
                {
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
        public void EllenorizHalal(Karakter celpont)
        {
            if (celpont.EletEro <= 0)
            {
                MessageBox.Show($"{celpont.KarakterNev} elesett a harcban!", "Halál", MessageBoxButton.OK);
                hosok.Remove(celpont);

                string fajlNev = $"{celpont.KarakterNev}.txt";
                if (File.Exists(fajlNev))
                {
                    File.Delete(fajlNev);
                }
                
                FrissitLista();
            }
    
        }
        private void EnergiaFrissit(object sender, EventArgs e)
        {
            foreach (var karakter in hosok)
            {
                karakter.EnergiaUjra();
            }
            FrissitLista();
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
            lblEnergia.Content = "Energia: " + karakter.Energia;

            FrissitKep(karakter.EletEro);
            CharacterSprite.Visibility = Visibility.Visible;
            }
            lstKarakterek.Items.Refresh();


        }

        private void Fejvadaszat(object sender, RoutedEventArgs e)
        {
            if (lstKarakterek.SelectedItem is Karakter karakter)
            {
               

                Random randszam = new Random();

                if (!karakter.EnergiaVesztes(30))
                {
                    MessageBox.Show("Nincs elég energiád a fejvadászatra!", "Energiahiány", MessageBoxButton.OK);
                    return;
                }

                int esely = randszam.Next(1, 101);
                esely += (int)(Math.Log(karakter.Szint + 1) * 10);
                esely += karakter.Szerencse;

                int randomErtek = randszam.Next(1, 101); // --- kontrolnak van itte 
                bool siker = esely >= randomErtek;

                if (karakter.Fejvadaszatra_indul(siker))
                {
                    MessageBox.Show("A külkdetés sikeresn teljesítve -- + 10 G");
                }
                else
                {
                    MessageBox.Show("Baj lett, nem sikerült a megadott célponot eltalálni, -13 G");
                }
            }
            else
            {
                MessageBox.Show("Nincs karakter kivalsztva");
            }
            
        }

        private void PiacMegnyit(object sender, RoutedEventArgs e)
        {
            if (lstKarakterek.SelectedItem is Karakter karakter)
            {
               Piac Piacablaak = new Piac(karakter);
               Piacablaak.Show();
            }
            else
            {
                MessageBox.Show("Válasz ki kérlek egy karakterk aki elmegy a piacra.");
            }
        }
    }     
}
