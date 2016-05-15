using ProjectionMapping.Cues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionMapping.Controls
{
    public interface ICommandListener
    {
        ICueControl CueControl { get; }
        void DoWork();
        void ProcessCmd(string val);
    }
}
