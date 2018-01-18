using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;


namespace OthelloHeroesBattle
{
    public static class ImageManager
    {
        public static readonly Dictionary<ECoinType, String> dictionnaryHeroes = new Dictionary<ECoinType, string>
        {
            {ECoinType.spiderman, "spiderman.png" },
            {ECoinType.ironman, "ironman.png" },
            {ECoinType.superman, "superman.png" },
        };

        public static ImageBrush GetBrushHeroes(ECoinType eCoinType)
        {
            string path = "images/"+dictionnaryHeroes[eCoinType];
            Uri resourceHeroe = new Uri(path, UriKind.Relative);

            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceHeroe);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);

            ImageBrush brush = new ImageBrush
            {
                Stretch = Stretch.UniformToFill,
                ImageSource = temp
            };
            return brush;
        }

        public static ImageBrush GetBrushImage(String filename)
        {
            string path = "images/"+filename;
            Uri resouceImage = new Uri(path, UriKind.Relative);

            StreamResourceInfo streamInfo = Application.GetResourceStream(resouceImage);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);

            ImageBrush brush = new ImageBrush
            {
                Stretch = Stretch.Uniform,
                ImageSource = temp
            };
            return brush;
        }
    }
}
