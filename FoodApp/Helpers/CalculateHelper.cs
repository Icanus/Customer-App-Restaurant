using FoodApp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Helpers
{
    public class CalculateHelper
    {
        private const double BaseRate = 0.10;
        private const double RatePerMeter = 0.01;  // Adjust this according to your needs

        public async Task<double> CalculateDeliveryFee(string origin, string destination)
        {
            try
            {
                var distanceMatrixService = new JsonWebApiAction();
                var (distanceInMeters, duration) = await distanceMatrixService.GetDistanceAndDurationAsync(origin, destination);

                double distanceFee = distanceInMeters * RatePerMeter;
                double totalFee = BaseRate + distanceFee;

                return totalFee;
            }
            catch(Exception)
            {

            }
            return 0.0;
        }
    }
}
