using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceEngine.Models;

namespace PriceEngine.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;

            if (_context.Products.Any()) return;

            ProductsData.Init(context);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IQueryable<Product>> GetProducts([FromQuery] ProductPaging request)
        {
            var result = _context.Products as IQueryable<Product>;
            Response.Headers["x-total-count"] = result.Count().ToString();

            return Ok(result
              .OrderBy(p => p.ProductNumber)
              .Skip(request.StartAt)
              .Take(request.MaxResults));
        }

        [HttpGet]
        [Route("ListPrices/{productNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IQueryable<FullPrice>> ListPrices([FromRoute] string productNumber, [FromQuery] ProductPaging request)
        {
            try
            {
                var product = _context.Products
                  .FirstOrDefault(p => p.ProductNumber.Equals(productNumber,
                            StringComparison.InvariantCultureIgnoreCase));

                if (product == null) return NotFound();

                var response = new List<FullPrice>();
                for (int i = 1; i <= 50; i++)
                {
                    var totalCost = Calculator.Calc(product, i);
                    response.Add(new FullPrice() { ProductName = product.ProductName, ProductNumber = product.ProductNumber, RequestedUnits = i, TotalCost = totalCost });
                }

                return Ok(response
                  .OrderBy(p => p.RequestedUnits)
                  .Skip(request.StartAt)
                  .Take(request.MaxResults));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "ListPrices encountered an error");
                return ValidationProblem(e.Message);
            }
        }

        [HttpGet]
        [Route("GetPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<FullPrice> GetProductByProductNumber([FromQuery] PriceRequest request)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductNumber.Equals(request.ProductNumber, StringComparison.InvariantCultureIgnoreCase));

                if (product == null) return NotFound();

                var totalCost = Calculator.Calc(product, request.Units);

                return Ok(new FullPrice() { ProductName = product.ProductName, ProductNumber = product.ProductNumber, RequestedUnits = request.Units, TotalCost = totalCost });
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "GetPrice encountered an error.");
                return ValidationProblem(e.Message);
            }
        }
    }
}
