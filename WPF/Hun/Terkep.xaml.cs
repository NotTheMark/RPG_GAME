﻿using RPGProjekt;
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
        private MainWindow mainWindow;
        private Karakter Karakter;

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

        public Terkep(MainWindow mainWindow, Karakter karakter)
        {
            InitializeComponent();
            this.mainWindow = mainWindow; 
            this.Karakter = karakter;  
            RajzolTerkep();
            this.KeyDown += KeyDownHandler;
        }


        private bool LehetMozogni(int x, int y)
        {
            if (x < 0 || x >= terkep.GetLength(0) || y < 0 || y >= terkep.GetLength(1)-1)
            {
                return false; 
            }

            Random szam = new Random();
            int esely = szam.Next(1, 101);

            int tile = terkep[y, x];
            if (tile == 2 || tile == 3 || tile == 1)
            {

                if (tile == 1 &&esely <= 15)
                {
                    Karakter.ValamiVan();
                    mainWindow.EllenorizHalal(Karakter);
                    if (Karakter.EletEro <= 0) this.Close();
                }
                if (tile == 3 && esely <= 35)
                {
                    Karakter.ValamiVan();
                    mainWindow.EllenorizHalal(Karakter);
                    if (Karakter.EletEro <= 0) this.Close();
                }

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

            Image karakterImage = new Image
            {
                Width = cellaMeret,
                Height = cellaMeret,
                Source = GetKarakterSprite()
            };

            Canvas.SetLeft(karakterImage, karakterX * cellaMeret);
            Canvas.SetTop(karakterImage, karakterY * cellaMeret);
            TerkepCanvas.Children.Add(karakterImage);
        }

        private ImageSource GetKarakterSprite()
        {
            BitmapImage spriteSheet = new BitmapImage(new Uri("Resources/karakterkep.png", UriKind.Relative));

            int spriteSzam = 4;
            double spriteWidth = spriteSheet.PixelWidth / spriteSzam;
            double spriteHeight = spriteSheet.PixelHeight;

            int spriteIndex = 0;

            if (Karakter.EletEro > 75) spriteIndex = 0;
            else if (Karakter.EletEro > 50) spriteIndex = 1; 
            else if (Karakter.EletEro > 25) spriteIndex = 2;  
            else spriteIndex = 3;

            CroppedBitmap croppedBitmap = new CroppedBitmap(
                spriteSheet,
                new Int32Rect((int)(spriteIndex * spriteWidth), 0, (int)spriteWidth, (int)spriteHeight)
            );

            return croppedBitmap;
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
