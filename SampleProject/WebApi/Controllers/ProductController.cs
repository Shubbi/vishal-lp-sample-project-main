using BusinessEntities;
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
        private readonly IUpdateProductService _updateProductService;
        private readonly IGetProductService _getProductService;
        private readonly IDeleteProductService _deleteProductService;

        public ProductController(
            ICreateProductService createProductService, 
            IUpdateProductService updateProductService,
            IGetProductService getProductService, 
            IDeleteProductService deleteProductService)
        {
            _createProductService = createProductService;
            _updateProductService = updateProductService;
            _getProductService = getProductService;
            _deleteProductService = deleteProductService;
        }

        //ToDo - Implement Authorization to ensure that only Admin can access this
        //Maybe - moving it to an Admin controller might be a good idea

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateProduct([FromBody] ProductModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var product = _createProductService.Create(model.ProductName, model.ProductDescription, model.Price, model.StockQuantity);
            return Success(new ProductData(product));
        }

        [Route("{productId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);

            if (product != null)
            {
                return Success(new ProductData(product));
            }

            return DoesNotExist();
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetProducts()
        {
            var products = _getProductService.GetProducts();

            return Success(products);
        }

        //ToDo - Implement Authorization to ensure that only Admin can access this
        //Maybe - moving it to an Admin controller might be a good idea
        [Route("{productId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(Guid productId, [FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var product = _getProductService.GetProduct(productId);

            if (product == null)
            {
                return DoesNotExist();
            }

            _updateProductService.Update(product, model.ProductName, model.ProductDescription, model.Price, model.StockQuantity);
            return Success(new ProductData(product));
        }

        //ToDo - Implement Authorization to ensure that only Admin can access this
        //Maybe - moving it to an Admin controller might be a good idea
        [Route("{productId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);

            if (product != null)
            {
                _deleteProductService.Delete(product);
                return Success();
            }
            
            return DoesNotExist();
        }

        //ToDo - Implement Authorization to ensure that only Admin can access this
        //Maybe - moving it to an Admin controller might be a good idea
        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllProducts()
        {
            _deleteProductService.DeleteAll();
            return Success();
        }
    }
}
