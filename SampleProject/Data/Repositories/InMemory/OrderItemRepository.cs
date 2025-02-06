using BusinessEntities;
using Common;

namespace Data.Repositories.InMemory
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
    }
}
