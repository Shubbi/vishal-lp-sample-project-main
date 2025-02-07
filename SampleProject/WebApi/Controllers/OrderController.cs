using BusinessEntities;
using Core.DTOs;
using Core.Services.Orders;
using Raven.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebApi.Filters;
using WebApi.Models.Orders;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IUpdateOrderService _updateOrderService;

        public OrderController(
            ICreateOrderService createOrderService, 
            IGetOrderService getOrderService,
            IUpdateOrderService updateOrderService)
        {
            _createOrderService = createOrderService;
            _getOrderService = getOrderService;
            _updateOrderService = updateOrderService;   
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

        //For Simplicity - I'm only allowing to change OrderStatus for now        
        [Route("{orderId:guid}/update")]
        [CustomAuthFilter("Admin", "Customer")]
        [HttpPut]
        public HttpResponseMessage UpdateOrderStatus(Guid orderId, [FromBody]OrderUpdateModel orderUpdateModel)
        {
            var userIdClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Role);

            if (userRoleClaim.Value == "Customer"
                && !_getOrderService.IsOrderAccessibleToCustomer(orderId, Guid.Parse(userIdClaim.Value)))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var order = _getOrderService.GetOrder(orderId);

            if (order == null)
            {
                return DoesNotExist();
            }

            if (Enum.TryParse(userRoleClaim.Value, out UserTypes userType))
            {
                try
                {
                    _updateOrderService.Update(order, orderUpdateModel.OrderStatus, userType);
                    return Success();
                }
                catch (InvalidOperationException ex)
                {
                    //Not very good use of Bad Request
                    // As it could be Forbidden or Conflict depending on the error type
                    return BadRequestError(ex.Message);
                }
            }

            return DoesNotExist();
        }

        [Route("{orderId:guid}")]
        [HttpGet]
        [CustomAuthFilter("Admin","Customer")]
        public HttpResponseMessage GetOrder(Guid orderId)
        {
            var userIdClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Role);

            if (userRoleClaim.Value == "Customer" 
                && !_getOrderService.IsOrderAccessibleToCustomer(orderId, Guid.Parse(userIdClaim.Value)))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

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
        [CustomAuthFilter("Admin")]
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
        [CustomAuthFilter("Admin", "Customer")]
        public HttpResponseMessage GetOrdersForCustomer(Guid customerId)
        {
            var userIdClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Role);

            if (userRoleClaim.Value == "Customer" && customerId != Guid.Parse(userIdClaim.Value))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var orders = _getOrderService.GetOrdersForCustomer(customerId);

            if (orders != null && orders.Any())
            {
                var orderResponse = orders.Select(x => OrderResponseDto.GetOrderResponseDto(x));
                return Success(orders);
            }

            return Success(new List<OrderResponseDto>());
        }
    }
}
