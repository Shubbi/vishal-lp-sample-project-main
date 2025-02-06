using System;
using System.Collections.Generic;
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
    }

    public class OrderItemResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public decimal TotalItemPrice { get; set; }

    }
}
