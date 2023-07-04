namespace AddStickersToYourPhoto
{
    internal class Sticker
    {
        public string Name { get; set; }       
        public string Image { get; set; }

        public Sticker(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }
}
