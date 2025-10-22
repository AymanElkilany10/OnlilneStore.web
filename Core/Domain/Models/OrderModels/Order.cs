using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModels
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
        }
        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; }
        public Address ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Navigation property
        public DeliveryMethod DeliveryMethod { get; set; } // Navigation property
        public int? DeliveryMethodId { get; set; } // Foreign key

        public OrdrePaymentStatus PaymentStatus { get; set; } = OrdrePaymentStatus.Pending;

        public decimal Subtotal { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTime.UtcNow;
        public string PaymentIntentId { get; set; }

    }
}
