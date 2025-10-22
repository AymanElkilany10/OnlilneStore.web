using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IServiceManager
    {
        IProductServices ProductServices { get; }
        IBaskerServices BasketServices { get; }
        ICacheService CacheService { get; }
        IAuthService AuthService { get; }
        IOrderServices OrderService { get; }
        IPaymentServices PaymentService { get; }
    }
}
