using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository) : IServiceManager
    {
        public IProductServices ProductServices { get; } = new ProductServices(unitOfWork, mapper);
        public IBaskerServices BasketServices { get; } = new BasketServices(basketRepository, mapper);
    }
}
