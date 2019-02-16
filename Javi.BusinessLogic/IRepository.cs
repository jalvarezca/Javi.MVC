using Javi.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javi.BusinessLogic
{
    public interface IRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string user, bool includeItems);
        void AddOrder(Order newOrder);

        Order GetOrderById(string user, int id);
        void AddEntity(object model);
    }
}
