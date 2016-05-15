using System;
namespace ProjectionMapping.Cues
{
    public interface ICueControl
    {
        void CueGoto(int cueN);
        void NextCue();
        void PrevCue();
    }
}
