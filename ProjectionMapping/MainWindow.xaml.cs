using System.Runtime.CompilerServices;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProjectionMapping.Annotations;
using ProjectionMapping.Dialog;
using ProjectionMapping.Sources;

namespace ProjectionMapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: INotifyPropertyChanged
    {
        private Solution currentSolution;
        // readonly CueProcessor processor = new CueProcessor();
        public static CollectionViewSource SurfaceCollection;
        public static CollectionViewSource mediaCollection;
        public static CollectionViewSource sourcesCollection;

        readonly Output renderWindow = new Output(App.model3DCollection);

        private bool solutionOpen;

        public bool SolutionOpen
        {
            get { return solutionOpen; }
            set
            {
                solutionOpen = value;
                OnPropertyChanged("SolutionOpen");
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            App.model3DCollection.Add(new AmbientLight(System.Windows.Media.Colors.DarkGray));
            App.model3DCollection.Add(new DirectionalLight(System.Windows.Media.Colors.White, new Vector3D(-5, -5, -7)));
            //<!--<AmbientLight Color="DarkGray" />
            //<DirectionalLight Color="White" Direction="-5,-5,-7" />-->
            renderWindow.Models3DGroup.Children = App.model3DCollection;
            //Output
            renderWindow.Show();

            //renderWindow.Models3DGroup.Children = ((Model3DCollection)FindResource("surfaceModels"));
            //renderWindow.Left= this.Left + this.Width + 10;
            SurfaceCollection = (CollectionViewSource)FindResource("SurfacesCollection");

            SurfaceCollection.Source = App.surfaces;

            mediaCollection = (CollectionViewSource)FindResource("mediaCollection");
            mediaCollection.Source = App.mediaFiles;

            sourcesCollection = (CollectionViewSource)FindResource("sourcesCollection");
            sourcesCollection.Source = App.sources;

            renderWindow.KeyDown += configWindow_KeyUp;

            //processor.Start();

        }

        private void configWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                // renderWindow.VideoControl.Play();
                renderWindow.WindowStyle = WindowStyle.None;
                renderWindow.Topmost = true;
                renderWindow.WindowState = WindowState.Maximized;
                Mouse.OverrideCursor = Cursors.None;

            }
            else if(e.Key == Key.Space)
            {
                cueControl.NextCue();
            }
            else if(e.Key == Key.Back)
            {
                cueControl.PrevCue();
            }
        }

        // Create and add a new surface 
        private void AddSurfaceButtonClick(object sender, RoutedEventArgs e)
        {
            var surface = new Layer("test");
            App.surfaces.Add(surface); // add the surface to the gui list
            App.model3DCollection.Add(surface.Layer3DModel);
            App.manager.Attach(string.Format("/layer/{0}/surface", surface.ID), new Rug.Osc.OscMessageEvent(surface.ChangeSource));
            lvSurfaces.SelectedIndex = 0;
           // App.sources.Add(new Webpage());
            // add the 3d model of the surface to the output window
            // renderWindow.Models3DGroup.Children.Add(surface.Layer3DModel);

        }

        private void RemoveSurfaceButtonClick(object sender, RoutedEventArgs e)
        {
            Layer surface = lvSurfaces.SelectedItem as Layer;

            if (surface != null)
            {
                App.model3DCollection.Remove(surface.Layer3DModel);
                App.surfaces.Remove(surface);
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            //renderWindow.Close();

        }

        #region Corner Values Sliders
        private void SliderTopLeftX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = lvSurfaces.SelectedItem as Layer;
            if (temp != null && surf != null)
                surf.Move(temp.Value, surf.PointsTransformed[(int)Corners.TopLeft].Y, (int)Corners.TopLeft);
        }

        private void SliderTopLeftY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null && surf != null)
                surf.Move(surf.PointsTransformed[(int)Corners.TopLeft].X, temp.Value, (int)Corners.TopLeft);
        }

        private void SliderTopRightX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null && surf != null)
                surf.Move(temp.Value, surf.PointsTransformed[(int)Corners.TopRight].Y, (int)Corners.TopRight);
        }

        private void SliderTopRightY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null)
                surf.Move(surf.PointsTransformed[(int)Corners.TopRight].X, temp.Value, (int)Corners.TopRight);
        }

        private void SliderBottomLeftX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null && surf != null)
                surf.Move(temp.Value, surf.PointsTransformed[(int)Corners.BottomLeft].Y, (int)Corners.BottomLeft);
        }

        private void SliderBottomLeftY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null && surf != null)
                surf.Move(surf.PointsTransformed[(int)Corners.BottomLeft].X, temp.Value, (int)Corners.BottomLeft);
        }

        private void SliderBottomRightX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null)
                surf.Move(temp.Value, surf.PointsTransformed[(int)Corners.BottomRight].Y, (int)Corners.BottomRight);
        }

        private void SliderBottomRightY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var temp = (sender as Slider);
            var surf = (Layer)lvSurfaces.SelectedItem;
            if (temp != null)
                surf.Move(surf.PointsTransformed[(int)Corners.BottomRight].X, temp.Value, (int)Corners.BottomRight);
        }
        #endregion

        private void NewSolution_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                FileName = "default",
                DefaultExt = ".prj",
                Filter = "Projection Mapping Project (.prj)|*.prj"
            };

            if (dlg.ShowDialog() != true)
                return;

            currentSolution = Solution.CreateSolution(dlg.FileName);
            this.Title = currentSolution.FilePath;
            SolutionOpen = true;
            // var files = Directory.EnumerateFiles(currentSolution.ImagesFolder, "*.*", SearchOption.AllDirectories)
            //     .Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".bmp"));
        }

        private void SaveSolution_Click(object sender, RoutedEventArgs e)
        {
            var presets = new List<LayerPreset>(App.surfaces.Count);
            presets.AddRange(App.surfaces.Select(layer => layer.Export()));
            if (currentSolution != null)
                currentSolution.Save(presets, App.sources.ToList());
        }

        private void OpenSolution_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                FileName = "default",
                DefaultExt = ".prj",
                Filter = "Projection Mapping Project (.prj)|*.prj"
            };

            if (dlg.ShowDialog() == true)
            {
                currentSolution = Solution.OpenSolution(dlg.FileName);
                this.Title = currentSolution.FilePath;

                foreach (var source in currentSolution.Sources)
                {
                    App.sources.Add(source);
                }

                foreach (var pre in currentSolution.Layers)
                {
                    var surface = new Layer(pre);
                    App.surfaces.Add(surface);
                    App.model3DCollection.Add(surface.Layer3DModel);
                    // renderWindow.Models3DGroup.Children.Add(surface.Layer3DModel);

                }

                lvSurfaces.SelectedIndex = 0;

                foreach (var cue in currentSolution.Cues)
                {
                    App.cues.Add(cue);
                }
                
                SolutionOpen = true;
                //   var files = Directory.EnumerateFiles(currentSolution.ImagesFolder, "*.*", SearchOption.AllDirectories)
                //         .Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".bmp"));

            }
        }

        private IEnumerable<string> GetFilePaths()
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Multi-Media Files|*.avi; *.mpeg; *.mp4; *.mov; *.wmv; *.mpg; *.jpg;*.jpeg;*.png;*.bmp; *.gif"
            };
            ofd.ShowDialog();
            return ofd.FileNames;
        }


        private void AddMediaClick(object sender, RoutedEventArgs e)
        {
            var files = GetFilePaths();

            foreach (var item in files)
            {
                // don't add if it exist already
                //if (mediaFiles.FirstOrDefault(m => m.FilePath != item) != null)
                {
                    // What do we do if the solution is null, we should probably wait until save to move files
                    //File.Copy(item, System.IO.Path.Combine(currentSolution.ImagesFolder, System.IO.Path.GetFileName(item)));
                    var mf = new MediaFile
                    {
                        Ext = Path.GetExtension(item),
                        FileName = Path.GetFileName(item),
                        FilePath = item
                    };
                    App.mediaFiles.Add(mf);
                }

            }
        }

        private void RemoveMeidaClick(object sender, RoutedEventArgs e)
        {

        }

        private void OpenOutputWindowClick(object sender, RoutedEventArgs e)
        {
            // Create new window? Re-Tie view port objects
            Output window = new Output(App.model3DCollection);
            window.Show();
        }


        // move the selected file over
        private void AddSourceClick(object sender, RoutedEventArgs e)
        {
            foreach (MediaFile mf in lvLibrary.SelectedItems.Cast<MediaFile>().Where(mf => mf != null))
            {
                if (mf.IsStill())
                {
                    App.sources.Add(new Still(mf.FilePath));

                }
                else if (mf.IsGif())
                {
                    App.sources.Add(new Gif(mf.FilePath));
                }
                else
                {
                    App.sources.Add(new Video(mf.FilePath));

                }
            }
            //var mf = lvLibrary.SelectedItems as MediaFile;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var cue = new RemoteCue();
            cue.Show();
        }

        private void AddCameraSouce(object sender, RoutedEventArgs e)
        {
            App.sources.Add(new Webcam());
        }

        private void AddSolidColorClick(object sender, RoutedEventArgs e)
        {
            App.sources.Add(new SolidColor(colorPicker.SelectedColor));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
