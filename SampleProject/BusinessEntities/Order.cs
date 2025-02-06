using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace BusinessEntities
{
    public class Order : IdObject
    {
        //Calculated field
        public Decimal TotalPrice => CalculateTotalPrice();
        public DateTime CreatedDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> Items { get; set;} = new List<OrderItem>();
        public Guid CustomerId { get; set; }
        public User Customer { get; set; }

        //Keeping it simple for now
        public string ShippingAddress { get; set; }
        public DateTime? ShippingDate { get; set; }

        private Decimal CalculateTotalPrice()
        {
            var totalPrice = 0m;

            foreach (var item in Items) {
                totalPrice += item.TotalPrice();
            }

            return totalPrice;
        }
    }
}
