using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repositories.InMemory
{
    //Registering as singleton because it is in memory
    //For Databases we will make it Scoped
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public IEnumerable<Order> GetOrdersForCustomer(Guid customerId)
        {
            return _items.FindAll(x => x.CustomerId == customerId);
        }

        public bool IsOrderAccessibleToCustomer(Guid Id, Guid customerId)
        {
            return _items.Any(x => x.Id == Id && x.CustomerId == customerId);
        }
    }
}
