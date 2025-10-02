using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Abstraction;
using Shared;

namespace Services
{
    public class BasketServices(IBasketRepository basketRepository, IMapper mapper) : IBaskerServices
    {
        public async Task<BasketDto?> GetBasketAsync(string Id)
        {
            var basket = await basketRepository.GetBasketAsync(Id);
            if (basket == null) throw new BasketNotFoundException(Id);
            
            var result = mapper.Map<BasketDto>(basket);
            return result;
        }

        public async Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);
            basket = await basketRepository.UpdateBasketAsync(basket);

            if(basket == null)
                throw new BasketCreateOrUpdateBadRequestException();

            var result = mapper.Map<BasketDto>(basket);
            return result;
        } 

        public async Task<bool> DeleteBasketAsync(string Id)
        {
            var flag = await basketRepository.DeleteBasketAsync(Id);

            if (!flag)
                throw new BasketDeleteBadRequest();

            return flag;
        }
    }
}
