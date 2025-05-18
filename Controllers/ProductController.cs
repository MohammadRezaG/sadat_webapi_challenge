using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sadas_test.Models.Domain;
using Sadas_test.Models.DTOs;
using Sadas_test.Services.Implementations;
using Sadas_test.Services.Interfaces;

namespace Sadas_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPriceController : ControllerBase
    {
        private readonly IPriceSourcesService _priceFetcher;
        private readonly IProductService _ProductService;

        private readonly IHttpContextAccessor _contextAccessor;

        public GetPriceController(IPriceSourcesService PriceSourcesService, IProductService ProductService) {
            _ProductService = ProductService;
            _priceFetcher = PriceSourcesService;
        }

        [HttpGet("{productName}")]
        public async Task<IActionResult> GetPrice(string ProductName)
        {
            var (Source, Price) = await _priceFetcher.CallAllSources(ProductName.Trim().Trim('"', '/'));
            var product = new Product
            {
                ProductName = ProductName,
                Price = Price,
                Source = Source,
                RetrievedAt = DateTime.UtcNow
            };

            await _ProductService.SaveProductAsync(product);

            var responseDto = new ProductResponseDto
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Source = product.Source,
                RetrievedAt = product.RetrievedAt
            };

            return Ok(responseDto);
        }
    }
}
