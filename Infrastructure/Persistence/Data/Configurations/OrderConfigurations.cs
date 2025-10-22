using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, Address => Address.WithOwner());

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

            builder.Property(o => o.PaymentStatus)
                .HasConversion( v => v.ToString(), v => Enum.Parse<OrdrePaymentStatus>(v));

            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,4)");
        }
    }
}
