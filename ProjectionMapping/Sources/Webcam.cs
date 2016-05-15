using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using WpfCap;

namespace ProjectionMapping.Sources
{
    public class Webcam : InputSource
    {
        //[XmlIgnore] 
        [XmlIgnore]
        public override string DisplayName
        {
            get { return this.Source; }
        }

        //[XmlIgnore]
        //private static CapPlayer camPlayer = null;

        //public static CapPlayer Player
        //{
        //    get
        //    {
        //        if (camPlayer == null)
        //        {
        //            camPlayer = new CapPlayer { LayoutTransform = new RotateTransform(-180), Framerate = 30 };
        //        }
        //        return camPlayer;
        //    }

        //}
        // = new CapPlayer { LayoutTransform = new RotateTransform(-180), Framerate = 30 };


        public Webcam()
        {
            Source = "Webcam"; //camPlayer.Source.ToString();            
        }

        public override Brush GetBursh()
        {
            DoFadeEffect(App.camPlayer);

            return new VisualBrush(App.camPlayer);
        }

        public override FrameworkElement GetElement()
        {
            return App.camPlayer;
          //  App.camPlayer
        }

        public override InputType InputSourceType
        {
            get { return InputType.Cam; }
        }
    }
}
