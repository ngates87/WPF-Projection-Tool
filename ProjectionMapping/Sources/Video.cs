using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace ProjectionMapping.Sources
{
	[Serializable]
	public class Video : InputSource
	{
	    private bool loop = false;

	    public bool Loop
	    {
	        get { return loop; }
	        set
	        {
	            loop = value;
	            OnPropertyChanged("Loop");
	        }
	    }

	    public double Speed { get; set; }

        [XmlIgnore]
        [NonSerialized]
    	private  MediaElement mediaPlayer = new MediaElement()
    	{
    	    //LoadedBehavior = 
    	};

        //private string source;

        //public override string Source
        //{
        //    get
        //    {
        //        return source;
        //    }
        //    set
        //    {
        //        source = value;
        //        mediaPlayer.Source = new Uri(value);
        //        mediaPlayer.Position = TimeSpan.Zero;
        //        mediaPlayer.Play();
        //        mediaPlayer.Pause();
        //    }
        //}

	    [XmlIgnore]
		public override string DisplayName
		{
			get { return Path.GetFileNameWithoutExtension(this.Source); }
		}

		public Video()
		{
			Construct(string.Empty, true, 1);
		}

		public Video(string source, bool loop, double speed)
		{
			Construct(source, loop, speed);
		}

		public Video(string source, bool loop)
		{
			Construct(source, loop, 1.0);
		}

		public Video(string source)
		{
			Construct(source, false, 1.0);
		}

		private void Construct(string source, bool loop, double speed)
		{

        ////	mediaPlayer.LoadedBehavior = MediaState.Manual;
        //    mediaPlayer.MediaEnded += mediaPlayer_MediaEnded;

        //    mediaPlayer.Volume = 0;
			Speed = 1.0;
			Loop = loop;
			Source = source;
		}

		// loops the video
		void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
		    var mediaPlayer = sender as MediaElement;

            if (mediaPlayer == null || !Loop || mediaPlayer.LoadedBehavior != MediaState.Manual) 
                return;


		    mediaPlayer.Position = TimeSpan.Zero;
		    mediaPlayer.Play();
		}


		public override Brush GetBursh()
		{
			if (string.IsNullOrWhiteSpace(Source) || !File.Exists(Source))
				return null;
			//var imgBr = new ImageBrush(new BitmapSource)

			var mediaPlayerl = new MediaElement
			{
				//LoadedBehavior = MediaState.Manual,
				SpeedRatio = Speed,
				Opacity = Opacity,
                Volume = 0,
				Source = new Uri(Source)
			};
			//mediaPlayerl.MediaEnded += mediaPlayer_MediaEnded;
			//mediaPlayerl.Play();

			DoFadeEffect(mediaPlayerl);
			return new VisualBrush(mediaPlayerl);


		}

	    public override FrameworkElement GetElement()
	    {
            if (string.IsNullOrWhiteSpace(Source) || !File.Exists(Source))
                return null;
            //var imgBr = new ImageBrush(new BitmapSource)

            var mediaPlayerl = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                SpeedRatio = Speed,
                Opacity = Opacity,
                Volume = 0,
                Source = new Uri(Source)
            };
            mediaPlayerl.MediaEnded += mediaPlayer_MediaEnded;
            mediaPlayerl.Play();

	        return mediaPlayerl;
	    }

	    [XmlIgnore]
		public override InputType InputSourceType
		{
			get { return InputType.Video; }
		}


    }
}
