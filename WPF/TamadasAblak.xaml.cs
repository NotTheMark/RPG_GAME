using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RPGProjekt
{
    public partial class TamadasAblak : Window
    {
        public Karakter KivalasztottTamado { get; private set; }
        public Karakter KivalasztottCelpont { get; private set; }

        private List<Karakter> hosok;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public TamadasAblak(List<Karakter> hosokLista)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            InitializeComponent();
            hosok = hosokLista.Where(h => !h.Halott).ToList();

            cmbTamado.ItemsSource = hosok;
            cmbCelpont.ItemsSource = hosok;

            cmbTamado.DisplayMemberPath = "KarakterNev";
            cmbCelpont.DisplayMemberPath = "KarakterNev";
        }

        private void TamadasGomb_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTamado.SelectedItem is Karakter tamado && cmbCelpont.SelectedItem is Karakter celpont)
            {
                if (tamado == celpont)
                {
                    MessageBox.Show("Egy karakter nem támadhatja meg önmagát!", "Hiba", MessageBoxButton.OK);
                    return;
                }

                KivalasztottTamado = tamado;
                KivalasztottCelpont = celpont;

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Válassz ki egy támadót és egy célpontot!", "Figyelmeztetés", MessageBoxButton.OK);
            }
        }
    }
}
