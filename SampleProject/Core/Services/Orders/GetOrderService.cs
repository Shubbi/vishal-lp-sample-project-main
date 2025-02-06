using BusinessEntities;
using Common;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    [AutoRegister(AutoRegisterTypes.Scope)]
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
    }
}
