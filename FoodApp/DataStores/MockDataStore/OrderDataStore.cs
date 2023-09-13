using System;
using System.Collections.Generic;
using FoodApp.Models;

namespace FoodApp.DataStores.MockDataStore
{
    /// <summary>
    /// Mock data store with fake entities to test.
    /// </summary>
    //public class OrderDataStore : BaseDataStore<Order>, IOrderDataStore
    //{
    //    protected override IList<Order> items { get; }

    //    public OrderDataStore()
    //    {
    //        var myOrders = Globals.Orders;
    //        if (myOrders != null)
    //        {
    //            items = myOrders;
    //        }
    //        else
    //        {
    //            items = new List<Order>();
    //        }
    //        //items = new List<Order>
    //        //{
    //        //    new Order { Id = "or001", CustomerId = "cu001", DateGmt = new DateTime(2021, 1, 7),
    //        //                Status = OrderStatus.Delivered,
    //        //                BillingAddress = new Address { Id = "a001", CustomerId = "cu001", Title = "My Work", Address1 = "351 Morbi 16 quis eleifend turpis",
    //        //                                        City = "Duis", PostCode = "67452", Country = "Quisque" },
    //        //                ShippingAddress = new Address { Id = "a002", CustomerId = "cu001", Title = "My House", Address1 = "8763 Duis in 227 nisi tristique",
    //        //                                       City = "Duis", PostCode = "35535", Country = "Quisque" }},

    //        //    new Order { Id = "or002", CustomerId = "cu001", DateGmt = new DateTime(2021, 2, 13),
    //        //                Status = OrderStatus.Cancelled ,
    //        //                BillingAddress = new Address { Id = "a002", CustomerId = "cu001", Title = "My House", Address1 = "8763 Duis in 227 nisi tristique",
    //        //                                        City = "Duis", PostCode = "35535", Country = "Quisque" },
    //        //                ShippingAddress = new Address { Id = "a002", CustomerId = "cu001", Title = "My House", Address1 = "8763 Duis in 227 nisi tristique",
    //        //                                       City = "Duis", PostCode = "35535", Country = "Quisque" }},

    //        //    new Order { Id = "or003", CustomerId = "cu001", DateGmt = new DateTime(2021, 2, 24),
    //        //                Status = OrderStatus.OnTheWay,
    //        //                BillingAddress = new Address { Id = "a001", CustomerId = "cu001", Title = "My Work", Address1 = "351 Morbi 16 quis eleifend turpis",
    //        //                                        City = "Duis", PostCode = "67452", Country = "Quisque" },
    //        //                ShippingAddress = new Address { Id = "a001", CustomerId = "cu001", Title = "My Work", Address1 = "351 Morbi 16 quis eleifend turpis",
    //        //                                        City = "Duis", PostCode = "67452", Country = "Quisque" }},

    //        //    new Order { Id = "or004", CustomerId = "cu001", DateGmt = new DateTime(2021, 3, 9),
    //        //                Status = OrderStatus.Processing,
    //        //                BillingAddress = new Address { Id = "a002", CustomerId = "cu001", Title = "My House", Address1 = "8763 Duis in 227 nisi tristique",
    //        //                                        City = "Duis", PostCode = "35535", Country = "Quisque" },
    //        //                ShippingAddress = new Address { Id = "a001", CustomerId = "cu001", Title = "My Work", Address1 = "351 Morbi 16 quis eleifend turpis",
    //        //                                        City = "Duis", PostCode = "67452", Country = "Quisque" }}

    //        //};
    //    }
    //}
}
