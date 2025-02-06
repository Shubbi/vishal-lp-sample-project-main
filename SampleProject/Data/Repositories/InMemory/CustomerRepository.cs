using BusinessEntities;
using Common;
using System.Linq;

namespace Data.Repositories.InMemory
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public Customer GetCustomerByEmail(string email)
        {
            return _items.FirstOrDefault(e => e.Email == email);
        }
    }
}
