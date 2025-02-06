using BusinessEntities;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface IOrderService
    {
        OrderResponseDto PlaceOrder(OrderRequestDto orderRequest);
    }
}
