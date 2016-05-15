using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCue
{
    public interface ICommandSender
    {
        ObservableCollection<string> History { get;  }

        void SendCueNext();
        void SendCueBack();
        void GoToCue(int i);

        void RawSend(string val);
    }
}
