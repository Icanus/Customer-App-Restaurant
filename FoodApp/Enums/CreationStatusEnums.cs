using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Enums
{
    enum CreationStatusEnums
    {
        Error = 0,    
        Success = 1,
        AlreadyExist = 2,
        DoesNotExist = 3,
        IsNotRegisteredLoyaltyMember = 4,
        InviteExist = 5,
    }
}
