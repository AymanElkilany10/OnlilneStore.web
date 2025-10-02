using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecificationParameters specParams) : base(

                x =>
                 (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
                 (!specParams.BrandId.HasValue || x.BrandId == specParams.BrandId) &&
                 (!specParams.TypeId.HasValue || x.TypeId == specParams.TypeId)
            )

        {

        }




    }
}
