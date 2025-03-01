using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RPG_nagymarci_WPF
{
    public partial class Terkep : Window
    {
        int[,] terkep = {
        { 1, 1, 1, 1, 4, 4, 4, 3, 1, 1, 1, 1, 0, 0 },
        { 1, 1, 1, 4, 4, 4, 3, 3, 1, 1, 3, 1, 1, 0 },
        { 1, 1, 1, 4, 4, 4, 3, 3, 1, 3, 3, 0, 0, 0 },
        { 1, 1, 1, 4, 4, 4, 3, 3, 1, 1, 1, 0, 0, 0 },
        { 3, 1, 1, 4, 4, 4, 3, 3, 3, 2, 1, 0, 0, 0 },
        { 4, 1, 1, 3, 4, 4, 4, 3, 1, 1, 1, 0, 0, 0 },
        { 4, 2, 1, 3, 4, 4, 3, 3, 1, 1, 1, 0, 0, 0 },
        { 3, 3, 1, 1, 4, 3, 3, 3, 3, 1, 1, 0, 0, 0 },
        { 3, 3, 1, 1, 4, 3, 3, 3, 1, 1, 1, 0, 0, 0 },
        { 3, 3, 3, 1, 1, 1, 3, 3, 1, 2, 0, 0, 0, 0 },
        { 0, 3, 3, 3, 1, 1, 1, 3, 1, 1, 0, 0, 0, 0 },
        { 0, 0, 4, 4, 3, 1, 1, 3, 1, 1, 0, 0, 0, 0 },
        { 0, 0, 0, 4, 3, 3, 1, 1, 1, 1, 0, 0, 0, 0 }
    };



        int cellaMeret = 25; 


        private int karakterX = 0; 
        private int karakterY = 0; 


        Rectangle karakter;

        public Terkep()
        {
            InitializeComponent();
            RajzolTerkep();
            this.KeyDown += KeyDownHandler;
        }
        private bool LehetMozogni(int x, int y)
        {
            if (x < 0 || x >= terkep.GetLength(0) || y < 0 || y >= terkep.GetLength(1))
            {
                return false; 
            }


            int tile = terkep[y, x];
            if (tile == 2 || tile == 3 || tile == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int ujX = karakterX;
            int ujY = karakterY;

            switch (e.Key)
            {
                case Key.W: ujY -= 1; break;
                case Key.S: ujY += 1; break;
                case Key.A: ujX -= 1; break;
                case Key.D: ujX += 1; break;
            }

            if (LehetMozogni(ujX, ujY))
            {
                karakterX = ujX;
                karakterY = ujY;
                RajzolTerkep();
            }
        }
        private void MozgassKarakter(int dx, int dy)
        {
            int ujX = karakterX + dx;
            int ujY = karakterY + dy;

            if (LehetMozogni(ujX, ujY))
            {
                karakterX = ujX; 
                karakterY = ujY;
                RajzolTerkep(); 
            }
            else
            {
                MessageBox.Show("Nem tudsz ide menni, akadály v");
            }
        }
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.W:
                    MozgassKarakter(0, -1);
                    break;
                case Key.S:
                    MozgassKarakter(0, 1);
                    break;
                case Key.A:
                    MozgassKarakter(-1, 0);
                    break;
                case Key.D:
                    MozgassKarakter(1, 0);
                    break;
            }
        }


        private void RajzolTerkep()
        {
            TerkepCanvas.Children.Clear();

            for (int sor = 0; sor < terkep.GetLength(0); sor++)
            {
                for (int oszlop = 0; oszlop < terkep.GetLength(1); oszlop++)
                {
                    Rectangle cella = new Rectangle
                    {
                        Width = cellaMeret,
                        Height = cellaMeret,
                        Fill = GetSzint(terkep[sor, oszlop]),
                        Stroke = Brushes.Black
                    };

                    Canvas.SetLeft(cella, oszlop * cellaMeret);
                    Canvas.SetTop(cella, sor * cellaMeret);
                    TerkepCanvas.Children.Add(cella);
                }
            }


            karakter = new Rectangle
            {
                Width = cellaMeret,
                Height = cellaMeret,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(karakter, karakterX * cellaMeret);
            Canvas.SetTop(karakter, karakterY * cellaMeret);
            TerkepCanvas.Children.Add(karakter);
        }



        private ImageBrush GetSzint(int tipus)
        {
            ImageBrush brush = new ImageBrush();

            switch (tipus)
            {
                case 0: 
                    brush.ImageSource = new BitmapImage(new Uri("Resources/viz.png", UriKind.Relative));
                    break;
                case 1:
                    brush.ImageSource = new BitmapImage(new Uri("Resources/fu.png", UriKind.Relative));
                    break;
                case 2:
                    brush.ImageSource = new BitmapImage(new Uri("Resources/varos.png", UriKind.Relative));
                    break;
                case 3:
                    brush.ImageSource = new BitmapImage(new Uri("Resources/erdo.png", UriKind.Relative));
                    break;
                case 4:
                    brush.ImageSource = new BitmapImage(new Uri("Resources/hegyek.png", UriKind.Relative));
                    break;

                default:
                    brush.ImageSource = new BitmapImage(new Uri("Resources/fu.png", UriKind.Relative));
                    break;
            }

            return brush;
        }

    }
}
