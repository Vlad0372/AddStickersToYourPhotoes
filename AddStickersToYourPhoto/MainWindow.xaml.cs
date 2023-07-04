using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;

namespace AddStickersToYourPhoto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isStickerChosen;
        private bool isFileUploaded;
        private Point mousePos;
        private string chosenStickerPath;
        private ChosenSticker.Size stickerSize;
        private List<ChosenSticker> imgCollection;
        private MainImage mainImage;

        public MainWindow()
        {
            InitializeComponent();
            
            var stickers = GetStickers();
            if (stickers.Count > 0)
                ListViewProducts.ItemsSource = stickers;

           
            image1.Visibility = Visibility.Hidden;
            stickerSize = ChosenSticker.Size.Medium;
            isStickerChosen = false;
            isFileUploaded = false;         
            imgCollection = new List<ChosenSticker>();  
            
            CanvasBorder.Width = CanvasColumn.Width.Value;
            CanvasBorder.Height = CanvasRow.Height.Value;
        }
        private List<Sticker> GetStickers()
        {
            return new List<Sticker>()
            {
                 new Sticker("Sticker 1", "/Assets/s1.png"),
                 new Sticker("Sticker 2", "/Assets/s2.png"),
                 new Sticker("Sticker 3", "/Assets/s3.png"),
                 new Sticker("Sticker 4", "/Assets/s4.png"),
                 new Sticker("Sticker 5", "/Assets/s5.png"),
                 new Sticker("Sticker 6", "/Assets/s6.png"),
                 new Sticker("Sticker 7", "/Assets/s7.png"),
                 new Sticker("Sticker 8", "/Assets/s8.png"),
                 new Sticker("Sticker 9", "/Assets/s1.png"),
                 new Sticker("Sticker 10", "/Assets/s2.png"),
                 new Sticker("Sticker 11", "/Assets/s3.png"),
                 new Sticker("Sticker 12", "/Assets/s4.png"),
                 new Sticker("Sticker 13", "/Assets/s5.png"),
                 new Sticker("Sticker 14", "/Assets/s6.png"),
                 new Sticker("Sticker 15", "/Assets/s7.png"),
                 new Sticker("Sticker 16", "/Assets/s8.png")
            };
        }
        private void Sticker_Click(object sender, MouseEventArgs e)
        {
            if (isFileUploaded)
            {
                switch (stickerSizeCmbx.SelectedIndex)
                {
                    case 0:
                        stickerSize = ChosenSticker.Size.Small;
                        break;
                    case 1:
                        stickerSize = ChosenSticker.Size.Medium;
                        break;
                    case 2:
                        stickerSize = ChosenSticker.Size.Large;
                        break;
                }

                var originalSource = (Border)e.OriginalSource;
                var stickerDataContext = (Sticker)originalSource.DataContext;
                chosenStickerPath = stickerDataContext.Image;

                var position = e.GetPosition(MainCanvas);
                Canvas.SetLeft(image1, position.X);
                Canvas.SetTop(image1, position.Y);

                GrabSticker();
            }
            else
            {
                string messageBoxText = "Upload a photo to use stickers fisrt";
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Exclamation;

                MessageBox.Show(messageBoxText, caption, button, icon);
            }           
        }     
        private void Border_Click(object sender, MouseEventArgs e)
        {
            mousePos = e.GetPosition(MyCanvas);
                            
            if (isStickerChosen)
            {
                var newSticker = new ChosenSticker(chosenStickerPath, mousePos, stickerSize);                           
                var visualHost = new MyVisualHost(newSticker);
               
                MyCanvas.Children.Add(visualHost);
                imgCollection.Add(newSticker);
            }
        }   
        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isStickerChosen)
            {
                var position = e.GetPosition(MainCanvas);

                Canvas.SetLeft(image1, position.X);
                Canvas.SetTop(image1, position.Y);

                if (IsStickerInRect(position))
                {
                    CanvasBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    CanvasBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }                    
        }   
        private bool IsStickerInRect(Point p)
        {
            var totalLeftIndent = CanvasBorder.Margin.Left + CanvasBorder.BorderThickness.Left +  mainImage.Location.X;
            var totalRightIndent = CanvasBorder.Margin.Left - CanvasBorder.BorderThickness.Right - mainImage.Location.X + CanvasBorder.Width - (int)stickerSize;

            var totalTopIndent = 25 + CanvasBorder.Margin.Top + CanvasBorder.BorderThickness.Top +  mainImage.Location.Y;
            var totalBottomIndent = 25 + CanvasBorder.Margin.Top - CanvasBorder.BorderThickness.Bottom - mainImage.Location.Y + CanvasBorder.Height - (int)stickerSize;

            if (p.X > totalLeftIndent && p.X < totalRightIndent 
                && p.Y > totalTopIndent && p.Y < totalBottomIndent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isFileUploaded)
            {
                mousePos = e.GetPosition(MainCanvas);

                if (IsStickerInRect(mousePos))
                {
                    Border_Click(sender, e);
                }

                ReleaseGrabbedSticker();
            }         
        }

        private void GrabSticker()
        {
            CanvasBorder.BorderBrush = new SolidColorBrush(Colors.Red);

            MainCanvas.MouseMove += MainCanvas_MouseMove;

            image1.Source = new BitmapImage(new Uri(chosenStickerPath, UriKind.Relative));
            image1.Visibility = Visibility.Visible;
            image1.Width = (int)stickerSize;
            image1.Height = (int)stickerSize;

            isStickerChosen = true;
        }
        private void ReleaseGrabbedSticker()
        {           
            CanvasBorder.BorderBrush = new SolidColorBrush(Colors.DimGray);

            MainCanvas.MouseMove -= MainCanvas_MouseMove;
            image1.Visibility = Visibility.Hidden;

            isStickerChosen = false;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;

            if(menuItem.Header.ToString() == "_Upload" && !isFileUploaded)
            {             
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
                openFileDialog.FilterIndex = 1;

                if(openFileDialog.ShowDialog() == true)
                {
                    var imageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                    mainImage = new MainImage(imageSource.UriSource.AbsoluteUri, new Point(0, 0), imageSource.PixelWidth, imageSource.PixelHeight);
                   
                    if(mainImage.Width > mainImage.Height)
                    {
                        FitToParentByWidth();

                        if (mainImage.Height > CanvasBorder.Height - CanvasBorder.BorderThickness.Top * 2)
                        {
                            FitToParentByHeight();
                        }
                    }
                    else
                    {
                        FitToParentByHeight();

                        if (mainImage.Width > CanvasBorder.Width - CanvasBorder.BorderThickness.Left * 2)
                        {
                            FitToParentByWidth();
                        }
                    }                 

                    var visualHost = new MyVisualHost(mainImage);               
                    MyCanvas.Children.Add(visualHost);                

                    isFileUploaded = true;
                }

                void FitToParentByHeight()
                {
                    double coeffChild = mainImage.Width / mainImage.Height;

                    mainImage.Height = CanvasBorder.Height - (CanvasBorder.BorderThickness.Top * 2);
                    mainImage.Width = mainImage.Height * coeffChild;

                    var newX = (int)((CanvasBorder.Width - mainImage.Width) / 2 - CanvasBorder.BorderThickness.Left); 
                    mainImage.Location = new Point(newX, 0);
                }
                void FitToParentByWidth()
                {                 
                    double coeffChild = mainImage.Height / mainImage.Width;

                    mainImage.Width = CanvasBorder.Width - (CanvasBorder.BorderThickness.Left * 2);
                    mainImage.Height = mainImage.Width * coeffChild;

                    var newY = (int)((CanvasBorder.Height - mainImage.Height) / 2 - CanvasBorder.BorderThickness.Top);
                    mainImage.Location = new Point(0, newY);
                }
            }
            else if(menuItem.Header.ToString() == "_Save" && isFileUploaded)
            {               
                var encoder = MyVisualHost.SaveResults(imgCollection, mainImage);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Image files|*.png;*.bmp;*.jpg";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (Stream stream = File.Create(saveFileDialog.FileName))
                    {
                        encoder.Save(stream);
                    }
                      
                    isFileUploaded = false;
                    MyCanvas.Children.Clear();
                }             
            }
            else if(menuItem.Header.ToString() == "_ClearAll")
            {
                isStickerChosen = false;
                isFileUploaded = false;                                            

                mainImage = null;

                imgCollection.Clear();
                MyCanvas.Children.Clear();
            }
            else
            {
                string messageBoxText = "There is nothing to save or the photo has been already uploaded";
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Exclamation;

                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }
    }
}
