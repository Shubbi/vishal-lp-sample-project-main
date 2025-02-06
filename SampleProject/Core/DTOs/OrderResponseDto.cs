using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DTOs
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }        
        public decimal OrderTotal { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItemResponseDto> Items { get; } = new List<OrderItemResponseDto>();

        public static OrderResponseDto GetOrderResponseDto(Order order)
        {
            if(order == null || order.Items == null || !order.Items.Any())
                return null;

            var orderResponseDto = new OrderResponseDto()
            {
                OrderId = order.Id,
                CustomerId = order.Customer.Id,
                CustomerName = order.Customer.Name,
                OrderTotal = order.TotalPrice
            };

            order.Items.ForEach(x =>
            {
                orderResponseDto.Items.Add(new OrderItemResponseDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductPrice = x.ProductPrice,
                    ProductQuantity = x.Quantity,
                    TotalItemPrice = x.TotalPrice()
                });
            });

            return orderResponseDto;
        }
    }
}
