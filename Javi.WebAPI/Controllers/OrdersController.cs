using AutoMapper;
using Javi.BusinessLogic;
using Javi.BusinessLogic.Entities;
using Javi.BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Javi.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IRepository repository;
        private readonly ILogger<OrdersController> logger;
        private readonly IMapper mapper;

        public OrdersController(IRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                this.logger.LogInformation($"Trying to get the list of all orders with items = {includeItems}");
                var results = repository.GetAllOrders(includeItems);
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error ocurred getting the list of Orders {ex}");
                return BadRequest("Error getting Orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = this.repository.GetOrderById(id);
                if (order != null) return Ok(mapper.Map<Order, OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error ocurred getting an Irder by Id {ex}");
                return BadRequest("Error getting  an order by Id");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = mapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate < DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                    this.repository.AddOrder(newOrder);
                    if (this.repository.SaveAll())
                    {
                        return Created($"/api/Orders/{newOrder.Id}", mapper.Map<Order, OrderViewModel>(newOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error ocurred Creating a new Order {ex}");
            }
            return BadRequest("Failed to create a new Order");
        }
    }
}
