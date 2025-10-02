using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(p => p.Id == id)
        {
            ApplyIncludes();
        }

        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationParameters SpecParam) :
            base(
                P =>
                    (string.IsNullOrEmpty(SpecParam.Search) || P.Name.ToLower().Contains(SpecParam.Search)) &&
                    (!SpecParam.BrandId.HasValue || P.BrandId == SpecParam.BrandId) &&
                    (!SpecParam.TypeId.HasValue || P.TypeId == SpecParam.TypeId)

                )
        {
            ApplyIncludes();
            ApplySorting(SpecParam.Sort);
            ApplyPagination(SpecParam.PageIndex, SpecParam.PageSize);
        }

        private void ApplyIncludes()
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
        private void ApplySorting(string? sort)
        {
            if (string.IsNullOrEmpty(sort)) return;
            switch (sort.ToLower())
            {
                case "nameasc":
                    AddOrderBy(p => p.Name);
                    break;
                case "namedesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                case "priceasc":
                    AddOrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }

        }


    }
}
