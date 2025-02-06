using BusinessEntities;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface ICreateOrderService
    {
        OrderResponseDto CreateOrder(OrderRequestDto orderRequest);
    }
}
