using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ProjectionMapping.Sources
{
	[Serializable]
	public class Still : InputSource
	{
		public Still()
		{

		}

		public Still(string source)
		{
			Source = source;
		}

		public override Brush GetBursh()
		{
			Brush brush = null;

			if (!string.IsNullOrWhiteSpace(Source) && File.Exists(Source))
			{
				//var imgBr = new ImageBrush(new BitmapSource)
				//var me = new MediaElement {Source = new Uri(Source)};
				var bmp = new BitmapImage();
				bmp.BeginInit();
				bmp.UriSource = new Uri(Source);
				bmp.EndInit();
				var img = new Image
				{
					Name ="img",
					Stretch = Stretch.Uniform,
					Source = bmp
				};
                
				DoFadeEffect(img);
                
				brush = new VisualBrush(img);

			}

			return brush;
		}

	    public override FrameworkElement GetElement()
	    {
            //var imgBr = new ImageBrush(new BitmapSource)
            //var me = new MediaElement {Source = new Uri(Source)};
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(Source);
            bmp.EndInit();
            var img = new Image
            {
                Name = "img",
                Stretch = Stretch.Uniform,
                Source = bmp
            };
           
	        return img;
	    }

	    [XmlIgnore]
		public override string DisplayName
		{
			get { return Path.GetFileNameWithoutExtension(this.Source); }
		}

		[XmlIgnore]
		public override InputType InputSourceType
		{
			get { return InputType.Still; }
		}

	}
}
