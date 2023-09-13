using FoodApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace FoodApp.Utilities
{
    public class JsonWebApiAction
    {
        public static async Task<Customer> CheckUserInfo(string email, string password)
        {
            try
            {
                var account = new
                {
                    Email = email,
                    Password = password
                };

                return await App.jsonWebApiAgent.SendGetAsyncRequest<Customer>($"/api/Customer/Email/{email}/Password/{password}");
            }
            catch (Exception e)
            {
                return new Customer();
            }
        }

        public static async Task<int> CreateAccount(Customer customer)
        {
            try
            {
                var account = new
                {
                    id = 0,
                    customerId = customer.CustomerId,
                    fullName = string.IsNullOrEmpty(customer.FullName) ? "" : customer.FullName,
                    username = string.IsNullOrEmpty(customer.Username) ? "" : customer.Username,
                    email = string.IsNullOrEmpty(customer.Email) ? "" : customer.Email,
                    password = string.IsNullOrEmpty(customer.Password) ? "" : customer.Password,
                    phone = string.IsNullOrEmpty(customer.Phone) ? "" : customer.Phone,
                    dateOfBirth = "2023-07-25T07:53:40.399Z",
                    gender = string.IsNullOrEmpty(customer.Gender) ? "" : customer.Gender,
                    accountPreferences = string.IsNullOrEmpty(customer.AccountPreferences) ? "" : customer.AccountPreferences,
                    termsAndCondition = customer.TermsAndCondition,
                    image = string.IsNullOrEmpty(customer.Image) ? "" : customer.Image,
                    referralCode = string.IsNullOrEmpty(customer.ReferralCode) ? "" : customer.ReferralCode,
                    country = string.IsNullOrEmpty(customer.Country) ? "Fiji" : customer.Country,
                    countryCode = string.IsNullOrEmpty(customer.CountryCode) ? "679" : customer.CountryCode
                };

                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/Customer", account);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<int> UpdateAccount(string customerId, Customer customer)
        {
            try
            {
                var account = new
                {
                    id = customer.Id,
                    customerId = customer.CustomerId,
                    fullName = string.IsNullOrEmpty(customer.FullName) ? "" : customer.FullName,
                    username = string.IsNullOrEmpty(customer.Username) ? "" : customer.Username,
                    email = string.IsNullOrEmpty(customer.Email) ? "" : customer.Email,
                    password = string.IsNullOrEmpty(customer.Password) ? "" : customer.Password,
                    phone = string.IsNullOrEmpty(customer.Phone) ? "" : customer.Phone,
                    dateOfBirth = "2023-07-25T07:53:40.399Z",
                    gender = string.IsNullOrEmpty(customer.Gender) ? "" : customer.Gender,
                    accountPreferences = string.IsNullOrEmpty(customer.AccountPreferences) ? "" : customer.AccountPreferences,
                    termsAndCondition = customer.TermsAndCondition,
                    image = string.IsNullOrEmpty(customer.Image) ? "" : customer.Image,
                    country = string.IsNullOrEmpty(customer.Country) ? "Fiji" : customer.Country,
                    countryCode = string.IsNullOrEmpty(customer.CountryCode) ? "679" : customer.CountryCode,
                    referralCode = customer.ReferralCode
                };

                return await App.jsonWebApiAgent.SendPutAsyncRequest<int>($"/api/Customer/{customerId}", account);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<List<Category>> GetAllCategoryAsync()
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAllAsyncRequest<Category>($"/api/Category");
                return res;
            }
            catch (Exception e)
            {
                return new List<Category>();
            }
        }

        public static async Task<List<Address>> GetAllAddressAsync(string customerId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAllAsyncRequest<Address>($"/api/Address/{customerId}");
                return res;
            }
            catch (Exception e)
            {
                return new List<Address>();
            }
        }

        public static async Task<int> CreateAddress(Address address)
        {
            try
            {
                var addrss = new
                {
                    id = 0,
                    addressId = address.AddressId,
                    customerId = address.CustomerId,
                    title = string.IsNullOrEmpty(address.Title) ? "" : address.Title,
                    postCode = string.IsNullOrEmpty(address.PostCode) ? "" : address.PostCode,
                    address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1,
                    state = string.IsNullOrEmpty(address.State) ? "" : address.State,
                    street = string.IsNullOrEmpty(address.Street) ? "" : address.Street,
                    city = string.IsNullOrEmpty(address.City) ? "" : address.City,
                    country = string.IsNullOrEmpty(address.Country) ? "" : address.Country,
                    comment = string.IsNullOrEmpty(address.Comment) ? "" : address.Comment,
                    lon = address?.Lon == null ? "0" : Convert.ToString(address.Lon),
                    lat = address?.Lat == null ? "0" : Convert.ToString(address.Lat),
                    isDeleted = 0
                };

                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/Address", addrss);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<int> UpdateAddress(Address address, bool isDeleted = false)
        {
            try
            {
                var addrss = new
                {
                    id = 0,
                    addressId = address.AddressId,
                    customerId = address.CustomerId,
                    title = string.IsNullOrEmpty(address.Title) ? "" : address.Title,
                    postCode = string.IsNullOrEmpty(address.PostCode) ? "" : address.PostCode,
                    address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1,
                    state = string.IsNullOrEmpty(address.State) ? "" : address.State,
                    street = string.IsNullOrEmpty(address.Street) ? "" : address.Street,
                    city = string.IsNullOrEmpty(address.City) ? "" : address.City,
                    country = string.IsNullOrEmpty(address.Country) ? "" : address.Country,
                    comment = string.IsNullOrEmpty(address.Comment) ? "" : address.Comment,
                    lon = address?.Lon == null ? "0" : Convert.ToString(address.Lon),
                    lat = address?.Lat == null ? "0" : Convert.ToString(address.Lat),
                    isDeleted = isDeleted ? 1 : 0
                };

                return await App.jsonWebApiAgent.SendPutAsyncRequest<int>($"/api/Address/{address.AddressId}", addrss);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<int> InsertFavorite(Favorite favorite)
        {
            try
            {
                var fave = new
                {
                    id = 0,
                    favoriteId = favorite.FavoriteId,
                    customerId = favorite.CustomerId,
                    ItemId = favorite.ItemId
                };

                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/Favorite", fave);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<int> DeleteFavorite(string favoriteId)
        {
            try
            {
                return await App.jsonWebApiAgent.SendDeleteAsyncRequest<int>($"/api/Favorite/{favoriteId}");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<List<Favorite>> GetAllFavoriteAsync(string CustomerId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAllAsyncRequest<Favorite>($"/api/Favorite/{CustomerId}");
                return res;
            }
            catch (Exception e)
            {
                return new List<Favorite>();
            }
        }


        public static async Task<List<Items>> GetAllItems()
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAllAsyncRequest<Items>($"/api/Item");
                return res;
            }
            catch (Exception e)
            {
                return new List<Items>();
            }
        }

        public static async Task<int> CreateOrder(object obj, bool IsDeductRewards)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/Order/{IsDeductRewards}", obj);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<int> UpdateOrderAddressByOrderId(ChangeAddressModel obj)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPutAsyncRequest<int>($"/api/Order/OrderId/{obj.OrderId}", obj);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<List<OrderParameter>> GetOngoingOrder(string customerId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAllAsyncRequest<OrderParameter>($"/api/Order/OngoingOrder/{customerId}");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<List<OrderParameter>> GetAllOrders(string customerId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAllAsyncRequest<OrderParameter>($"/api/Order/{customerId}");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<int> SubmitFeedback(Feedback feedback)
        {
            try
            {

                return await App.jsonWebApiAgent.SendPutAsyncRequest<int>($"/api/Feedback/{feedback.FeedbackId}", feedback);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<int> GetMaxOrderId()
        {
            try
            {
                return await App.jsonWebApiAgent.SendGetAsyncRequest<int>($"/api/Order");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<CustomerLoyaltyPoints> GetActiveCustomerLoyaltyPoints(string customerId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAsyncRequest<CustomerLoyaltyPoints>($"/api/CustomerLoyaltyPoints/CustomerId/{customerId}");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<int> CreateCustomerLoyaltyPoints(CustomerLoyaltyPoints customerLoyaltyPoints)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/CustomerLoyaltyPoints", customerLoyaltyPoints);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<CustomerLoyaltyPointsParam> GetCustomerLoyaltyPointsHistory(string customerId, string loyaltyId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAsyncRequest<CustomerLoyaltyPointsParam>($"/api/CustomerLoyaltyPoints/CustomerId/{customerId}/LoyaltyId/{loyaltyId}");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<int> TransferLoyaltyPoints(string customerId, string senderName, string recipientEmail, LoyaltyPointsHistory history)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/CustomerLoyaltyPoints/Transfer/{customerId}/Sender/{senderName}/Email/{recipientEmail}", history);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<OrderParameter> GetOrderDetails(string customerId, string OrderId)
        {
            try
            {
                return await App.jsonWebApiAgent.SendGetAsyncRequest<OrderParameter>($"/api/Order/CustomerId/{customerId}/OrderId/{OrderId}");
            }
            catch (Exception e)
            {
                return new OrderParameter();
            }
        }

        public static async Task<int> InsertReferral(Referrals referrals)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/Referral/Create", referrals);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<List<Referrals>> GetReferrals(string customerId)
        {
            try
            {
                return await App.jsonWebApiAgent.SendGetAllAsyncRequest<Referrals>($"/api/Referral/Get/{customerId}");
            }
            catch (Exception e)
            {
                return new List<Referrals>();
            }
        }
        public static async Task<ReferralRewardsParam> GetReferralReward(string customerId)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAsyncRequest<ReferralRewardsParam>($"/api/ReferralReward/Get/{customerId}");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static async Task<int> CheckIfFirstPurchaseV2(string customerId)
        {
            try
            {
                return await App.jsonWebApiAgent.SendGetAsyncRequest<int>($"/api/Order/CheckIfFirstPurchaseV2/{customerId}");
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public static async Task<SMTPConfig> GetConfig(string type)
        {
            try
            {
                var res = await App.jsonWebApiAgent.SendGetAsyncRequest<SMTPConfig>($"/api/SMTP/{type}");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<int> SendSms(SMTPConfigParams param)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/SMTP/SMS", param);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static async Task<int> SendEmail(SMTPConfigParams param)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPosAsyncRequest<int>($"/api/SMTP/Email", param);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static async Task<Customer> CheckUserInfoByEmail(string email)
        {
            try
            {
                return await App.jsonWebApiAgent.SendGetAsyncRequest<Customer>($"/api/Customer/Email/{email}");
            }
            catch (Exception e)
            {
                return new Customer();
            }
        }

        public async Task<(int distanceInMeters, TimeSpan duration)> GetDistanceAndDurationAsync(string origin, string destination)
        {
            var apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destination}&key={App.GOOGLE_MAP_API_KEY}";

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);

                var distanceInMeters = (int)json["rows"][0]["elements"][0]["distance"]["value"];
                var durationInSeconds = (int)json["rows"][0]["elements"][0]["duration"]["value"];

                return (distanceInMeters, TimeSpan.FromSeconds(durationInSeconds));
            }
        }

        public static async Task<GoogleDirectionsResponse> GetMapsDirection(Position startLocation, Position endLocation, List<Position> waypointLocations)
        {
            try
            {
                //List<string> positionStrings = new List<string>();
                //foreach (var position in waypointLocations)
                //{
                //    positionStrings.Add($"{position.Latitude},{position.Longitude}");
                //}
                //var waypoints = string.Join("|", positionStrings);
                //var googleDirection = await App.jsonWebApiAgent.SendGetAsyncRequestMaps<GoogleDirectionsResponse>($"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={startLocation.Latitude},{startLocation.Longitude}&destination={endLocation.Latitude},{endLocation.Longitude}&waypoints={waypoints}&key={App.GOOGLE_MAP_API_KEY}");
                var googleDirection = await App.jsonWebApiAgent.SendGetAsyncRequestMaps<GoogleDirectionsResponse>($"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={startLocation.Latitude},{startLocation.Longitude}&destination={endLocation.Latitude},{endLocation.Longitude}&key={App.GOOGLE_MAP_API_KEY}");
                return googleDirection;
            }
            catch (Exception e)
            {
                return new GoogleDirectionsResponse();
            }
        }

        public static async Task<DriverDetails> GetVehicleByDriverId(string driverId)
        {
            try
            {
                return await App.jsonWebApiAgent.SendGetAsyncRequest<DriverDetails>($"/api/Driver/DriverId/{driverId}");
            }
            catch (Exception e)
            {
                return new DriverDetails();
            }
        }
        public static async Task<int> ArchiveOrder(string orderId)
        {
            try
            {
                return await App.jsonWebApiAgent.SendPutAsyncRequest<int>($"/api/Order/ArchiveOrder/Type/Customer/OrderId/{orderId}",null);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
