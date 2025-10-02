using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                var query = _dbContext.Products.Include(p => p.ProductType).Include(p => p.ProductBrand);

                return trackChanges
                    ? await query.ToListAsync() as IEnumerable<TEntity>
                    : await query.AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }

            return trackChanges
                ? await _dbContext.Set<TEntity>().ToListAsync()
                : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Tkey id)
        {
            
            if (typeof(TEntity) == typeof(Product))
            {
                return await _dbContext.Products
                    .Include(p => p.ProductType)
                    .Include(p => p.ProductBrand)
                    .FirstOrDefaultAsync(p => p.Id.Equals(id)) as TEntity;
            }

            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }
        
        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> spec, bool trackChanges = false)
        {
            return  await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, Tkey> spec)
        {
            return SpecificationEvaluator.GetQuery(_dbContext.Set<TEntity>(), spec);
        }

        
    }
}
