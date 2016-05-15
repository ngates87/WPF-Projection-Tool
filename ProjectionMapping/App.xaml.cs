using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ProjectionMapping.Sources;
using WpfCap;

namespace ProjectionMapping
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // list of Layers
        public static ObservableCollection<Layer> surfaces = new ObservableCollection<Layer>();
        // just a list of images/video
        public static readonly ObservableCollection<MediaFile> mediaFiles = new ObservableCollection<MediaFile>();

        public static readonly Rug.Osc.OscAddressManager manager = new Rug.Osc.OscAddressManager();

        //public Still still;
        // list of Sources
        public static ObservableCollection<InputSource> sources = new ObservableCollection<InputSource>();
        public static ObservableCollection<string> cues = new ObservableCollection<string>();
        public static Model3DCollection model3DCollection = new Model3DCollection();
        public static CapPlayer camPlayer;// = new CapPlayer { LayoutTransform = new RotateTransform(-180), Framerate = 30 };
        public const int sendRecvPort = 1701;

        //public static Model3DCollection surfaceModels =  new Model3DCollection();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Model3DCollection surfaceModels = (Model3DCollection)FindResource("surfaceModels");

            // surfaceModels.Add(new AmbientLight(Colors.DarkGray));
            // surfaceModels.Add(new DirectionalLight(Colors));
            // <AmbientLight Color="DarkGray" />
            //  <DirectionalLight Color="White" Direction="-5,-5,-7" />

           // camPlayer = new CapPlayer { LayoutTransform = new RotateTransform(-180), Framerate = (float)29.97};
            
            
        }


    }
}
