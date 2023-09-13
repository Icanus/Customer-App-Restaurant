using FoodApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Data
{
    public interface IRestaurantDatabaseRepository
    {
        Task<List<Banner>> GetBannerAsync();
        Task<int> AddOrUpdateBannerAsync(Banner banner);
        Task<List<Category>> GetCategoryAsync(string name);
        Task<int> AddOrUpdateCategoryAsync(Category category);
        Task<int> AddAllCategoryAsync(List<Category> category);
        Task<Items> GetItemAsync(string id);
        Task<IEnumerable<Items>> GetItemsParameterAsync(string categoriId = null, string key = null,
                                             bool onlyFavorite = false, bool onlyFeatured = false,
                                             bool onlyPopular = false, bool onlySale = false);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string id);
        Task<Customer> GetCustomerAsync(string email, string password);
        Task<Address> GetAddressAsync(string id);
        Task<IEnumerable<Address>> GetAddressesAsync(string customerId);
        Task<IEnumerable<Address>> GetAddressesAsyncExculdeExistingAddress(string customerId, string AddressTitle);
        Task<bool> DeleteAddressAsync(string id);
        Task<Address> AddAddressAsync(Address address);
        Task<int> AddAllAddressAsync(List<Address> address);
        Task<Favorite> AddFavoriteAsync(Favorite favorite);
        Task<bool> DeleteFavoriteAsync(string id);
        Task<bool> IsFavoriteAsync(string customerId, string productId);
        Task<Favorite> GetFavoriteAsync(string customerId, string productId);
        Task<int> AddAllFavoriteAsync(List<Favorite> favorite);
        Task<int> DeleteAllFavorites();
        Task<int> AddAllItemsAsync(List<Items> items);
        Task<CustomerLoyaltyPoints> GetActiveLoyaltyPoints(string customerId);
        Task<int> AddCustomerLoyaltyPoints(CustomerLoyaltyPoints customerLoyaltyPoints);
        Task<int> AddAllHistory(List<LoyaltyPointsHistory> items);
        Task<List<LoyaltyPointsHistory>> GetLoyaltyPointsHistories(string name);
        Task<int> AddAllOrder(List<Order> items);
        Task<int> AddAllOrderItem(List<OrderItem> items);
        Task<int> AddAllFeedback(List<Feedback> items);
        Task<List<OrderParameter>> GetOrders(string customerId);
        Task<OrderParameter> GetOrderByOrderId(string orderId);
        Task<int> AddAllReferral(List<Referrals> referrals);
        Task<List<Referrals>> GetReferrals(string CustomerId);
        Task<int> AddReferralRewards(ReferralRewards referralRewards);
        Task<int> AddReferralRewardHistoy(List<ReferralRewardsHistory> items);
        Task<ReferralRewardsParam> GetReferralRewards(string customerId);
        Task<BasketItem> GetCartItemAsync(int id);
        Task<BasketItem> AddCartItemAsync(BasketItem cartItem);
        Task<bool> UpdateCartItemAsync(BasketItem cartItem);
        Task<IEnumerable<BasketItem>> GetCartItemsAsync();
        Task<bool> DeleteCartItemAsync(int id);
        Task<bool> DeleteAllCartItemsAsync(string customerId);
        Task<int> ClearAllTables();

    }
}
