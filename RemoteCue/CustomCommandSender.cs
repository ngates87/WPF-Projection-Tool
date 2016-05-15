using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCue
{
    public class CustomCommandSender : ICommandSender
    {
        private readonly ObservableCollection<string> history = new ObservableCollection<string>();

        public ObservableCollection<string> History
        {
            get
            {
                return history;
            }
        }

        public CustomCommandSender()
        {
            udp.Connect(new IPEndPoint(IPAddress.Broadcast, 1701));

        }

        private readonly UdpClient udp = new UdpClient()
        {
            ExclusiveAddressUse = false
        };


        private void UdpSend(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                history.Add(string.Format("Command Fail {0}", cmd));
                return;
            }
            history.Insert(0, cmd);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(cmd);
            //udpPort.SendErtd(sendBytes);
            //ertdUDP.SendErtd(sendBytes);

            udp.Send(sendBytes, sendBytes.Length);
        }

        public void SendCueNext()
        {
            UdpSend("/cue/next");
        }


        public void SendCueBack()
        {
            UdpSend("/cue/back");
        }

        public void GoToCue(int cue)
        {
            UdpSend(string.Format("/cue/{0}", cue));
        }


        public void RawSend(string val)
        {
            UdpSend(val);
        }
    }
}
