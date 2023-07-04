using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AddStickersToYourPhoto
{
    public class MyVisualHost : FrameworkElement
    {
        private readonly VisualCollection _children;
        
        public MyVisualHost(ChosenSticker sticker)
        {
            _children = new VisualCollection(this);
            _children.Add(CreateDrawingVisualSticker(sticker));                    
        }

        public MyVisualHost(MainImage mainImage)
        {
            _children = new VisualCollection(this);
            _children.Add(CreateDrawingVisualMainImage(mainImage));
        }

        public static PngBitmapEncoder SaveResults(List<ChosenSticker> imgCollection, MainImage mainImage)
        {
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {            
                var mainImageSource = new BitmapImage(new Uri(mainImage.FullPath, UriKind.Absolute));
                drawingContext.DrawImage(mainImageSource, new Rect(0, 0, mainImage.Width, mainImage.Height));
                
                foreach (ChosenSticker sticker in imgCollection)
                {
                    var imageSource = new BitmapImage(new Uri(@"../.." + sticker.Path, UriKind.Relative));
                    drawingContext.DrawImage(imageSource, new Rect(sticker.Location.X - mainImage.Location.X, sticker.Location.Y - mainImage.Location.Y, (int)sticker.StickerSize, (int)sticker.StickerSize));
                }               
            }

            //Converts the Visual(DrawingVisual) into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)mainImage.Width, (int)mainImage.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
         
            // Creates a PngBitmapEncoder and adds the BitmapSource to the frames of the encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            return encoder;         
        }
     
        private DrawingVisual CreateDrawingVisualSticker(ChosenSticker sticker)
        {
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                var imageSource = new BitmapImage(new Uri(@"../.." + sticker.Path, UriKind.Relative));
                drawingContext.DrawImage(imageSource, new Rect(sticker.Location.X, sticker.Location.Y, (int)sticker.StickerSize, (int)sticker.StickerSize));
            }

            return drawingVisual;
        }
        private DrawingVisual CreateDrawingVisualMainImage(MainImage mainImage)
        {      
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                var imageSource = new BitmapImage(new Uri(mainImage.FullPath, UriKind.Absolute));
                drawingContext.DrawImage(imageSource, new Rect(mainImage.Location.X, mainImage.Location.Y, mainImage.Width, mainImage.Height));
            }
       
            return drawingVisual;
        }


        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount => _children.Count;

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }
    }
}
