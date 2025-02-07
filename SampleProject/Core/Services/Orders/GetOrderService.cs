using BusinessEntities;
using Common;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class GetOrderService : IGetOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Order GetOrder(Guid id)
        {
            return _unitOfWork.Orders.Get(id);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _unitOfWork.Orders.GetAll();
        }
        public IEnumerable<Order> GetOrdersForCustomer(Guid customerId)
        {
            return _unitOfWork.Orders.GetOrdersForCustomer(customerId);
        }
        public bool IsOrderAccessibleToCustomer(Guid Id, Guid customerId)
        {
            return _unitOfWork.Orders.IsOrderAccessibleToCustomer(Id, customerId);
        }
    }
}
