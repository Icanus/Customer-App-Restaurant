using FoodApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Interface
{
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
