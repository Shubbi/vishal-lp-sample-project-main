using BusinessEntities;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Customers
{
    public interface IGetCustomerService
    {
        User GetCustomer(string customerEmail);
    }
}
