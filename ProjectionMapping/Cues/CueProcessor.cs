using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProjectionMapping.Cues
{
    class CueProcessor
    {
        // cues could be a simple as a well formated string

        public void Start()
        {
            var workerThread = new Thread(Work);
            workerThread.Start();
        }

        public void Work()
        {
            const int listenPort = App.sendRecvPort;
            var listener = new UdpClient(listenPort);
            var groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            try
            {
                while (true)
                {
                    // Note that this is a synchronous or blocking call.
                    byte[] receiveByteArray = listener.Receive(ref groupEP);
                    Console.WriteLine(@"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              Received a broadcast from {0}", groupEP);
                    string receivedData = Encoding.ASCII.GetString(receiveByteArray, 0, receiveByteArray.Length);

                    ProcessCue(receivedData);
                    Console.WriteLine(@"data follows \n{0}\n\n", receivedData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            listener.Close();
        }

        public static void ProcessCue(string cue)
        {
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
                    switch (parts[1])
                    {
                        case "Next":
                            
                           // App.cues.
                            break;
                    }
                }
            }

        }

        private static void ProcessLayerCmd(string[] parts)
        {
            int layerID;

            if (parts.Length >= 2 && int.TryParse(parts[1], out layerID))
            {
                if (parts.Length >= 3 && string.Equals(parts[2], "source", StringComparison.OrdinalIgnoreCase))
                {
                    int sourceID;
                    if (parts.Length >= 4 && int.TryParse(parts[3], out sourceID))
                    {
                        var surface = App.surfaces.SingleOrDefault(s => s.ID == layerID);

                        if (surface != null)
                        {
                            var source = App.sources.SingleOrDefault(s => s.ID == sourceID);

                            if (source != null)
                            {
                                surface.ChangeSource(source);
                            }
                        }
                    }
                }
            }
        }
    }
}
