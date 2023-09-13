using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Interface
{
    public interface ILocationSettingsService
    {
        bool IsGpsTurnedOn();
        Task OpenLocationSettingsAsync();
    }
}
