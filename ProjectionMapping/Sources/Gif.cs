using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace ProjectionMapping.Sources
{
    class Gif:InputSource
    {

        public Gif()
        {
            
        }
        public Gif(string source)
        {
            Source = source;
        }
        public override string DisplayName
        {
            get {return Source; }
        }

        public override InputType InputSourceType
        {
            get { return InputType.Gif; }
        }

        public override Brush GetBursh()
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(Source);
            bmp.EndInit();
           
            var gif = new Image();
	        ImageBehavior.SetAnimatedSource(gif, bmp);
			//ImageBehavior.SetRepeatBehavior(gif,);

            DoFadeEffect(gif);

            return new VisualBrush(gif);
        }

        public override FrameworkElement GetElement()
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(Source);
            bmp.EndInit();

            var gif = new Image();
            ImageBehavior.SetAnimatedSource(gif, bmp);

            return gif;
        }
    }
}
