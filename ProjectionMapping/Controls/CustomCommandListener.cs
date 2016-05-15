using ProjectionMapping.Cues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectionMapping.Controls
{
    public class CustomCommandListener : ICommandListener
    {
        private bool listen = true;
        private const int ListenPort = App.sendRecvPort;
        
        private ICueControl cueControl;

        public ICueControl CueControl
        {
            get
            {
                return cueControl;
            }
        }


        public CustomCommandListener(ICueControl cueControl)
        {
            this.cueControl = cueControl;
        }

        public void DoWork()
        {
            try
            {
                using (var listener = new UdpClient(ListenPort))
                {

                    var groupEP = new IPEndPoint(IPAddress.Broadcast, ListenPort);
                    try
                    {
                        while (listen)
                        {

                            // Note that this is a synchronous or blocking call.
                            var receiveByteArray = listener.Receive(ref groupEP);
                            var receivedData = Encoding.ASCII.GetString(receiveByteArray, 0, receiveByteArray.Length);

                            ProcessCmd(receivedData);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    finally
                    {
                        listener.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

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
                            this.cueControl.NextCue();
                        else if (string.Equals(parts[1], "back", StringComparison.OrdinalIgnoreCase))
                            this.cueControl.PrevCue();
                        else if (int.TryParse(parts[1], out cueX))
                            this.cueControl.CueGoto(cueX);

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
