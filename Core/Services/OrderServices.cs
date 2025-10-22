using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Services.Abstraction;
using Services.Specifications;
using Shared.OrderModels;

namespace Services
{
    public class OrderServices(
        IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderServices
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {
            var address = mapper.Map<Address>(orderRequest.ShipToAddress);

            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);

            if (basket == null) throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int >().GetAsync(item.Id);
                if(product is null) throw new ProductNotFoundExceptions(item.Id);

                var orderItem = new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, product.Price);
                orderItems.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod == null) throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existOrder = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);

            if (existOrder != null)
                unitOfWork.GetRepository<Order, Guid>().Delete(existOrder);

            var order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal, basket.PaymentIntentId);

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            var count = await unitOfWork.SaveChangesAsync();


            if (count == 0) throw new OrderCreateBadRequestException();

            var result = mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
            return result;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);
            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if (order == null) throw new OrderNotFoundException(id);

            var result = mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail)
        {
            var spec = new OrderSpecifications(userEmail);
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);

            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);
            return result;
        }
    }
}
