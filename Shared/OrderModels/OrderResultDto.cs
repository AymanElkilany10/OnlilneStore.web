using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrderModels;

namespace Shared.OrderModels
{
    public class OrderResultDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>(); // Navigation property
        public string DeliveryMethod { get; set; } 

        public string PaymentStatus { get; set; }

        public decimal Subtotal { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTime.UtcNow;
        public string PaymentIntentId { get; set; }  = string.Empty;
        public decimal Total { get; set; }
    }
}
