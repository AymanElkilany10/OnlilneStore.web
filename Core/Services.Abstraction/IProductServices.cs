using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstraction
{
    public interface IProductServices
    {
        Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters SpecParam);
        Task <ProductResultDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
    }
}
