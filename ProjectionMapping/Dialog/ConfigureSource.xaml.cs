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
using System.Windows.Shapes;
using InputType = ProjectionMapping.Sources.InputType;

namespace ProjectionMapping.Dialog
{
	

	/// <summary>
	/// Interaction logic for ConfigureSource.xaml
	/// </summary>
	public partial class ConfigureSource : Window
	{
	    public InputType Type { get;set; }
		public ConfigureSource()
		{
			InitializeComponent();
		}
	}
}
