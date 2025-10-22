using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Identity;

namespace Persistence
{
    public class DbInitializer : IDbIntitializer
    {
        private readonly StoreDbContext _dbContext;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            StoreDbContext dbContext,
            StoreIdentityDbContext identityDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _identityDbContext = identityDbContext;
           _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();


                // Seeding Prodcut Types From Json Files
                if (!_dbContext.ProductTypes.Any())
                {
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    if (types is not null && types.Any())
                    {
                        await _dbContext.ProductTypes.AddRangeAsync(types);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // Seeding Prodcut Brands From Json Files
                if (!_dbContext.ProductBrands.Any())
                {
                    var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                    if (brands is not null && brands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(brands);
                        await _dbContext.SaveChangesAsync();
                    }

                }

                // Seeding Prodcut From Json Files
                if (!_dbContext.Products.Any())
                {
                    var ProductData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                    if (products is not null && products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // Seeding Delivery Methods From Json Files
                if (!_dbContext.DeliveryMethods.Any())
                {
                    var DeliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");

                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        await _dbContext.DeliveryMethods.AddRangeAsync(deliveryMethods);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex) 
            {
                throw;
            }

        }

        public async Task InitializeIdentityAsync()
        {
            if(_identityDbContext.Database.GetPendingMigrations().Any())
            {
                await _identityDbContext.Database.MigrateAsync();
            }

            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });
                
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
            }

            if(! _userManager.Users.Any())
            {
                var SuperAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01011223344"
                };
                
                var AdminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01011112222"
                };

                await _userManager.CreateAsync(SuperAdminUser, "P@ssW0rd");
                await _userManager.CreateAsync(AdminUser, "P@ssW0rd");

                await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");
            }

            
        }


    }
}
