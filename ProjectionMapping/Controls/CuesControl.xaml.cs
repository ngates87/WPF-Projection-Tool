using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace ProjectionMapping.Controls
{
    /// <summary>
    /// Interaction logic for CuesControl.xaml
    /// </summary>
    public partial class CuesControl : IDisposable, ProjectionMapping.Cues.ICueControl
    {
        private bool listen = true;
        private const int ListenPort = App.sendRecvPort;

        readonly ICommandListener commandListener;

        public CuesControl()
        {
            InitializeComponent();
            //commandListener = new CustomCommandListener(this);
            commandListener = new OSCListener(this);
        }

        // handle next and previous cue clicks
        private void CueNextClick(object sender, RoutedEventArgs e)
        {
            NextCue();
        }

        public void NextCue()
        {
            var nextIndex = lvCues.SelectedIndex + 1;
            if (lvCues.Items.Count != nextIndex)
            {
                lvCues.SelectedIndex = nextIndex;

                //  var val = (string)lvCues.SelectedItem;
                //  ProcessCmd(val);
            }
        }

        public void CueGoto(int cueN)
        {
            if (cueN >= 0 && cueN < lvCues.Items.Count)
            {
                lvCues.SelectedIndex = cueN;

                var val = (string)lvCues.SelectedItem;
                commandListener.ProcessCmd(val);
            }
        }


        private void CuePrevClick(object sender, RoutedEventArgs e)
        {
            PrevCue();
        }

        public void PrevCue()
        {
            var prevIndex = lvCues.SelectedIndex - 1;

            if (prevIndex >= 0)
            {
                lvCues.SelectedIndex = prevIndex;
            }
        }


        private void AddCueLine(object sender, RoutedEventArgs e)
        {
            string line = txtBoxCue.Text;

            if (!string.IsNullOrWhiteSpace(line))
                App.cues.Add(line);
        }

        private void RemoveCueLine(object sender, RoutedEventArgs e)
        {
            if (App.cues.Count != 0 && lvCues.SelectedIndex >= 0)
                App.cues.RemoveAt(lvCues.SelectedIndex);
        }

        private void CueControlUnloaded(object sender, RoutedEventArgs e)
        {
            listen = false;
        }

        private void lvCues_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var val = (string)lvCues.SelectedItem;
            this.commandListener.ProcessCmd(val);
        }

        public void Dispose()
        {
            //listener.Close();
        }

        private void CuesControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var cuesCollection = (CollectionViewSource)FindResource("cuesCollection");
            cuesCollection.Source = App.cues;
            var workerThread = new Thread(new ThreadStart(commandListener.DoWork)) { IsBackground = true };

            workerThread.Start();
        }

        private void MoveUpClick(object sender, RoutedEventArgs e)
        {
            string cue = lvCues.SelectedItem as string;

            if (cue != null)
            {
                if (lvCues.SelectedIndex > 0)
                {
                    var temp = lvCues.Items[lvCues.SelectedIndex - 1] as string;
                    App.cues[lvCues.SelectedIndex - 1] = cue;
                    App.cues[lvCues.SelectedIndex] = temp;
                    lvCues.SelectedIndex -= 1;

                }
            }
        }

        private void MoveDownClick(object sender, RoutedEventArgs e)
        {
            string cue = lvCues.SelectedItem as string;

            if (cue != null)
            {
                if (lvCues.SelectedIndex < App.cues.Count - 1)
                {
                    var temp = lvCues.Items[lvCues.SelectedIndex + 1] as string;
                    App.cues[lvCues.SelectedIndex + 1] = cue;
                    App.cues[lvCues.SelectedIndex] = temp;
                    lvCues.SelectedIndex += 1;
                }
            }
        }
    }
}
