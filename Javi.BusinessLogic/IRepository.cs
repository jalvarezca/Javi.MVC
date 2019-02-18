using Javi.BusinessLogic.Entities;
using System;
using System.Collections.Generic;

namespace Javi.BusinessLogic
{
    public interface IRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        void AddOrder(Order newOrder);

        Order GetOrderById(int id);
        void AddEntity(object model);
    }
}
