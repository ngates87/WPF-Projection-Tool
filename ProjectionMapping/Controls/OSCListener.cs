using ProjectionMapping.Cues;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectionMapping.Controls
{
    public class OSCListener : ICommandListener
    {
        static OscReceiver receiver = new OscReceiver(port);
        const int port = App.sendRecvPort;
        OscAddressManager manager = new OscAddressManager();

        private ICueControl cuesControl;

        public ICueControl CueControl
        {
            get
            {
                return cuesControl;
            }
        }

        public OSCListener(ICueControl control)
        {
            this.cuesControl = control;
            App.manager.Attach("/cue/next", new OscMessageEvent(this.NextCue));
            App.manager.Attach("/cue/prevS", new OscMessageEvent(this.PrevCue));
            // manager.Attach("/cue/next", new OscMessageEvent(this.NextCue));
            //manager.Attach("/cue/back", new OscMessageEvent(this.PrevCue));
        }

        public void DoWork()
        {
            try
            {
                receiver.Connect();
                while (true/*receiver.State != OscSocketState.Closed*/)
                {
                    // if we are in a state to recieve
                    if (receiver.State == OscSocketState.Connected)
                    {
                        // get the next message 
                        // this will block until one arrives or the socket is closed
                        OscPacket packet = receiver.Receive();

                        // Write the packet to the console 
                        Console.WriteLine(packet.ToString());
                        App.manager.Invoke(packet);
                       // manager.Invoke(packet);
                    }
                    else
                    {
                        receiver.Connect();
                    }
                }
            }
            catch (Exception ex)
            {
                // if the socket was connected when this happens
                // then tell the user
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void NextCue(OscMessage message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.cuesControl.NextCue();
            });
        }

        private void PrevCue(OscMessage message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.cuesControl.PrevCue();
            });
        }

        public void ProcessCmd(string cue)
        {


            Application.Current.Dispatcher.Invoke(() =>
            { /* Your code here */
                // this.Dispatcher.Invoke()
                if (string.IsNullOrWhiteSpace(cue))
                    return;

                string[] parts = cue.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                if (parts.Length >= 1 && string.Equals(parts[0], "layer", StringComparison.OrdinalIgnoreCase))
                {
                    ProcessLayerCmd(parts);
                }
                else if (parts.Length >= 1 && string.Equals(parts[0], "cue", StringComparison.OrdinalIgnoreCase))
                {
                    if (parts.Length >= 2)
                    {
                        int cueX;
                        if (string.Equals(parts[1], "next", StringComparison.OrdinalIgnoreCase))
                            this.cuesControl.NextCue();
                        else if (string.Equals(parts[1], "back", StringComparison.OrdinalIgnoreCase))
                            this.cuesControl.PrevCue();
                        else if (int.TryParse(parts[1], out cueX))
                            this.cuesControl.CueGoto(cueX);

                    }
                }
            });
        }

        private static void ProcessLayerCmd(string[] parts)
        {
            int layerID;

            if (parts.Length >= 2 && int.TryParse(parts[1], out layerID))
            {
                var surface = App.surfaces.SingleOrDefault(s => s.ID == layerID);
                if (surface == null)
                    return;

                if (parts.Length >= 3 && string.Equals(parts[2], "source", StringComparison.OrdinalIgnoreCase))
                {
                    int sourceID;
                    if (parts.Length < 4 || !int.TryParse(parts[3], out sourceID))
                        return;

                    var source = App.sources.SingleOrDefault(s => s.ID == sourceID);

                    if (source != null)
                    {
                        surface.ChangeSource(source);
                    }
                }

                else if (parts.Length >= 3 && string.Equals(parts[2], "blank", StringComparison.OrdinalIgnoreCase))
                {
                    surface.RemoveAll();
                }

                else if (parts.Length >= 3 && string.Equals(parts[2], "remove", StringComparison.OrdinalIgnoreCase))
                {
                    App.model3DCollection.Remove(surface.Layer3DModel);
                }
                else if (parts.Length >= 3 && string.Equals(parts[2], "add", StringComparison.OrdinalIgnoreCase))
                {
                    App.model3DCollection.Add(surface.Layer3DModel);
                }
            }
        }
    }
}
