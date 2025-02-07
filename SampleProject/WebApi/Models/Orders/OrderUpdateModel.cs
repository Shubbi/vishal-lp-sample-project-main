using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Orders
{
    public class OrderUpdateModel
    {
        public OrderStatus OrderStatus{ get; set; }
    }
}