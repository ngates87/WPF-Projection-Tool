using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.DataGrid.FilterCriteria;

namespace ProjectionMapping.Sources
{
    public class SolidColor : InputSource
    {
        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        public SolidColor(Color color)
        {
            Color = color;
        }
        public SolidColor()
        {
            Color = Colors.DarkSlateGray;
        }

        public override string DisplayName
        {
            get { return Color.ToString(); }
        }

        public override InputType InputSourceType
        {
            get { return InputType.SolidColor; }
        }

        public override Brush GetBursh()
        {
            var brush = new SolidColorBrush(Color);

			// have to give it a height and width for this to work
            var rect = new Rectangle {Fill = brush, Width = 2, Height = 2};
	        DoFadeEffect(rect);

            return new VisualBrush(rect);
        }

        public override FrameworkElement GetElement()
        {
            var brush = new SolidColorBrush(Color);

            // have to give it a height and width for this to work
            var rect = new Rectangle { Fill = brush, Width = 2, Height = 2 };
            return rect;
        }
    }
}
