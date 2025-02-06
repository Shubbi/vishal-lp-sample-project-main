using Core.DTOs;
using Core.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage HandleOrder([FromBody] OrderRequestDto model)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequestError(ModelState);
            }

            var orderResponse = _orderService.PlaceOrder(model);

            return Success(orderResponse);
        }
    }
}
