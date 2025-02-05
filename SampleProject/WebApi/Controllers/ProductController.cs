using Core.Services.Products;
using Core.Services.Users;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Products;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private readonly ICreateProductService _createProductService;
        private readonly IGetProductService _getProductService;

        public ProductController(ICreateProductService createProductService, IGetProductService getProductService, IUnitOfWork unitOfWork)
        {
            _createProductService = createProductService;      
            _getProductService = getProductService;
        }

        [Route("{productId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateProduct(Guid productId, [FromBody] ProductModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var product = _getProductService.GetProduct(productId);
            if (product != null)
            {
                return ConflictError($"product {productId} already exists");
            }

            product = _createProductService.Create(productId, model.ProductName, model.ProductDescription, model.Price, model.StockQuantity);
            return Success(new ProductData(product));
        }

        [Route("{productId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);
            return Success(new ProductData(product));
        }
    }
}
