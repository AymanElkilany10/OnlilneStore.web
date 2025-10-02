using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DbInitializer : IDbIntitializer
    {
        private readonly StoreDbContext _dbContext;

        public DbInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
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
            }
            catch (Exception ex) 
            {
                throw;
            }

        }

        
        
    }
}
