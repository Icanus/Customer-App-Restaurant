using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Banner : Entity
    {
        /// <summary>
        /// Image of the banner
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Parameter to pass to the Shell's GoTo method when the banner is touched.
        /// </summary>
        public string GoTo { get; set; }
    }
}
