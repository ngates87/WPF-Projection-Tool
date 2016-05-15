using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace WpfCap
{
    public class CapPlayer : Image, IDisposable
    {
        private CapDevice Device = new CapDevice();

        //public CapDevice Device
        //{
        //    get
        //    {
        //        return device;
        //    }
        //}

        public CapPlayer()
        {
            InitBitmap();
            Application.Current.Exit += Current_Exit;
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            this.Dispose();
        }
        void InitBitmap()
        {

            if (Device == null)
            {
                Device = new CapDevice();
                Device.OnNewBitmapReady += _device_OnNewBitmapReady;
            }
            else
            {
                Device.Start();
            }
        }

        void _device_OnNewBitmapReady(object sender, EventArgs e)
        {
            var b = new Binding { Source = Device, Path = new PropertyPath(CapDevice.FramerateProperty) };
            this.SetBinding(FramerateProperty, b);

            this.Source = Device.BitmapSource;
        }

        public float Framerate
        {
            get { return (float)GetValue(FramerateProperty); }
            set { SetValue(FramerateProperty, value); }
        }
        public static readonly DependencyProperty FramerateProperty =
            DependencyProperty.Register("Framerate", typeof(float), typeof(CapPlayer), new UIPropertyMetadata(default(float)));


        #region IDisposable Members

        public void Dispose()
        {
            if (Device != null)
                Device.Dispose();
        }

        #endregion
    }
}
