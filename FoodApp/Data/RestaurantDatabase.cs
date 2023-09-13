using FoodApp.DataStores;
using FoodApp.Enums;
using FoodApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace FoodApp.Data
{
    public class RestaurantDatabase : IRestaurantDatabaseRepository
    {
        static SQLiteAsyncConnection Database;
        public RestaurantDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Restaurant.db3");
            Database = new SQLiteAsyncConnection(dbPath);
            ConfigureDb();
        }
        bool isProduction = false;
        void ConfigureDb()
        {
            Task.Run(async () =>
            {
                var tableTypes = new List<Type>
                {
                    typeof(Banner),
                    typeof(Category),
                    typeof(Customer),
                    typeof(Address),
                    typeof(Favorite),
                    typeof(Items),
                    typeof(Order),
                    typeof(OrderItem),
                    typeof(Feedback),
                    typeof(CustomerLoyaltyPoints),
                    typeof(LoyaltyPointsHistory),
                    typeof(ReferralRewards),
                    typeof(ReferralRewardsHistory),
                    typeof(Referrals),
                    typeof(BasketItem),
                    // Add other table types here
                };
                foreach (var tableType in tableTypes)
                {
                    if (isProduction)
                    {
                        var tableName = tableType.Name;
                        var tableExists = await Database.ExecuteScalarAsync<int>(
                            $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'"
                        );
                        if (tableExists == 0)
                        {
                            await Database.CreateTableAsync(tableType);
                        }
                    }
                    else
                    {
                        await Database.CreateTableAsync(tableType);
                    }
                }
            });
        }

        public async Task IsTableExists(string tableName)
        {
            try
            {
                await Task.Delay(0);
                var tableInfo = Database.GetConnection().GetTableInfo(tableName);
                if (tableInfo.Count > 0)
                {
                    Console.WriteLine("exists");
                }
                else
                {
                    Console.WriteLine("not exists");
                }
            }
            catch
            {
                Console.WriteLine("not exists");
            }
        }

        public async Task<int> AddOrUpdateBannerAsync(Banner banner)
        {
            if(banner.Id != null)
            {
                return await Database.UpdateAsync(banner);
            }
            else
            {
                return await Database.InsertAsync(banner);
            }
        }

        public async Task<List<Banner>> GetBannerAsync()
        {
            return await Database.Table<Banner>().ToListAsync();
        }

        public async Task<List<Category>> GetCategoryAsync(string name)
        {
            return await Database.Table<Category>().Where(i => name == null || i.Name.Contains(name)).ToListAsync();
        }

        public async Task<int> AddOrUpdateCategoryAsync(Category category)
        {
            if (category.Id != null)
            {
                return await Database.UpdateAsync(category);
            }
            else
            {
                return await Database.InsertAsync(category);
            }
        }
        public async Task<int> AddAllCategoryAsync(List<Category> category)
        {
            await Database.DeleteAllAsync<Category>();
            return await Database.InsertAllAsync(category);
        }

        public async Task<Items> GetItemAsync(string id)
        {
            return await Database.Table<Items>().Where(x => x.ItemId == id).FirstAsync();
        }

        public async Task<IEnumerable<Items>> GetItemsParameterAsync(string categoryId = null, string key = null,
                                           bool onlyFavorite = false, bool onlyFeatured = false,
                                           bool onlyPopular = false, bool onlySale = false)
        {
            var result = await Database.Table<Items>().ToListAsync();

            var res = result.Where(p => (categoryId == null || p.CategoryId == Convert.ToInt32(categoryId)) &&
                        (key == null || p.Name.ToLower().Contains(key.ToLower())) &&
                        (onlyFeatured == false || p.IsFeatured == true) &&
                        (onlyPopular == false || p.IsPopular == true) &&
                        (onlySale == false || p.OnSale == true)).ToList().Select(i =>
                        {
                            i.IsFavorite = IsFavoriteAsync(Globals.LoggedCustomerId, i.ItemId).Result;
                            return i;
                        }).Where(p => onlyFavorite == false || p.IsFavorite == true);
            return res;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            var count = await Database.Table<Customer>().Where(x => x.CustomerId == customer.CustomerId).ToListAsync();
            if (count.Count() == 0)
            {
                var res = await Database.InsertAsync(customer);
                return res == 1 ? true : false;
            }
            var result = await Database.InsertOrReplaceAsync(customer);
            return result == 1 ? true : false;
        }

        public async Task<Customer> GetCustomerAsync(string customerId)
        {
            var res = await Database.Table<Customer>().Where(x=>x.CustomerId == customerId).ToListAsync();
            if(res.Count() == 0)
            {
                return new Customer();
            }
            return await Database.Table<Customer>().Where(x => x.CustomerId == customerId).FirstAsync();
        }

        public async Task<Customer> GetCustomerAsync(string email, string password)
        {
            var res = await Database.Table<Customer>().Where(x => x.Email == email && x.Password == password).ToListAsync();
            if (res.Count() == 0)
            {
                return new Customer();
            }
            return await Database.Table<Customer>().Where(x => x.Email == email && x.Password == password).FirstAsync();
        }

        public async Task<Address> GetAddressAsync(string id)
        {
            return await Database.Table<Address>().Where(x => x.AddressId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync(string customerId)
        {
            return await Database.Table<Address>().Where(i => i.CustomerId == customerId).ToListAsync();
        }
        public async Task<Address> GetAddressByTitle(string AddressTitle, string customerId)
        {
            var res = await Database.Table<Address>().Where(i => i.Title == AddressTitle && i.CustomerId == customerId).ToListAsync();
            return res.FirstOrDefault();
        }
        public async Task<bool> DeleteAddressAsync(string id)
        {
            var res = await Database.Table<Address>().DeleteAsync(x=> x.AddressId == id);
            return res == 1 ? true : false;
        }

        public async Task<Address> AddAddressAsync(Address address)
        {
            var count = await Database.Table<Address>().Where(x => x.CustomerId == address.CustomerId).ToListAsync();
            if (count.Count() == 0)
            {
                var res = await Database.InsertOrReplaceAsync(address);
            }
            else
            {
                var result = await Database.UpdateAsync(address);
            }

            return await Database.Table<Address>().Where(x => x.CustomerId == address.CustomerId).FirstOrDefaultAsync();
        }

        public async Task<int> AddAllAddressAsync(List<Address> address)
        {
            await Database.DeleteAllAsync<Address>();
            return await Database.InsertAllAsync(address);
        }

        public async Task<Favorite> AddFavoriteAsync(Favorite favorite)
        {
            await Database.InsertAsync(favorite);
            return new Favorite(); // nneds to be fix
        }

        public async Task<bool> DeleteFavoriteAsync(string id)
        {
            var res = await Database.Table<Favorite>().DeleteAsync(x => x.FavoriteId == id);
            return res == 1 ? true : false;
        }

        public Task<bool> IsFavoriteAsync(string customerId, string productId)
        {
            var res = Database.Table<Favorite>().Where(x => x.CustomerId == customerId).ToListAsync();
            
            return Task.FromResult(res.Result.Any(x => x.ItemId == productId));
        }

        public async Task<Favorite> GetFavoriteAsync(string customerId, string productId)
        {
            var res = await Database.Table<Favorite>().Where(x => x.CustomerId == customerId && x.ItemId == productId).ToListAsync();
            if (res.Count() == 0)
            {
                return null;
            }

            return await Database.Table<Favorite>().Where(x => x.CustomerId == customerId && x.ItemId == productId).FirstOrDefaultAsync();
        }

        public async Task<int> AddAllFavoriteAsync(List<Favorite> favorite)
        {
            await Database.DeleteAllAsync<Favorite>();
            return await Database.InsertAllAsync(favorite);
        }

        public async Task<int> AddAllItemsAsync(List<Items> items)
        {
            await Database.DeleteAllAsync<Items>();
            return await Database.InsertAllAsync(items);
        }

        public async Task<int> DeleteAllFavorites()
        {
            return await Database.DeleteAllAsync<Favorite>();
        }

        public async Task<CustomerLoyaltyPoints> GetActiveLoyaltyPoints(string customerId)
        {
            return await Database.Table<CustomerLoyaltyPoints>().Where(x => x.CustomerId == customerId).FirstOrDefaultAsync();
        }

        public async Task<int> AddCustomerLoyaltyPoints(CustomerLoyaltyPoints customerLoyaltyPoints)
        {

            await Database.DeleteAllAsync<CustomerLoyaltyPoints>();
            var result = await Database.InsertOrReplaceAsync(customerLoyaltyPoints);
            return result;
        }

        public async Task<int> AddAllHistory(List<LoyaltyPointsHistory> items)
        {
            await Database.DeleteAllAsync<LoyaltyPointsHistory>();
            return await Database.InsertAllAsync(items);
        }

        public async Task<List<LoyaltyPointsHistory>> GetLoyaltyPointsHistories(string loyaltyId)
        {
            return await Database.Table<LoyaltyPointsHistory>().Where(i => i.LoyaltyId == loyaltyId).OrderByDescending(x=>x.Id).ToListAsync();
        }

        public async Task<int> AddAllOrder(List<Order> items)
        {
            await Database.DeleteAllAsync<Order>();
            return await Database.InsertAllAsync(items);
        }

        public async Task<int> AddAllOrderItem(List<OrderItem> items)
        {
            await Database.DeleteAllAsync<OrderItem>();
            return await Database.InsertAllAsync(items);
        }

        public async Task<List<OrderParameter>> GetOrders(string customerId)
        {
            List<OrderParameter> orders = new List<OrderParameter>();
            var res = await Database.Table<Order>().Where(i => i.CustomerId == customerId).OrderByDescending(x => x.Id).ToListAsync();
            foreach(var item in res)
            {
                OrderParameter orderParameter = new OrderParameter
                {
                    Id=item.Id,
                    OrderId=item.OrderId,
                    CustomerId = item.OrderId,
                    DateGmt = item.DateGmt,
                    Address = item.Address,
                    AddressTitle = item.AddressTitle,
                    Shipping = item.Shipping,
                    Total = item.Total,
                    Discount = item.Discount,
                    Status = item.Status,
                    ModeOfPayment = item.ModeOfPayment,
                    IsOngoingOrder = item.IsOngoingOrder,
                    OrderStatus = null,
                    PlacedTime = item.PlacedTime,
                    ProcessingTime = item.ProcessingTime,
                    OnTheWayTime = item.OnTheWayTime,
                    ForPickUpTime = item.ForPickUpTime,
                    DeliveredTime = item.DeliveredTime,
                    CanceledTime = item.CanceledTime,
                    GrandTotal = item.GrandTotal,
                    Lat = item.Lat,
                    Lon = item.Lon,
                    DriverId = item.DriverId,
                    DriverLat = item.DriverLat,
                    DriverLon = item.DriverLon,
                    OrderItems = await Database.Table<OrderItem>().Where(i => i.OrderId == item.OrderId).OrderByDescending(x => x.Id).ToListAsync(),
                    FeedBack = await Database.Table<Feedback>().Where(i => i.OrderId == item.OrderId).OrderByDescending(x => x.Id).FirstOrDefaultAsync(),
                    IsChangeAddress = item.IsChangeAddress,
                    ChangeAddress = item.ChangeAddress,
                    ChangeAddressTitle = item.ChangeAddressTitle,
                    ChangeAddressLat = item.ChangeAddressLat,
                    ChangeAddressLon = item.ChangeAddressLon,
                    AdditionalFee = item.AdditionalFee,
                    IsArchive = item.IsArchive
                };
                orders.Add(orderParameter);
            }
            return orders;
        }

        public async Task<int> AddAllFeedback(List<Feedback> items)
        {
            await Database.DeleteAllAsync<Feedback>();
            return await Database.InsertAllAsync(items);
        }

        public async Task<OrderParameter> GetOrderByOrderId(string orderId)
        {
            var item = await Database.Table<Order>().Where(i => i.OrderId == orderId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            OrderParameter orderParameter;
            if (item?.OrderId != null)
            {
                orderParameter = new OrderParameter
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    CustomerId = item.OrderId,
                    DateGmt = item.DateGmt,
                    Address = item.Address,
                    AddressTitle = item.AddressTitle,
                    Shipping = item.Shipping,
                    Discount = item.Discount,
                    Total = item.Total,
                    Status = item.Status,
                    ModeOfPayment = item.ModeOfPayment,
                    IsOngoingOrder = item.IsOngoingOrder,
                    OrderStatus = null,
                    PlacedTime = item.PlacedTime,
                    ProcessingTime = item.ProcessingTime,
                    OnTheWayTime = item.OnTheWayTime,
                    ForPickUpTime = item.ForPickUpTime,
                    DeliveredTime = item.DeliveredTime,
                    CanceledTime = item.CanceledTime,
                    GrandTotal = item.GrandTotal,
                    Lat = item.Lat,
                    Lon = item.Lon,
                    DriverId = item.DriverId,
                    DriverLat = item.DriverLat,
                    DriverLon = item.DriverLon,
                    OrderItems = await Database.Table<OrderItem>().Where(i => i.OrderId == item.OrderId).OrderByDescending(x => x.Id).ToListAsync(),
                    FeedBack = await Database.Table<Feedback>().Where(i => i.OrderId == item.OrderId).OrderByDescending(x => x.Id).FirstOrDefaultAsync(),
                    IsChangeAddress = item.IsChangeAddress,
                    ChangeAddress = item.ChangeAddress,
                    ChangeAddressTitle = item.ChangeAddressTitle,
                    ChangeAddressLat = item.ChangeAddressLat,
                    ChangeAddressLon = item.ChangeAddressLon,
                    AdditionalFee = item.AdditionalFee,
                    IsArchive = item.IsArchive
                };
                return orderParameter;
            }
            return orderParameter = new OrderParameter();
        }

        public async Task<int> AddReferralRewards(ReferralRewards referralRewards)
        {
            await Database.DeleteAllAsync<ReferralRewards>();
            var result = await Database.InsertOrReplaceAsync(referralRewards);
            return result;
        }

        public async Task<int> AddReferralRewardHistoy(List<ReferralRewardsHistory> items)
        {
            await Database.DeleteAllAsync<ReferralRewardsHistory>();
            return await Database.InsertAllAsync(items);
        }

        public async Task<ReferralRewardsParam> GetReferralRewards(string customerId)
        {
            var item = await Database.Table<ReferralRewards>().Where(i => i.ReferralId == customerId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            ReferralRewardsParam orderParameter = new ReferralRewardsParam
            {
                Id = item.Id,
                ReferralId = item.ReferralId,
                IsTerminated = item.IsTerminated,
                Balance = item.Balance,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                ReferralRewardsHistory = await Database.Table<ReferralRewardsHistory>().Where(i => i.ReferralId == item.ReferralId).OrderByDescending(x => x.Id).ToListAsync()
            };
            return orderParameter;
        }

        public async Task<int> AddAllReferral(List<Referrals> referrals)
        {
            await Database.DeleteAllAsync<Referrals>();
            var result = await Database.InsertAllAsync(referrals);
            return result;
        }

        public async Task<List<Referrals>> GetReferrals(string CustomerId)
        {
           return await Database.Table<Referrals>().Where(i => i.ReferrerId == CustomerId).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<BasketItem> GetCartItemAsync(int id)
        {
            return await Database.Table<BasketItem>().Where(x => x.Id == id).FirstAsync();
        }

        public async Task<BasketItem> AddCartItemAsync(BasketItem cartItem)
        {
            var oldItem = await Database.Table<BasketItem>().Where(i => i.ProductName == cartItem.ProductName &&
                                                        i.IngredientString == cartItem.IngredientString &&
                                                        i.ChoiceString == cartItem.ChoiceString &&
                                                        i.UnitPrice == cartItem.UnitPrice).FirstOrDefaultAsync();

            if (oldItem == null)
                await Database.InsertOrReplaceAsync(cartItem);
            else
            {
                oldItem.Quantity += cartItem.Quantity;
                await Database.UpdateAsync(oldItem);
                return oldItem;
            }

            return await Database.Table<BasketItem>().Where(x=>x.ProductId == cartItem.ProductId).FirstAsync();
        }

        public async Task<bool> UpdateCartItemAsync(BasketItem cartItem)
        {
            var res = await Database.UpdateAsync(cartItem);
            return res != 0 ? true : false;
        }

        public async Task<IEnumerable<BasketItem>> GetCartItemsAsync()
        {
            //return await Database.Table<BasketItem>().Where(x=>x.CustomerId == Globals.LoggedCustomerId).ToListAsync();
            return await Database.Table<BasketItem>().ToListAsync();
        }

        public async Task<bool> DeleteCartItemAsync(int id)
        {
            var res = await Database.DeleteAsync<BasketItem>(id);
            return res != 0 ? true : false;
        }

        public async Task<bool> DeleteAllCartItemsAsync(string customerId)
        {
            //var res = await Database.Table<BasketItem>().Where(x=>x.CustomerId == customerId).DeleteAsync(); ;
            var res = await Database.DeleteAllAsync<BasketItem>();
            return res != 0 ? true : false;
        }

        public async Task<IEnumerable<Address>> GetAddressesAsyncExculdeExistingAddress(string customerId, string addressTitle)
        {
            return await Database.Table<Address>().Where(i => i.CustomerId == customerId && i.Title != addressTitle).ToListAsync();
        }

        public async Task<int> ClearAllTables()
        {
            await Database.DeleteAllAsync<Banner>();
            await Database.DeleteAllAsync<Category>();
            //Database.CreateTableAsync<Item>().Wait();
            await Database.DeleteAllAsync<Customer>();
            await Database.DeleteAllAsync<Address>();
            await Database.DeleteAllAsync<Favorite>();
            await Database.DeleteAllAsync<Items>();
            await Database.DeleteAllAsync<Order>();
            await Database.DeleteAllAsync<OrderItem>();
            await Database.DeleteAllAsync<Feedback>();
            await Database.DeleteAllAsync<CustomerLoyaltyPoints>();
            await Database.DeleteAllAsync<LoyaltyPointsHistory>();
            await Database.DeleteAllAsync<ReferralRewards>();
            await Database.DeleteAllAsync<ReferralRewardsHistory>();
            await Database.DeleteAllAsync<Referrals>();
            await Database.DeleteAllAsync<BasketItem>();
            return 1;
        }
    }
}
