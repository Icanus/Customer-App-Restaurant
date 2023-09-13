using FoodApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Services
{
    //public interface IService
    //{
    //    /// <summary>
    //    /// Get the product by id
    //    /// </summary>
    //    /// <param name="id">product id</param>
    //    /// <returns>Product object or null</returns>
    //    Task<Items> GetItemAsync(string id);
    //    Task<int> AddAllItemsAsync(List<Items> category);
    //    /// <summary>
    //    /// Get products by parameters
    //    /// </summary>
    //    /// <param name="categoriId">Category id. Default is null.</param>
    //    /// <param name="key">Keyword for product name. Default is null.</param>
    //    /// <param name="onlyFavorite">Get only favorited items. Default is false.</param>
    //    /// <param name="onlyFeatured">Get only featured items. Default is false.</param>
    //    /// <param name="onlyPopular">Get only popular items. Default is false.</param>
    //    /// <returns>List of product filtered by parameters</returns>
    //    Task<IEnumerable<Items>> GetItemsAsync(string categoriId = null, string key = null,
    //                                          bool onlyFavorite = false, bool onlyFeatured = false,
    //                                          bool onlyPopular = false, bool onlySale = false);

    //    /// <summary>
    //    /// Get categories by name
    //    /// </summary>
    //    /// <param name="name">Keyword for category name</param>
    //    /// <returns>List of categories filtered by name</returns>
    //    Task<IEnumerable<Category>> GetCategoriesAsync(string name);
    //    Task<int> AddOrUpdateCategoryAsync(Category category);
    //    Task<int> AddAllCategoryAsync(List<Category> category);

    //    /// <summary>
    //    /// Get list of all banners
    //    /// </summary>
    //    /// <returns>List of banner objects</returns>
    //    Task<IEnumerable<Banner>> GetBannersAsync();
    //    Task<int> InsertBannerAsync(Banner banner);

    //    /// <summary>
    //    /// Get customer by id
    //    /// </summary>
    //    /// <param name="id">Customer Id</param>
    //    /// <returns>Customer object or null</returns>
    //    Task<Customer> GetCustomerAsync(string id);
    //    Task<Customer> GetCustomerAsync(string email, string password);

    //    /// <summary>
    //    /// Update the customer
    //    /// </summary>
    //    /// <param name="customer">Customer object</param>
    //    /// <returns>True, if successful</returns>
    //    Task<bool> UpdateCustomerAsync(Customer customer);

    //    // Address
    //    /// <summary>
    //    /// Get address by id
    //    /// </summary>
    //    /// <param name="id">Address id</param>
    //    /// <returns>Address object or null</returns>
    //    Task<Address> GetAddressAsync(string id);

    //    /// <summary>
    //    /// Get list of addresses by customer id
    //    /// </summary>
    //    /// <param name="customerId">Customer id</param>
    //    /// <returns>List of address of customer</returns>
    //    Task<IEnumerable<Address>> GetAddressesAsync(string customerId);
    //    Task<int> AddAllAddressAsync(List<Address> address);

    //    /// <summary>
    //    /// Delete the address
    //    /// </summary>
    //    /// <param name="id">Address Id</param>
    //    /// <returns>True, if successful</returns>
    //    Task<bool> DeleteAddressAsync(string id);

    //    /// <summary>
    //    /// Add the address
    //    /// </summary>
    //    /// <param name="address">Address object</param>
    //    /// <returns>Added address</returns>
    //    Task<Address> AddAddressAsync(Address address);

    //    /// <summary>
    //    /// Update the address
    //    /// </summary>
    //    /// <param name="address">Address object</param>
    //    /// <returns>True, if successful</returns>
    //    Task<bool> UpdateAddressAsync(Address address);

    //    /// <summary>
    //    /// Get cart item by id
    //    /// </summary>
    //    /// <param name="id">CartItem id</param>
    //    /// <returns>CartItem object or null</returns>
    //    //Task<BasketItem> GetCartItemAsync(string id);

    //    /// <summary>
    //    /// Add the cart item
    //    /// </summary>
    //    /// <param name="cartItem">CartItem object</param>
    //    /// <returns>Added cart item</returns>
    //    //Task<BasketItem> AddCartItemAsync(BasketItem cartItem);

    //    /// <summary>
    //    /// Update the CartItem
    //    /// </summary>
    //    /// <param name="cartItem">CartItem object</param>
    //    /// <returns>True, if successful</returns>
    //    //Task<bool> UpdateCartItemAsync(BasketItem cartItem);

    //    /// <summary>
    //    /// Get all items in the cart
    //    /// </summary>
    //    /// <returns>All CartItems in the cart</returns>
    //    //Task<IEnumerable<BasketItem>> GetCartItemsAsync();

    //    /// <summary>
    //    /// Delete cart by id
    //    /// </summary>
    //    /// <param name="id">CartItem id</param>
    //    /// <returns>True, if successful</returns>
    //    //Task<bool> DeleteCartItemAsync(string id);

    //    /// <summary>
    //    /// Delete all items in the cart
    //    /// </summary>
    //    /// <returns>True, if successful</returns>
    //    //Task<bool> DeleteAllCartItemsAsync();

    //    /// <summary>
    //    /// Get order by id
    //    /// </summary>
    //    /// <param name="id">Order id</param>
    //    /// <returns>Order object or null</returns>
    //    Task<Order> GetOrderAsync(string id);
    //    Task<Order> GetOrderByOngoingStatus(string id);

    //    /// <summary>
    //    /// Get orders by customer id
    //    /// </summary>
    //    /// <param name="customerId">Customer id</param>
    //    /// <returns>List of orders of customer</returns>
    //    Task<IEnumerable<Order>> GetOrdersAsync(string customerId);

    //    /// <summary>
    //    /// Add the order object
    //    /// </summary>
    //    /// <param name="order">Order object</param>
    //    /// <returns>Added order object</returns>
    //    Task<Order> AddOrderAsync(Order order);
    //    Task<bool> UpdateOrderAsync(Order order);

    //    /// <summary>
    //    /// Get items in the order
    //    /// </summary>
    //    /// <param name="orderId">Order id</param>
    //    /// <returns>Items in the order</returns>
    //    Task<IEnumerable<OrderItem>> GetOrderItemsAsync(string orederId);

    //    /// <summary>
    //    /// Add order item
    //    /// </summary>
    //    /// <param name="orderItem">OrderItem object</param>
    //    /// <returns>Added order item</returns>
    //    Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);

    //    /// <summary>
    //    /// Add the favorite
    //    /// </summary>
    //    /// <param name="favorite">Favorite object</param>
    //    /// <returns>Added favorite object</returns>
    //    Task<Favorite> AddFavoriteAsync(Favorite favorite);
    //    Task<int> DeleteAllFavorites();

    //    /// <summary>
    //    /// Delete the Favorite
    //    /// </summary>
    //    /// <param name="id">Favorite id</param>
    //    /// <returns>True, if successful</returns>
    //    Task<bool> DeleteFavoriteAsync(string id);

    //    /// <summary>
    //    /// Determine the product is favorite 
    //    /// </summary>
    //    /// <param name="customerId">Customer id</param>
    //    /// <param name="productId">Product id</param>
    //    /// <returns>True, if product has been addes to favorites for customer</returns>
    //    Task<bool> IsFavoriteAsync(string customerId, string productId);

    //    /// <summary>
    //    /// Get the Favorite object by parameters
    //    /// </summary>
    //    /// <param name="customerId">Customer id</param>
    //    /// <param name="productId">Product id</param>
    //    /// <returns>Favorite object or null</returns>
    //    Task<Favorite> GetFavoriteAsync(string customerId, string productId);
    //    Task<int> AddAllFavoriteAsync(List<Favorite> favorite);

    //}
}
