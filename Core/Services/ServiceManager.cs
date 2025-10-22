using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared;

namespace Services
{
    public class ServiceManager(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository,
        ICacheRepository cacheRepository,
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        IOptions<JwtOptions> options) : IServiceManager
    {
        public IProductServices ProductServices { get; } = new ProductServices(unitOfWork, mapper);
        public IBaskerServices BasketServices { get; } = new BasketServices(basketRepository, mapper);
        public ICacheService CacheService  {get;} = new CacheService(cacheRepository);
        public IAuthService AuthService { get; } = new AuthService(userManager, options, mapper);
        public IOrderServices OrderService { get; } = new OrderServices(mapper,basketRepository ,unitOfWork);

        public IPaymentServices PaymentService { get; } = new PaymentServices(basketRepository, unitOfWork, mapper, configuration);
    }
}
