using Core.DTOs;
using Core.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IGetOrderService _getOrderService;

        public OrderController(
            ICreateOrderService createOrderService, 
            IGetOrderService getOrderService)
        {
            _createOrderService = createOrderService;
            _getOrderService = getOrderService;
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] OrderRequestDto model)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var orderResponse = _createOrderService.CreateOrder(model);

            return Success(orderResponse);
        }

        //ToDo - Implement Authorization to ensure that only Admin can access this
        //Maybe - moving it to an Admin controller might be a good idea
        [Route("{orderId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateOrder(Guid orderId, [FromBody] OrderRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var order = _getOrderService.GetOrder(orderId);

            if (order == null)
            {
                return DoesNotExist();
            }

            //_updateProductService.Update(product, model.ProductName, model.ProductDescription, model.Price, model.StockQuantity);
            var orderResponse = OrderResponseDto.GetOrderResponseDto(order);

            return Success(orderResponse);
        }

        [Route("{orderId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetOrder(Guid orderId)
        {
            var order = _getOrderService.GetOrder(orderId);

            if (order != null)
            {
                var orderResponse = OrderResponseDto.GetOrderResponseDto(order);

                return Success(orderResponse);
            }

            return DoesNotExist();
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetOrders()
        {
            var orders = _getOrderService.GetOrders();

            if (orders != null && orders.Any())
            {
                var orderResponse =  orders.Select(x => OrderResponseDto.GetOrderResponseDto(x));
                return Success(orders);
            }

            return Success(new List<OrderResponseDto>());
        }
        
        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetOrdersForACustomer()
        {
            var orders = _getOrderService.GetOrders();

            if (orders != null && orders.Any())
            {
                var orderResponse = orders.Select(x => OrderResponseDto.GetOrderResponseDto(x));
                return Success(orders);
            }

            return Success(new List<OrderResponseDto>());
        }
    }
}
