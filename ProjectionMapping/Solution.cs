using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using System.Xml.XPath;
using ProjectionMapping.Sources;

namespace ProjectionMapping
{

	public class Solution
	{
		
		public string FilePath { get; set; }

		public string ProjectDir { get; set; }
		public string ImagesFolder { get; set; }
		public string VideosFolder { get; set; }
        //[XmlIgnore]
        //public ObservableCollection<InputSource> Sources = new ObservableCollection<InputSource>();
		// public string LayersPath { get; set; }

        [XmlArray] public List<LayerPreset> Layers;
        [XmlArray] public List<InputSource> Sources;
	    [XmlArray] public List<string> Cues;

		public static Solution CreateSolution(string sFileName)
		{
		    //new ObservableCollection<InputSource>(new List<InputSource>());
			if (sFileName != null)
			{
				var solution = new Solution
				{
					FilePath = sFileName,
					ProjectDir =
						Path.Combine(Path.GetDirectoryName(sFileName), Path.GetFileNameWithoutExtension(sFileName))
				};


				if (!Directory.Exists(solution.ProjectDir))
					Directory.CreateDirectory(solution.ProjectDir);

				solution.ImagesFolder = Path.Combine(solution.ProjectDir, "Images.cache");
				solution.VideosFolder = Path.Combine(solution.ProjectDir, "Videos.cache");
				// solution.LayersPath = Path.Combine(solution.ProjectDir, "Layers.xml");

				if (!Directory.Exists(solution.ImagesFolder))
				{
					Directory.CreateDirectory(solution.ImagesFolder);
				}

				if (!Directory.Exists(solution.VideosFolder))
				{
					Directory.CreateDirectory(solution.VideosFolder);
				}
				return solution;
			}
			return null;
		}

		public static Solution OpenSolution(string fileName)
		{
			Solution solution = null;
		    if (fileName != null)
		    {
                //solution = new Solution
                //{
                //    FilePath = fileName,
                //    ProjectDir = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName))
                //};

                //if (!Directory.Exists(solution.ProjectDir))
                //    Directory.CreateDirectory(solution.ProjectDir);

                //solution.ImagesFolder = Path.Combine(solution.ProjectDir, "Images.cache");
                //solution.VideosFolder = Path.Combine(solution.ProjectDir, "Videos.cache");

		        using (var fs = new FileStream(fileName, FileMode.Open))
		        {
		            var x = new XmlSerializer(typeof (Solution));
		            solution = x.Deserialize(fs) as Solution;
		        }
		    }
		    return solution;
		}

		public void Save(List<LayerPreset> expLayers, List<InputSource> sources )
		{
		    try
		    {
                Layers = expLayers;
                Sources = sources;
                Cues = App.cues.ToList();

                using (var fs = new FileStream(FilePath, FileMode.Create))
                {
                    var x = new XmlSerializer(typeof(Solution));
                    x.Serialize(fs, this);
                }
		    }
		    catch (Exception ex)
		    {

		        MessageBox.Show(ex.ToString());
		    }
			
		}
	}
}
