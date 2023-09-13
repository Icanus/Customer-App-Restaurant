using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class FlyoutItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
    }
}
