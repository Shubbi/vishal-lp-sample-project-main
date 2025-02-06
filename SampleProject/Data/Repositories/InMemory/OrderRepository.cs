using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories.InMemory
{
    //Registering as singleton because it is in memory
    //For Databases we will make it Scoped
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
    }
}
