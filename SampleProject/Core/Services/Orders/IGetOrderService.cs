using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface IGetOrderService
    {
        Order GetOrder(Guid id);
        IEnumerable<Order> GetOrders();
        bool IsOrderAccessibleToCustomer(Guid Id, Guid customerId);
        IEnumerable<Order> GetOrdersForCustomer(Guid customerId);
    }
}
