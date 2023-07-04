using System.Windows;

namespace AddStickersToYourPhoto
{
    public class ChosenSticker
    {   
        public string Path { get; set; }
        public Point Location { get; set; }
        public Size StickerSize { get; set; }     

        public ChosenSticker(string path, Point location, Size stickerSize)
        {      
            Path = path;
            Location = location;
            StickerSize = stickerSize;
        }
        public enum Size : ushort
        {
            Small = 50,
            Medium = 80,
            Large = 120
        }
    }
}
