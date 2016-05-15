using System;
using System.Windows;
using System.Windows.Controls;
using ProjectionMapping.Sources;

namespace ProjectionMapping
{
    public class InputTypeDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is InputSource)
            {
                switch (((InputSource)item).InputSourceType)
                {
                    case InputType.Video:
                        return element.FindResource("sourceVideoTemplate") as DataTemplate;
	                case InputType.Still:
                        return element.FindResource("sourceStillTemplate") as DataTemplate;
	                case InputType.Cam:
                        return element.FindResource("sourceWebcamTemplate") as DataTemplate;
	                case InputType.SolidColor:
                        return element.FindResource("sourceColorTemplate") as DataTemplate;
	                case InputType.Gif:
                        return element.FindResource("sourceGifTemplate") as DataTemplate;
	                default:
                        return element.FindResource("sourceStillTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
