using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IOrderServices
    {
        Task<OrderResultDto> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail);
        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest,string userEmail); //
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();
    }
}
