using System;

namespace ProjectionMapping
{
    public class MediaFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Ext { get; set; }

        public bool IsStill()
        {
            //"Multi-Media Files|*.avi; *.mpeg; *.mp4;*.wmv; *.mpg; *.jpg;*.jpeg;*.png;*.bmp"
            return (string.Equals(Ext, ".jpg", StringComparison.OrdinalIgnoreCase)  ||
                  string.Equals(Ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                  string.Equals(Ext, ".png", StringComparison.OrdinalIgnoreCase)  ||
                  string.Equals(Ext, ".bmp", StringComparison.OrdinalIgnoreCase) );
        }

        public bool IsGif()
        {
            return string.Equals(Ext, ".gif", StringComparison.OrdinalIgnoreCase);
        }

    }
}
