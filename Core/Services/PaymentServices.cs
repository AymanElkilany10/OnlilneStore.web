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
using Microsoft.Extensions.Configuration;
using Services.Abstraction;
using Shared;
using Shared.ErrorModels;
using Stripe;
using OrderProduct = Domain.Models.Product;

namespace Services
{
    public class PaymentServices(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : IPaymentServices
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket == null) throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
               var product = await unitOfWork.GetRepository<OrderProduct, int>().GetAsync(item.Id);
                if (product == null) throw new ProductNotFoundExceptions(item.Id);
                item.Price = product.Price;
            }

            if(!basket.DeliveryMethodId.HasValue) throw new Exception("Invalid Delivery method id");

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod == null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Cost;

            var amount = (long) (basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice.Value) * 100;

            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];


            var service = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var paymentIntent = await service.CreateAsync(createOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                };

                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await basketRepository.UpdateBasketAsync(basket);
            var result = mapper.Map<BasketDto>(basket);

            return result;

        }
    }
}
