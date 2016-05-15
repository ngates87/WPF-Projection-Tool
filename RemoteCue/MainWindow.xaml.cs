using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace RemoteCue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICommandSender commandSender;

        // Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // IPAddress send_to_address = IPAddress.Parse("255..255.255.255");
       
        // private Daktronics.V7000.ERTD.ERTD_UDP ertdUDP = new ERTD_UDP(1701);


        private readonly ObservableCollection<string> history = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            commandSender = new OSCSender();//new CustomCommandSender();
            var historyVs = (CollectionViewSource)FindResource("historyViewSource");
            historyVs.Source = commandSender.History;

            // ertdUDP.AddressSend = "255.255.255.255";
            // ertdUDP.AddressBind = "255.255.255.255";
            // ertdUDP.Open();

            //bool b = ertdUDP.IsOpen();

        }

        private void ButtonCueNextClick(object sender, RoutedEventArgs e)
        {
            commandSender.SendCueNext();
            //UdpSend("/cue/next");
        }

        private void ButtonCueBackClick(object sender, RoutedEventArgs e)
        {
            commandSender.SendCueBack();
            //UdpSend("/cue/back");
        }

        

        private void SendCmdClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmd.Text))
            {
               // UdpSend(cmd.Text);
                commandSender.RawSend(cmd.Text);
            }
        }

        private void CueNClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cueN.Text))
            {
                int cue;
                if (int.TryParse(cueN.Text, out cue))
                {
                    commandSender.GoToCue(cue);
                    //UdpSend(string.Format("/cue/{0}", cue));
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
            //  throw new NotImplementedException();
        }
    }
}

