using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Abstraction;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductServices(IUnitOfWork unitOfWork, IMapper mapper) : IProductServices
    {
        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters SpecParam)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(SpecParam);

            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecifications(SpecParam);

            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(specCount);

            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return new PaginationResponse<ProductResultDto>(SpecParam.PageIndex, SpecParam.PageSize, count, result);
        }
        
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(id); 

            var product = await unitOfWork.GetRepository<Product,int>().GetAsync(id);

            if(product is null)  throw new ProductNotFoundExceptions(id);

            var result = mapper.Map<ProductResultDto>(product);

            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            var result =  mapper.Map<IEnumerable<BrandResultDto>>(brands);

            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);

            return result;
        }

        
    }
}
