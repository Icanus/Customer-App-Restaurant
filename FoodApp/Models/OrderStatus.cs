using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public enum OrderStatus
    {
        Placed,
        Processing,
        OnTheWay,
        Delivered,
        Cancelled
    }
}
