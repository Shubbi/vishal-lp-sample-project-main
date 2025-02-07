using BusinessEntities;
using Common;
using Data.Repositories.InMemory;
using System;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Update(Order order, OrderStatus orderStatus, UserTypes userType)
        {
            if(CanUpdateOrderStatus(order.Status, orderStatus, userType))
            {
                order.Status = orderStatus;
                _unitOfWork.Orders.Update(order);
                _unitOfWork.Save();

                return;
            }

            //Not very good use of Bad Request
            // As it could be Forbidden or Conflict depending on the error type
            //We may have to modify CanUpdateOrderStatus method to get more better representation of error
            var errorMessage = $"Current Order Status of {order.Status} is not compatible with new status {orderStatus} " +
                $"or user may not have permission";

            throw new InvalidOperationException(errorMessage);
        }

        private static bool CanUpdateOrderStatus(OrderStatus currentStatus, OrderStatus newStatus, UserTypes userType)
        {
            switch (currentStatus)
            {
                case OrderStatus.Pending:
                    //can change to Processing or Cancelled.
                    //if we are changing status to Processing, user must be Admin or Employee.
                    return (newStatus == OrderStatus.Processing
                            && (userType == UserTypes.Admin || userType == UserTypes.Employee))
                        || newStatus == OrderStatus.Cancelled;

                case OrderStatus.Processing:
                    //can change to Shipped or Cancelled.
                    //But if we are changing to Shipped, then can only be done by user Admin or Employee.
                    return (newStatus == OrderStatus.Shipped
                            && (userType == UserTypes.Admin || userType == UserTypes.Employee))
                        || newStatus == OrderStatus.Cancelled;

                case OrderStatus.Shipped:
                    //can only change to Delivered, but only by an Admin or Employee.
                    return newStatus == OrderStatus.Delivered
                        && (userType == UserTypes.Admin || userType == UserTypes.Employee);

                case OrderStatus.Delivered:
                    //can only change to ReturnRequested.
                    return newStatus == OrderStatus.ReturnRequested;

                case OrderStatus.ReturnRequested:
                    //can only change to Returned, must be Admin or Employee.
                    return newStatus == OrderStatus.Returned
                        && (userType == UserTypes.Admin || userType == UserTypes.Employee);

                case OrderStatus.Returned:
                case OrderStatus.Cancelled:
                    //If Returned or Cancelled, you can not do anything
                    return false;

                default:
                    return false;
            }
        }
    }
}
