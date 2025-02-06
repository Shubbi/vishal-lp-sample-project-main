using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class OrderRequestDto
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress {  get; set; }
        public List<OrderItemRequestDto> OrderItems { get; set; }
    }
}
