using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class OrderRequestDto
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress {  get; set; }

        public List<OrderItemRequestDto> OrderItems { get; set; }
    }

    public class OrderItemRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
