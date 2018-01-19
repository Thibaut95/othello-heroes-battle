﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public static readonly Tuple<ECoinType, String>[] arrayOfTuplesHeroes =
        {
            //MARVEL TEAM
            Tuple.Create(ECoinType.spiderman, "spiderman.png"),
            Tuple.Create(ECoinType.ironman, "ironman.png"),
            Tuple.Create(ECoinType.greenarrow, "greenarrow.png"),
            Tuple.Create(ECoinType.wolverine, "wolverine.png"),

            //DC TEAM
            Tuple.Create(ECoinType.superman, "superman.png"),
            Tuple.Create(ECoinType.batman, "batman.png" ),
            Tuple.Create(ECoinType.greenlantern, "green_lantern.png"),
            Tuple.Create(ECoinType.robin, "robin.png"),
        };

        public static ImageBrush GetBrushHeroes(ECoinType heroesType)
        {
            String path = "";

            foreach (var item in arrayOfTuplesHeroes)
            {  
                if (item.Item1 == heroesType)
                {
                    path = "images/" + item.Item2;
                }
            }
            
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
