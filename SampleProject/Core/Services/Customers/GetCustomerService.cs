using BusinessEntities;
using Common;
using Core.Services.Users;
using System.Linq;

namespace Core.Services.Customers
{
    [AutoRegister]
    public class GetCustomerService : IGetCustomerService
    {
        private IGetUserService _getUserService;

        public GetCustomerService(IGetUserService getUserService)
        {
            _getUserService = getUserService;
        }

        public User GetCustomer(string customerEmail)
        {
            var user = _getUserService.GetUsers(UserTypes.Customer, string.Empty, customerEmail).FirstOrDefault();
            return user;
        }
    }
}
