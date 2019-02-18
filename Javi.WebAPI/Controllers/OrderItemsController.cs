using AutoMapper;
using Javi.BusinessLogic;
using Javi.BusinessLogic.Entities;
using Javi.BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Javi.WebAPI.Controllers
{
    [Route("api/orders/{orderid}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IRepository repository;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public OrderItemsController(IRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            try
            {
                var order = repository.GetOrderById(orderId);
                if (order != null)
                {
                    return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Failed getting the list of Order Items{ex}");
                return BadRequest("Failed Error getting Order Items");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            try
            {
                var order = repository.GetOrderById(orderId);
                if (order != null)
                {
                    var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                    if (item != null) return Ok(mapper.Map<OrderItem, OrderItemViewModel>(item));
                    else return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Failed getting an Order Items by Id{ex}");
                return BadRequest("Failed Error getting an Order Items by Id");
            }

        }

    }
}
