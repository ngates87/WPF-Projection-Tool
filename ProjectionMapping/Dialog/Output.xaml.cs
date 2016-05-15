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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace ProjectionMapping
{
	/// <summary>
	/// Interaction logic for Ouput.xaml
	/// </summary>
	public partial class Output : Window
	{
		public Output(Model3DCollection models)
		{
			InitializeComponent();
			Models3DGroup.Children = models;

		}

		private void outputWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				outputWindow.WindowStyle = WindowStyle.SingleBorderWindow;
				outputWindow.Topmost = false;
				outputWindow.WindowState = WindowState.Normal;
			    Mouse.OverrideCursor = Cursors.Arrow;
			}
		}

		private void outputWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F5)
			{
				outputWindow.WindowStyle = WindowStyle.None;
				outputWindow.Topmost = true;
				outputWindow.WindowState = WindowState.Maximized;
                Mouse.OverrideCursor = Cursors.None;

			}
		}
	}
}
