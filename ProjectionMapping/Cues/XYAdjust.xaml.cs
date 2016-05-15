using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectionMapping
{
    enum Corners
    {
        BottomLeft = 0,
        TopLeft = 1,
        BottomRight = 2,
        TopRight = 3,
    }
    /// <summary>
    /// Interaction logic for XYAdjust.xaml
    /// </summary>
    public partial class XYAdjust : UserControl
    {
        public XYAdjust()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof (string), typeof (XYAdjust), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public event EventHandler Move;

        protected virtual void Moved(EventArgs e)
        {
            EventHandler handler = Move;
            if (handler != null)
            {
                handler(this, e);
            }
        }

       // public string Header { get; set; }
    }
}
