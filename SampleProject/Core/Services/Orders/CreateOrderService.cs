using BusinessEntities;
using Common;
using Core.DTOs;
using Core.Factories;
using Core.Services.Customers;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    [AutoRegister(AutoRegisterTypes.Scope)]
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdObjectFactory<Order> _orderFactory;        
        private readonly IIdObjectFactory<OrderItem> _itemFactory;
        private readonly IGetCustomerService _getCustomerService;
        private readonly ICreateCustomerService _createCustomerService;

        public CreateOrderService(
            IUnitOfWork unitOfWork,
            IIdObjectFactory<Order> orderFactory,
            IIdObjectFactory<OrderItem> itemFactory,
            IGetCustomerService getCustomerService,
            ICreateCustomerService createCustomerService
            )
        {
            _unitOfWork = unitOfWork;
            _orderFactory = orderFactory;
            _itemFactory = itemFactory;
            _getCustomerService = getCustomerService;
            _createCustomerService = createCustomerService;
        }

        //Sorry I'm violating Single Responsibility principle
        //As I'm also creating customer and orderitem here
        //Ideally I should do them in their own service      
        public OrderResponseDto CreateOrder(OrderRequestDto orderRequest)
        {
            User customer = ProcessCustomer(orderRequest);

            if (customer == null)
            {
                throw new Exception("Problem processing Customer Data");
            }

            var orderId = Guid.NewGuid();
            var order = _orderFactory.Create(orderId);
            order.Customer = customer;
            order.CustomerId = customer.Id;

            foreach (var item in orderRequest.OrderItems)
            {
                Product product = GetProduct(item);

                CreateOrderItem(order, item, product);
            }

            _unitOfWork.Orders.Add(order);
            _unitOfWork.Save();

            OrderResponseDto orderResponseDto = OrderResponseDto.GetOrderResponseDto(order);

            return orderResponseDto;
        }

        private Product GetProduct(OrderItemRequestDto item)
        {
            var product = _unitOfWork.Products.Get(item.ProductId);

            if (product == null)
            {
                throw new ArgumentException($"Invalid ProductId {item.ProductId} provided");
            }

            return product;
        }

        private void CreateOrderItem(Order order, OrderItemRequestDto item, Product product)
        {
            var orderItemId = Guid.NewGuid();

            var orderItem = _itemFactory.Create(orderItemId);
            orderItem.ProductId = item.ProductId;
            orderItem.ProductName = product.Name;
            orderItem.ProductPrice = product.Price;
            orderItem.Quantity = item.Quantity;

            order.Items.Add(orderItem);
        }

        private User ProcessCustomer(OrderRequestDto orderRequest)
        {
            var customer = _getCustomerService.GetCustomer(orderRequest.CustomerEmail);

            if (customer == null)
            {
                customer = _createCustomerService.CreateCustomer(orderRequest.CustomerName, orderRequest.CustomerEmail);
            }

            return customer;
        }
    }
}
