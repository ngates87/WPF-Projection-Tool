using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using ProjectionMapping.Sources;

namespace ProjectionMapping
{
    public struct LayerPreset
    {
        public string Name;
        public int ID;
        public MatrixTransform3D Matrix;
        public Point3D[] PointsTransformed;
    }

    public class Layer
    {
        #region variables
        // acts like simple ordinal ID
        private static int instanceCount;
        public readonly int ID;

        public MatrixTransform3D MatrixTransform;
        public int NumTextures = -1;
        public Point3D[] PointsTransformed = new Point3D[4];
        public MaterialGroup Materials = new MaterialGroup();

        #endregion

        #region properties
        public string LayerName
        {
            get;
            set;
        }

        public GeometryModel3D Layer3DModel { get; set; }

        #endregion

        #region Constructor

        public Layer(string name)
        {

            ID = ++instanceCount;

            LayerName = "Layer" + ID;

           // App.manager.Attach(string.Format("/Layer/{0}/source", ID), new Rug.Osc.OscMessageEvent());

            var mesh = DefineMesh();
            MatrixTransform = new MatrixTransform3D();
            Materials.Children.Add(new DiffuseMaterial(Brushes.DarkGray)); // Should be black, just doing dark gray so I can see it. 

            Layer3DModel = new GeometryModel3D(mesh, Materials)
            {
                Transform = MatrixTransform
            };

            //LayerName = name;

            for (int i = 0; i < 4; i++)
            {
                PointsTransformed[i] = MatrixTransform.Transform(mesh.Positions[i]);
            }
            MatrixTransform.Matrix = CalculateNonAffineTransform(PointsTransformed);
        }


        // for re-loading a layer's positional geometry, needs to be cleaned up
        public Layer(LayerPreset pre)
        {
            ID = pre.ID;
            instanceCount++;
            LayerName = pre.Name;
            var mesh = DefineMesh();
            // MatrixTransform = new MatrixTransform3D();
            MatrixTransform = pre.Matrix;
            Materials = new MaterialGroup();
            Layer3DModel = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.White))
            {
                Transform = MatrixTransform,
                Material = Materials
            };
            PointsTransformed = pre.PointsTransformed;
        }

        public LayerPreset Export()
        {
            LayerPreset exp;
            exp.Matrix = this.MatrixTransform;
            exp.Name = this.LayerName;
            exp.ID = this.ID;
            exp.PointsTransformed = this.PointsTransformed;
            return exp;

        }
        private static MeshGeometry3D DefineMesh()
        {
            var mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(0, 0, 0));
            mesh.Positions.Add(new Point3D(0, 1, 0));
            mesh.Positions.Add(new Point3D(1, 0, 0));
            mesh.Positions.Add(new Point3D(1, 1, 0));
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(1);
            mesh.TextureCoordinates.Add(new Point(0, 1));
            mesh.TextureCoordinates.Add(new Point(0, 0));
            mesh.TextureCoordinates.Add(new Point(1, 1));
            mesh.TextureCoordinates.Add(new Point(1, 0));
            return mesh;
        }

        #region Layer Creation

        #endregion

        #endregion

        #region BackGroundHelpers(GRID) SHOW/HIDE

        public void ShowBackGroundHelper(Material backgroundHelper)
        {
            if (!Materials.Children.Contains(backgroundHelper)) // if there is already there is no need to add another one
                Materials.Children.Add(backgroundHelper);
        }
        public void HideBackGroundHelper(Material backgroundHelper)
        {
            Materials.Children.Remove(backgroundHelper);
        }

        #endregion

        #region Layer Transformations

        public void Move(double offsetX, double offsetY, int index)
        {
            var pt = PointsTransformed[index];
            pt.X = offsetX;
            pt.Y = offsetY;
            PointsTransformed[index] = pt;
            MatrixTransform.Matrix = CalculateNonAffineTransform(PointsTransformed);

        }

        Matrix3D CalculateNonAffineTransform(Point3D[] points)
        {
            var matrixA = new Matrix3D
            {
                M11 = points[2].X - points[0].X,
                M12 = points[2].Y - points[0].Y,
                M21 = points[1].X - points[0].X,

                M22 = points[1].Y - points[0].Y,
                OffsetX = points[0].X,
                OffsetY = points[0].Y,
                OffsetZ = points[0].Z
            };

            double d = matrixA.M11 * matrixA.M22 - matrixA.M12 * matrixA.M21;
            double a = (matrixA.M22 * points[3].X - matrixA.M21 * points[3].Y + matrixA.M21 * matrixA.OffsetY - matrixA.M22 * matrixA.OffsetX) / d;

            double b = (matrixA.M11 * points[3].Y - matrixA.M12 * points[3].X + matrixA.M12 * matrixA.OffsetX - matrixA.M11 * matrixA.OffsetY) / d;

            var matrixB = new Matrix3D
            {
                M11 = a / (a + b - 1),
                M22 = b / (a + b - 1)
            };
            matrixB.M14 = matrixB.M11 - 1;
            matrixB.M24 = matrixB.M22 - 1;

            return matrixB * matrixA;
        }

        #endregion

        #region Layer Materials

        //public void ChangeSource(InputSource source)
        //{

        //    //will this work?
        //    //  Thinking of keeping just the black brush... but maybe I don't need too?
        //    while (Materials.Children.Count > 1)
        //    {
        //        Materials.Children.RemoveAt(0);
        //    }

        //    var material = new DiffuseMaterial(source.GetBursh());
        //    Materials.Children.Add(material);


        //}

        public void ChangeSource(Rug.Osc.OscMessage msg)
        {
           
        }

        public void ChangeSource(InputSource source)
        {

            //will this work?
            //  Thinking of keeping just the black brush... but maybe I don't need too?

            var element = source.GetElement();

            var material = new DiffuseMaterial(new VisualBrush(element));

            DoFadeEffect(element, source.Opacity, source.FadeInSec);

            Materials.Children.Add(material);
        }
        protected void DoFadeEffect(FrameworkElement element, double opacity, double fade)
        {
            var fadeAnimation = new DoubleAnimation()
            {
                From = 0.0,
                To = opacity,
                Duration = TimeSpan.FromSeconds(fade)
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeAnimation);
            Storyboard.SetTarget(fadeAnimation, element);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Completed += storyboard_Completed;

            storyboard.Begin();
        }

        void storyboard_Completed(object sender, EventArgs e)
        {
            while (Materials.Children.Count > 1)
            {
                Materials.Children.RemoveAt(0);
            }
        }

        public void RemoveAll()
        {
            while (Materials.Children.Count != 0)
            {
                Materials.Children.RemoveAt(0);
            }
        }

        #endregion
    }
}
