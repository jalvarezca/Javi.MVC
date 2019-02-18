using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Javi.BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Javi.BusinessLogic
{
    public class Repository: IRepository
    {
        private readonly Context ctx;
        private readonly ILogger<Repository> logger;

        public Repository(Context ctx, ILogger<Repository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }
        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                this.logger.LogInformation("Getting the list of All orders from the db context");
                if (includeItems)
                {
                    return this.ctx.Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();
                }
                else
                {
                    return this.ctx.Orders.ToList();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed Getting the list of All Orders from the db context {ex}");
                throw ex;
            }

        }

        public Order GetOrderById(int id)
        {
            try
            {
                this.logger.LogInformation("Getting an order by Id");
                return this.ctx.Orders
                    .Where(o => o.Id == id)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed Getting the list of All Orders from the db context {ex}");
                throw ex;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                this.logger.LogInformation("Getting the list of All products from the db context");

                return this.ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Failed Getting the list of All products from the db context {ex}");
                throw ex;
            }
        }


        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return this.ctx.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public bool SaveAll()
        {
            return this.ctx.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
            this.ctx.Add(model);
        }

        public void AddOrder(Order newOrder)
        {
            foreach (var item in newOrder.Items)
            {
                item.Product = this.ctx.Products.Find(item.Product.Id);
            }

            AddEntity(newOrder);
        }
    }
}
