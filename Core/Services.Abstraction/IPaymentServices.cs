using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstraction
{
    public interface IPaymentServices
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
