using BusinessEntities;
using Common;
using Core.DTOs;
using Core.Factories;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdObjectFactory<Order> _orderFactory;
        private readonly IIdObjectFactory<Customer> _customerFactory;
        private readonly IIdObjectFactory<OrderItem> _itemFactory;

        public OrderService(
            IUnitOfWork unitOfWork,
            IIdObjectFactory<Order> orderFactory,
            IIdObjectFactory<Customer> customerFactory,
            IIdObjectFactory<OrderItem> itemFactory
            )
        {
            _unitOfWork = unitOfWork;
            _orderFactory = orderFactory;
            _customerFactory = customerFactory;
            _itemFactory = itemFactory;
        }

        //Sorry I'm violating Single Responsibility principle
        //As I'm also creating customer and orderitem here
        //Ideally I should do them in their own service      
        public OrderResponseDto PlaceOrder(OrderRequestDto orderRequest)
        {
            Customer customer = CheckAndCreateCustomer(orderRequest);

            var orderId = Guid.NewGuid();
            var order = _orderFactory.Create(orderId);
            order.Customer = customer;

            foreach (var item in orderRequest.OrderItems)
            {
                Product product = GetProduct(item);

                CreateOrderItem(order, item, product);
            }

            _unitOfWork.Orders.Add(order);
            _unitOfWork.Save();

            OrderResponseDto orderResponseDto = GetOrderResponseDto(orderId, order);

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

        private Customer CheckAndCreateCustomer(OrderRequestDto orderRequest)
        {
            var customer = _unitOfWork.Customers.GetCustomerByEmail(orderRequest.CustomerEmail);

            if (customer == null)
            {
                var customerId = Guid.NewGuid();

                customer = _customerFactory.Create(customerId);
                customer.Address = orderRequest.CustomerAddress;
                customer.SetEmail(orderRequest.CustomerEmail);
                customer.SetName(orderRequest.CustomerName);

                _unitOfWork.Customers.Add(customer);
            }

            return customer;
        }

        private static OrderResponseDto GetOrderResponseDto(Guid orderId, Order order)
        {
            var orderResponseDto = new OrderResponseDto()
            {
                OrderId = orderId,
                CustomerId = order.Customer.Id,
                CustomerName = order.Customer.Name,
                OrderTotal = order.TotalPrice
            };

            order.Items.ForEach(x =>
            {
                orderResponseDto.Items.Add(new OrderItemResponseDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductPrice = x.ProductPrice,
                    ProductQuantity = x.Quantity,
                    TotalItemPrice = x.TotalPrice()
                });
            });
            return orderResponseDto;
        }
    }
}
