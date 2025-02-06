using BusinessEntities;
using Core.DTOs;

namespace Core.Services.Customers
{
    public interface ICreateCustomerService
    {
        User CreateCustomer(string customerName, string customerEmail);
    }
}