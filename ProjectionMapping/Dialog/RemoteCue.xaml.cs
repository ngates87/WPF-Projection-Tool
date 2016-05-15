using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Data;

//using Daktronics.Sports.SportPortControls;

namespace ProjectionMapping.Dialog
{
    /// <summary>
    /// Interaction logic for RemoteCue.xaml
    /// </summary>
    public partial class RemoteCue
    {
       // Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
       // IPAddress send_to_address = IPAddress.Parse("255..255.255.255");
        private UdpClient udp = new UdpClient()
        {
            ExclusiveAddressUse = false
        };
        private readonly ObservableCollection<string> history = new ObservableCollection<string>(); 

        public RemoteCue()
        {
            InitializeComponent();

            var historyVs = (CollectionViewSource)FindResource("historyViewSource");
            historyVs.Source = history;

	         
        }

        private void ButtonCueNextClick(object sender, RoutedEventArgs e)
        {
            UdpSend("/cue/next");
        }

        private void ButtonCueBackClick(object sender, RoutedEventArgs e)
        {
            UdpSend("/cue/back");
        }

        private void  UdpSend( string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                history.Add(string.Format("Command Fail {0}", cmd));
                return;
            }
            history.Insert(0,cmd);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(cmd);
			//udpPort.SendErtd(sendBytes);
			
	        udp.Send(sendBytes, sendBytes.Length);
        }

        private void SendCmdClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmd.Text))
            {
                UdpSend(cmd.Text);
            }
        }

        private void CueNClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cueN.Text))
            {
                int cue;
                if (int.TryParse(cueN.Text, out cue))
                {
                    UdpSend(string.Format("/cue/{0}", cue));
                }
            }
        }

	    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
	    {
			//udpPort.Description = "Send Port";
			//var pc = new PortConfiguration(udpPort)
			//{
			//	IsNameRequired = false,
			//	FileEnabled = false,
			//	Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
			//	NameEditingDisabled = true,
			//	Foreground = Brushes.White
			//};

			//var showDialog = pc.ShowDialog();
			//if (showDialog == null || !((bool)showDialog))
			//	return;

			//var bWasOpen = udpPort.IsOpen();
			//this.udpPort.ErtdProtocolProcessor.EventRealTimeData -= ErtdProtocolProcessor_EventRealTimeData;
			//this.udpPort.Close();
			//this.udpPort = pc.PortObject;

			//this.udpPort.ErtdProtocolProcessor.EventRealTimeData += ErtdProtocolProcessor_EventRealTimeData;

			//if (!udpPort.IsOpen() && bWasOpen)
			//{
			//	this.udpPort.Open();
			//}


	    }

        private void RemoteCue_OnLoaded(object sender, RoutedEventArgs e)
        {
            udp.Connect(new IPEndPoint(IPAddress.Broadcast, App.sendRecvPort));
          //  throw new NotImplementedException();
        }
    }
}
