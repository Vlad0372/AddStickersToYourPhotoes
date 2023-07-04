using System.Windows;

namespace AddStickersToYourPhoto
{
    public class MainImage
    {
        public string FullPath { get; set; }
        public Point Location { get; set; } 
        public double Width { get; set; }
        public double Height { get; set; }

        public MainImage(string fullPath, Point location, double width, double height)
        {
            FullPath = fullPath;
            Location = location;
            Width = width;
            Height = height;
        }     
    }
}
