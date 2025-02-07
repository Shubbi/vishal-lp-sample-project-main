using BusinessEntities;
using System;
using System.Collections.Generic;

namespace Data.Repositories.InMemory
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IEnumerable<Order> GetOrdersForCustomer(Guid customerId);
        bool IsOrderAccessibleToCustomer(Guid Id, Guid customerId);
    }
}
