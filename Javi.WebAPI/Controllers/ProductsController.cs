using Javi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Javi.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IRepository repository;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IRepository repository, ILogger<ProductsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(this.repository.GetAllProducts());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get products : {ex}");
                return BadRequest("Failed to get products");
            }
        }
    }
}
