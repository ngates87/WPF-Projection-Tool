using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCue
{
    public class OSCSender : ICommandSender
    {

        IPAddress address = IPAddress.Broadcast;//IPAddress.Parse("127.0.0.1"); 
        const int port = 1701;


        private readonly ObservableCollection<string> history = new ObservableCollection<string>();

        public ObservableCollection<string> History
        {
            get
            {
                return history;
            }
        }

        public void SendCueNext()
        {
            SendCommand("/cue/next");
        }

        private void SendCommand(string commandToSend)
        {
            using (OscSender sender = new OscSender(address, port))
            {
                sender.Connect();

                //new Rug.Osc.OscBundle()
                // sender.Send(new OscMessage("/test", 1, 2, 3, 4));

                sender.Send(new OscMessage(commandToSend));
            }
        }

        public void SendCueBack()
        {
            SendCommand("/cue/prev");
        }

        public void GoToCue(int i)
        {
            throw new NotImplementedException();
        }

        public void RawSend(string val)
        {
            throw new NotImplementedException();
        }
    }
}
