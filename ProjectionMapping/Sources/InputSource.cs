using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Serialization;
using ProjectionMapping.Annotations;

namespace ProjectionMapping.Sources
{
    public enum InputType
    {
        Invalid = 0,
        Video = 1,
        Still = 2,
        Cam = 3,
        SolidColor = 4,
        Gif = 5,
        Webpage = 6
    }

    [Serializable]
    [XmlInclude(typeof(Video))]
    [XmlInclude(typeof(Still))]
    [XmlInclude(typeof(Webcam))]
    [XmlInclude(typeof(SolidColor))]
    //[XmlInclude(typeof(Webpage))]
    public abstract class InputSource : INotifyPropertyChanged
    {
        // acts like simple ordinal ID
        [XmlIgnore]
        private static int instanceCount = 1;

        private double opacity;
        private double fadeInSec;

        public int ID { get; set; }

        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged("Opacity");
            }
        }
        public virtual string Source { get; set; }

        public double FadeInSec
        {
            get { return fadeInSec; }
            set
            {
                fadeInSec = value;
                OnPropertyChanged("FadeInSec");
            }
        }

        // Give it a user friendly name
        [XmlIgnore]
        public virtual string DisplayName
        {
            get
            { return this.Source; }
        }

        protected InputSource()
        {
            ID = instanceCount;
            instanceCount++;
            Opacity = 1;
            FadeInSec = 0;
        }

        // not sure I am fan of having the type like this
        [XmlIgnore]
        public abstract InputType InputSourceType { get; }

        // get the source's brush to paint on the surface
        public abstract Brush GetBursh();

        public abstract FrameworkElement GetElement();

        protected void DoFadeEffect(FrameworkElement element)
        {
            var fadeAnimation = new DoubleAnimation()
            {
                From = 0.0,
                To = this.Opacity,
                Duration = TimeSpan.FromSeconds(FadeInSec)
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeAnimation);
            Storyboard.SetTarget(fadeAnimation, element);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));

            storyboard.Begin();
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
