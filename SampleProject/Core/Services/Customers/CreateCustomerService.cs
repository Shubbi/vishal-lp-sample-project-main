using BusinessEntities;
using Common;
using Core.DTOs;
using Core.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Customers
{
    [AutoRegister]
    public class CreateCustomerService : ICreateCustomerService
    {
        private ICreateUserService _createUserService;

        public CreateCustomerService(ICreateUserService createUserService)
        {
            _createUserService = createUserService;
        }
        public User CreateCustomer(string customerName, string customerEmail)
        {
            var customer = _createUserService.Create(Guid.NewGuid(),customerName, customerEmail, UserTypes.Customer,null, new List<string>());

            return customer;
        }
    }
}
