using BusinessEntities;
using Core.DTOs;
using Core.Factories;
using Core.Services.Customers;
using Core.Services.Orders;
using Data.Repositories.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test
{
    //Not providing 100% Code coverage
    //Just implementing Few Unit Tests to show the Conceptual understanding of the  stuff
    [TestClass]
    public class CreateOrderServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IdObjectFactory<Order> _orderFactory = new IdObjectFactory<Order>();
        private IdObjectFactory<OrderItem> _orderItemFactory = new IdObjectFactory<OrderItem>();
        private IdObjectFactory<Product> _productFactory = new IdObjectFactory<Product>();
        private IdObjectFactory<User> _userFactory = new IdObjectFactory<User>();
        private Mock<IGetCustomerService> _mockGetCustomerService;
        private Mock<ICreateCustomerService> _mockCreateCustomerService;

        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IProductRepository> _mockProductRepository;

        private CreateOrderService _createOrderService;

        [TestInitialize]
        public void TestInitialize()
        {   
            _mockGetCustomerService = new Mock<IGetCustomerService>();
            _mockCreateCustomerService = new Mock<ICreateCustomerService>();
            
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);            

            _createOrderService = new CreateOrderService(
                _mockUnitOfWork.Object,
                _orderFactory,
                _orderItemFactory,
                _mockGetCustomerService.Object,
                _mockCreateCustomerService.Object
            );
        }

        [TestMethod]
        public void CreateOrderCreatesWithExistingProductAndCustomer()
        {
            //Arrange
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            var orderRequest = new OrderRequestDto
            {
                CustomerEmail = "vishal@company123.com",
                CustomerName = "vishal",
                OrderItems = new List<OrderItemRequestDto>
                {
                    new OrderItemRequestDto { ProductId = productId1, Quantity = 5 },
                    new OrderItemRequestDto { ProductId = productId2, Quantity = 11 }
                }
            };

            var existingCustomer = _userFactory.Create(Guid.NewGuid());
            existingCustomer.SetEmail("vishal@company123.com");
            existingCustomer.SetName("vishal");

            _mockGetCustomerService.Setup(s => s.GetCustomer("vishal@company123.com"))
                .Returns(existingCustomer);

            var product1 = _productFactory.Create(productId1);
            product1.Name = "Some Dummy Product Abc";
            product1.Price = 10m;

            var product2 = _productFactory.Create(productId2);
            product2.Name = "Some Dummy Product Xyz";
            product2.Price = 5m;
            
            _mockProductRepository.Setup(p => p.Get(productId1)).Returns(product1);
            _mockProductRepository.Setup(p => p.Get(productId2)).Returns(product2);            

            var orderItemId = Guid.NewGuid();
            var orderItem = _orderItemFactory.Create(orderItemId);
            var orderId = Guid.NewGuid();
            var order = _orderFactory.Create(orderId);
            order.Items = new List<OrderItem>() { orderItem };

            //Act
            var responseDto = _createOrderService.CreateOrder(orderRequest);

            Assert.AreEqual(2, responseDto.Items.Count);
            Assert.IsTrue(responseDto.Items.Exists(x => x.ProductId == productId1 && x.ProductQuantity == 5 && x.TotalItemPrice == 50));
            Assert.IsTrue(responseDto.Items.Exists(x => x.ProductId == productId2 && x.ProductQuantity == 11 && x.TotalItemPrice == 55));
        }

        [TestMethod]
        public void CreateOrderCreatesCustomerWhenNotExists()
        {
            //Arrange
            var productId1 = Guid.NewGuid();

            var orderRequest = new OrderRequestDto
            {
                CustomerEmail = "Test123@company123.com",
                CustomerName = "Test123",
                OrderItems = new List<OrderItemRequestDto>()
                {
                    new OrderItemRequestDto
                    {
                        ProductId = productId1,
                        Quantity = 1
                    }
                }
            };

            _mockGetCustomerService.Setup(s => s.GetCustomer("Test123@company123.com"))
                .Returns((User)null);

            var newCustomerId = Guid.NewGuid();
            var newCustomer = _userFactory.Create(newCustomerId);
            newCustomer.SetEmail("Test123@company123.com");

            newCustomer.SetName("Test123");
            _mockCreateCustomerService.Setup(s => s.CreateCustomer("Test123", "Test123@company123.com"))
                .Returns(newCustomer);

            var product1 = _productFactory.Create(productId1);
            product1.Name = "Some Dummy Product Abc";
            product1.Price = 10m;

            _mockProductRepository.Setup(p => p.Get(productId1)).Returns(product1);

            var orderItemId = Guid.NewGuid();
            var orderItem = _orderItemFactory.Create(orderItemId);
            var orderId = Guid.NewGuid();
            var order = _orderFactory.Create(orderId);
            order.Items = new List<OrderItem>() { orderItem };

            //Act
            var responseDto = _createOrderService.CreateOrder(orderRequest);

            //Assert
            Assert.AreEqual(newCustomerId, responseDto.CustomerId);

            _mockCreateCustomerService.Verify(x => x.CreateCustomer("Test123", "Test123@company123.com"), Times.Once);
            _mockOrderRepository.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrderFailsWhenProductIsNotFound()
        {
            //Arrange
            var invalidProductId = Guid.NewGuid();
            var orderRequest = new OrderRequestDto
            {
                CustomerEmail = "vishal@company123.com",
                CustomerName = "vishal",
                OrderItems = new List<OrderItemRequestDto>
                {
                    new OrderItemRequestDto { ProductId = invalidProductId, Quantity = 1 }
                }
            };
            
            var customerId = Guid.NewGuid();
            var existingCustomer = _userFactory.Create(customerId);

            existingCustomer.SetEmail("vishal@company123.com");
            existingCustomer.SetName("Vishal");
            _mockGetCustomerService.Setup(s => s.GetCustomer("vishal@company123.com"))
                .Returns(existingCustomer);

            _mockProductRepository.Setup(p => p.Get(invalidProductId)).Returns((Product)null);            

            //Act
            _createOrderService.CreateOrder(orderRequest);

            //Assert
            //[ExpectedException]
        }
    }
}
