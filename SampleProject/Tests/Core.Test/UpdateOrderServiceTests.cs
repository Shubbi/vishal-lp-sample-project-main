using BusinessEntities;
using Core.Services.Orders;
using Data.Repositories.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Core.Test
{    
    //Not providing 100% Code coverage
    //Just implementing Few Unit Tests to show the Conceptual understanding of the  stuff
    [TestClass]
    public class UpdateOrderServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOrderRepository> _mockOrderRepository;
        private UpdateOrderService _updateOrderService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();

            _mockUnitOfWork.Setup(x => x.Orders).Returns(_mockOrderRepository.Object);

            _updateOrderService = new UpdateOrderService(_mockUnitOfWork.Object);
        }

        //Showing only Authorized user can perform Certain operations
        //For Ex - Cutomer can not change Pending to processed 
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CustomerCanNotChangeOrderFromPendingToProcessed()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Pending };
            var newOrderStatus = OrderStatus.Processing;
            var userType = UserTypes.Customer;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            //We are going to throw the exception
            //Which should be handled in the attribute [ExpectedException] placed above 
        }

        [TestMethod]
        public void CustomerCanChangeOrderFromPendingToCancelled()
        {
            //Arrange
            var orderId= Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Pending };
            var newOrderStatus = OrderStatus.Cancelled;
            var userType = UserTypes.Customer;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            Assert.AreEqual(newOrderStatus, order.Status);

            _mockOrderRepository.Verify(x => x.Update(order), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotChangeStatusOnceOrderIsCancelled()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Cancelled };
            //Trying to see if we can put a Cancelled order back in processing
            var newOrderStatus = OrderStatus.Processing;
            var userType = UserTypes.Admin;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            //[ExpectedException] 
        }

        

        [TestMethod]
        public void AdminCanChangeOrderFromPendingToProcessing()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Pending };
            var newOrderStatus = OrderStatus.Processing;
            var userType = UserTypes.Admin;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            Assert.AreEqual(newOrderStatus, order.Status);

            _mockOrderRepository.Verify(x => x.Update(order), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }

        //Showing even an Admin cannot jump sequence of order status
        //like making a processing order delivered
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AdminCanNotChangeOrderFromProcessingToDelivered()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Processing };
            var newOrderStatus = OrderStatus.Delivered;
            var userType = UserTypes.Admin;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);
        }

        [TestMethod]
        public void AdminCanChangeOrderFromProcessingToShipped()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Processing };
            var newOrderStatus = OrderStatus.Shipped;
            var userType = UserTypes.Admin;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            Assert.AreEqual(newOrderStatus, order.Status);

            _mockOrderRepository.Verify(x => x.Update(order), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }


        [TestMethod]
        public void AdminCanChangeOrderFromShippedToDelivered()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Shipped };
            var newOrderStatus = OrderStatus.Delivered;
            var userType = UserTypes.Admin;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            Assert.AreEqual(newOrderStatus, order.Status);

            _mockOrderRepository.Verify(x => x.Update(order), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void CustomerCanInitiateReturnRequestOnADeliveredItem()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.Delivered };
            var newOrderStatus = OrderStatus.ReturnRequested;
            var userType = UserTypes.Customer;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            Assert.AreEqual(newOrderStatus, order.Status);

            _mockOrderRepository.Verify(x => x.Update(order), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }

        //Admin can mark item as returned
        //once company receive the items back from customer
        [TestMethod]
        public void AdminCanCompleteTheReturnRequestInitiatedByCustomer()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Status = OrderStatus.ReturnRequested };
            var newOrderStatus = OrderStatus.Returned;
            var userType = UserTypes.Admin;

            //Act
            _updateOrderService.Update(order, newOrderStatus, userType);

            //Assert
            Assert.AreEqual(newOrderStatus, order.Status);

            _mockOrderRepository.Verify(x => x.Update(order), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }
    }
}
