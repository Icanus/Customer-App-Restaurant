using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace FoodApp.iOS.Renderer
{
    public class CustomMKAnnotationView : MKAnnotationView
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id)
        {
        }
    }
}