using RPGProjekt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RPG_nagymarci_WPF
{

    public partial class Piac : Window
    {
        private Karakter aktualisKarakter;

        public Piac(Karakter karakter)
        {   
            InitializeComponent();
            aktualisKarakter = karakter;
            FrissitPenz();
        }

        private void FrissitPenz()
        {
            if (aktualisKarakter != null)
            {
                penz0.Text = $"{aktualisKarakter.Penz} G";
                penz1.Text = $"{aktualisKarakter.Penz} G";
            }
        }

        private void Feketekave(object sender, RoutedEventArgs e)
        {

            if (aktualisKarakter.Vasarlas("kave", 5))
            {
                MessageBox.Show("Sikeres vásárlás +15 energia");
                FrissitPenz();
            }
            else
            {
                MessageBox.Show("Nincs elég pénz!");
            }
            
        }

        private void Vbjkard(object sender, RoutedEventArgs e)
        {
            
            if (aktualisKarakter.Vasarlas("vbjkard", 100))
            {
                MessageBox.Show("Sikeres vásárlás, lett egy jobb kardod");
                FrissitPenz();
            }
            else
            {
                MessageBox.Show("Nincs elég pénz!");
            }
        }

        private void Katakulcs(object sender, RoutedEventArgs e)
        {
            if (aktualisKarakter.Vasarlas("katakulcs", 30))
            {
                MessageBox.Show("Sikeres vásárlás, uj lehetőségek nyiltak meg kalandozás során");
                FrissitPenz();
            }
            else
            {
                MessageBox.Show("Nincs elég pénz!");
            }
        }

        private void Kataterkep(object sender, RoutedEventArgs e)
        {
            if (aktualisKarakter.Vasarlas("kataterkep", 50))
            {
                MessageBox.Show("Sikeres vásárlás, uj lehetőségek nyiltak meg kalandozás során");
                FrissitPenz();
            }
            else
            {
                MessageBox.Show("Nincs elég pénz!");
            }
        }

        private void Szerencse(object sender, RoutedEventArgs e)
        {
            if (aktualisKarakter.Vasarlas("talizman", 25))
            {
                MessageBox.Show("Sikeres vásárlás, innentől kedvezőbben forognak a sors dobokockái nálad");
                FrissitPenz();
            }
            else
            {
                MessageBox.Show("Nincs elég pénz!");
            }
        }
    }
}
