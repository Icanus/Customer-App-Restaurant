using FoodApp.DataStores;
using FoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.Services
{
    //public class MockService : IService
    //{
        //IBannerDataStore dataBanner => DependencyService.Get<IBannerDataStore>();
        //ICategoryDataStore dataCategory => DependencyService.Get<ICategoryDataStore>();
        //IItemDataStore dataItem => DependencyService.Get<IItemDataStore>();
        //ICustomerDataStore dataCustomer = DependencyService.Get<ICustomerDataStore>();
        //IAddressDataStore dataAddress => DependencyService.Get<IAddressDataStore>();
        //ICartItemDataStore dataCartItem => DependencyService.Get<ICartItemDataStore>();
        //IOrderDataStore dataOrder => DependencyService.Get<IOrderDataStore>();
        //IOrderItemDataStore dataOrderItem => DependencyService.Get<IOrderItemDataStore>();
        //IFavoriteDataStore dataFavorite => DependencyService.Get<IFavoriteDataStore>();

        // Methods for Category entity

    //    public async Task<IEnumerable<Category>> GetCategoriesAsync(string name)
    //    {
    //        return await App.RestaurantDatabase.GetCategoryAsync(name);
    //        //if (Globals.isProduction)
    //        //{
               
    //        //}
    //        //else
    //        //{

    //        //    var result = await dataCategory.GetByAsync(i => name == null || i.Name.Contains(name));

    //        //    return await Task.FromResult(result);
    //        //}
    //    }
    //    public async Task<int> AddOrUpdateCategoryAsync(Category category)
    //    {
    //        return await App.RestaurantDatabase.AddOrUpdateCategoryAsync(category);
    //    }

    //    public async Task<int> AddAllCategoryAsync(List<Category> category)
    //    {
    //        return await App.RestaurantDatabase.AddAllCategoryAsync(category);
    //    }

    //    // Methods for Banner entity

    //    public async Task<IEnumerable<Banner>> GetBannersAsync()
    //    {
    //        if (Globals.isProduction)
    //        {
    //            return await App.RestaurantDatabase.GetBannerAsync();
    //        }
    //        else
    //        {
    //            return await dataBanner.GetAllAsync();
    //        }
    //    }

    //    public async Task<int> InsertBannerAsync(Banner banner)
    //    {
    //        return await App.RestaurantDatabase.AddOrUpdateBannerAsync(banner);
    //    }

    //    // Methods for item entity

    //    public async Task<Items> GetItemAsync(string id)
    //    {
    //        if (Globals.isProduction)
    //        {
    //            //return await dataItem.GetAsync(id);
    //        }
    //        return await App.RestaurantDatabase.GetItemAsync(id);
    //    }


    //    public async Task<int> AddAllItemsAsync(List<Items> items)
    //    {
    //        return await App.RestaurantDatabase.AddAllItemsAsync(items);
    //    }

    //    public async Task<IEnumerable<Items>> GetItemsAsync(string categoryId = null, string key = null,
    //                                        bool onlyFavorite = false, bool onlyFeatured = false,
    //                                        bool onlyPopular = false, bool onlySale = false)
    //    {
    //        if (Globals.isProduction)
    //        {
    //            //var result = (await dataItem.GetByAsync(p => (categoryId == null || p.CategoryId == Convert.ToInt32(categoryId)) &&
    //            //                           (key == null || p.Name.ToLower().Contains(key.ToLower())) &&
    //            //                           (onlyFeatured == false || p.IsFeatured == true) &&
    //            //                           (onlyPopular == false || p.IsPopular == true) &&
    //            //                           (onlySale == false || p.OnSale == true))).ToList()
    //            //                           .Select(i =>
    //            //                           {
    //            //                               i.IsFavorite = IsFavoriteAsync(Globals.LoggedCustomerId, i.Id).Result;
    //            //                               return i;
    //            //                           }).Where(p => onlyFavorite == false || p.IsFavorite == true);
    //        }
    //        var res = await App.RestaurantDatabase.GetItemsParameterAsync(categoryId, key, onlyFavorite, onlyFeatured, onlyPopular, onlySale);
    //        return res;
    //    }

    //    // Methods for Customer entity

    //    public async Task<Customer> GetCustomerAsync(string id)
    //    {
    //        if (Globals.isProduction)
    //        {
    //            //return await dataCustomer.GetAsync(id);
    //        }
    //        return await App.RestaurantDatabase.GetCustomerAsync(id);
    //    }

    //    public async Task<Customer> GetCustomerAsync(string email, string password)
    //    {
    //        if (Globals.isProduction)
    //        {
    //            //var res = (await dataCustomer.GetByAsync(i => i.Email == email && i.Password == password)).OrderBy(i => i.Email).ToList();
    //            //return res.Count() == 0 || res == null ? null : res[0];
    //            return new Customer();
    //        }
    //        return await App.RestaurantDatabase.GetCustomerAsync(email, password);
    //    }

    //    public async Task<bool> UpdateCustomerAsync(Customer customer)
    //    {
    //        if (Globals.isProduction)
    //        {
    //            //return await dataCustomer.UpdateAsync(customer);
    //        }
    //        return await App.RestaurantDatabase.UpdateCustomerAsync(customer);
    //    }

    //    // Methods for Address entity

    //    public async Task<Address> GetAddressAsync(string id)
    //    {
    //        //return await dataAddress.GetAsync(id);
    //        return await App.RestaurantDatabase.GetAddressAsync(id);
    //    }

    //    public async Task<IEnumerable<Address>> GetAddressesAsync(string customerId)
    //    {
    //        //return (await dataAddress.GetByAsync(i => i.CustomerId == customerId))
    //        //.OrderBy(i => i.Title);

    //        //return new List<Address>();
    //        return await App.RestaurantDatabase.GetAddressesAsync(customerId);
    //    }
    //    public async Task<int> AddAllAddressAsync(List<Address> address)
    //    {
    //        return await App.RestaurantDatabase.AddAllAddressAsync(address);
    //    }

    //    public async Task<bool> DeleteAddressAsync(string id)
    //    {
    //        //return await dataAddress.DeleteAsync(id);
    //        //return false;
    //        return await App.RestaurantDatabase.DeleteAddressAsync(id);
    //    }

    //    public async Task<Address> AddAddressAsync(Address address)
    //    {
    //        //return await dataAddress.AddAsync(address);
    //        //return new Address();
    //        return await App.RestaurantDatabase.AddAddressAsync(address);
    //    }

    //    public async Task<bool> UpdateAddressAsync(Address address)
    //    {
    //        //return await dataAddress.UpdateAsync(address);
    //        return false;
    //    }

    //    // Methods for CartItem entity

    //    //public async Task<BasketItem> GetCartItemAsync(string id)
    //    //{
    //    //    return await dataCartItem.GetAsync(id);
    //    //}

    //    //public async Task<BasketItem> AddCartItemAsync(BasketItem cartItem)
    //    //{
    //    //    var oldItem = dataCartItem.GetByAsync(i => i.ProductName == cartItem.ProductName &&
    //    //                                                i.IngredientString == cartItem.IngredientString &&
    //    //                                                i.ChoiceString == cartItem.ChoiceString &&
    //    //                                                i.UnitPrice == cartItem.UnitPrice)
    //    //                    .Result.FirstOrDefault();

    //    //    if (oldItem == null)
    //    //        return await dataCartItem.AddAsync(cartItem);
    //    //    else
    //    //    {
    //    //        oldItem.Quantity += cartItem.Quantity;
    //    //        await dataCartItem.UpdateAsync(oldItem);
    //    //        return oldItem;
    //    //    }
    //    //}

    //    //public async Task<bool> UpdateCartItemAsync(BasketItem cartItem)
    //    //{
    //    //    return await dataCartItem.UpdateAsync(cartItem);
    //    //}

    //    //public async Task<IEnumerable<BasketItem>> GetCartItemsAsync()
    //    //{
    //    //    var result = (await dataCartItem.GetAllAsync());

    //    //    return await Task.FromResult(result);
    //    //}

    //    //public async Task<bool> DeleteCartItemAsync(string id)
    //    //{
    //    //    return await dataCartItem.DeleteAsync(id);
    //    //}

    //    //public async Task<bool> DeleteAllCartItemsAsync()
    //    //{
    //    //    return await dataCartItem.DeleteAllAsync();
    //    //}

    //    // Methods for Order entity

    //    public async Task<Order> GetOrderAsync(string id)
    //    {
    //        return new Order();
    //        //return await dataOrder.GetAsync(id);
    //    }
    //    public async Task<Order> GetOrderByOngoingStatus(string id)
    //    {
    //        try
    //        {
    //            //var res = (await dataOrder.GetByAsync(i => i.CustomerId == id && i.IsOngoingOrder == true)).ToList().Select(o => {
    //            //    o.Total = 0;
    //            //    dataOrderItem.GetByAsync(l =>
    //            //        l.OrderId == o.Id).Result.ToList().ForEach(li => o.Total += li.Total);
    //            //    return o;
    //            //}).First();
    //            //return res;
    //            return null;
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }

    //    public async Task<IEnumerable<Order>> GetOrdersAsync(string customerId)
    //    {
    //        //var result = (await dataOrder.GetByAsync(o => o.CustomerId == customerId)).ToList()
    //        //                .Select(o => {
    //        //                    o.Total = 0;
    //        //                    dataOrderItem.GetByAsync(l =>
    //        //                        l.OrderId == o.Id).Result.ToList().ForEach(li => o.Total += li.Total);
    //        //                    return o;
    //        //                });

    //        //return await Task.FromResult(result);
    //        return null;
    //    }

    //    public async Task<Order> AddOrderAsync(Order order)
    //    {
    //        return null;
    //        //return await dataOrder.AddAsync(order);
    //    }

    //    public async Task<bool> UpdateOrderAsync(Order order)
    //    {
    //        return false;
    //        //return await dataOrder.UpdateAsync(order);
    //    }
    //    // Methods for OrderItem entity

    //    public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(string orederId)
    //    {
    //        //var result = await dataOrderItem.GetByAsync(s => s.OrderId == orederId);
    //        var result = new List<OrderItem>();
    //        return await Task.FromResult(result);
    //    }

    //    public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
    //    {
    //        return new OrderItem();
    //        //return await dataOrderItem.AddAsync(orderItem);
    //    }

    //    // Methods for Favorite entity

    //    public async Task<Favorite> AddFavoriteAsync(Favorite favorite)
    //    {
    //        //return await dataFavorite.AddAsync(favorite);
    //        return await App.RestaurantDatabase.AddFavoriteAsync(favorite);
    //    }

    //    public async Task<bool> DeleteFavoriteAsync(string id)
    //    {
    //        return await App.RestaurantDatabase.DeleteFavoriteAsync(id);
    //        //return await dataFavorite.DeleteAsync(id);
    //    }

    //    public async Task<bool> IsFavoriteAsync(string customerId, string itemId)
    //    {
    //        return await App.RestaurantDatabase.IsFavoriteAsync(customerId, itemId);
    //        //return await dataFavorite.IsExistAsync(f => f.CustomerId == customerId && f.ItemId == itemId);
    //    }

    //    public async Task<Favorite> GetFavoriteAsync(string customerId, string itemId)
    //    {
    //        return await App.RestaurantDatabase.GetFavoriteAsync(customerId, itemId);
    //        //return (await dataFavorite.GetByAsync(f => f.CustomerId == customerId && f.ItemId == itemId))
    //        //          .FirstOrDefault();
    //    }
    //    public async Task<int> AddAllFavoriteAsync(List<Favorite> favorite)
    //    {
    //        return await App.RestaurantDatabase.AddAllFavoriteAsync(favorite);
    //    }

    //    public async Task<int> DeleteAllFavorites()
    //    {
    //        return await App.RestaurantDatabase.DeleteAllFavorites();
    //    }

    //}
}
