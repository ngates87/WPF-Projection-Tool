//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Media;
//using Awesomium.Web;

//namespace ProjectionMapping.Sources
//{
//    public class Webpage:InputSource
//    {
//        //public override string DisplayName
//        //{
//        //    get { return "page"; }
//        //}

//        public override InputType InputSourceType
//        {
//            get { return InputType.Webpage; }
//        }

//        public override System.Windows.Media.Brush GetBursh()
//        {
//            //WebBrowser browser = new WebBrowser();
//            //browser.Navigate("www.google.com");
//            Awesomium.Windows.Controls.WebControl wc = new Awesomium.Windows.Controls.WebControl();
//            wc.Source = new Uri("www.google.com");
//            //wc.lo
//            return new VisualBrush(wc);
//            //throw new NotImplementedException();
//        }

//        public override System.Windows.FrameworkElement GetElement()
//        {
//            Awesomium.Windows.Controls.WebControl wc = new Awesomium.Windows.Controls.WebControl();
//            wc.ViewType = Awesomium.Core.WebViewType.Window;
//            wc.Source = new Uri("http://www.google.com");
//            return wc;
//        }
//    }
//}
