using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstraction;
using Shared;
using Shared.ErrorModels;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Cache(300)]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecificationParameters SpecParam)
        {
            var result = await serviceManager.ProductServices.GetAllProductsAsync(SpecParam);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
        {
            var result = await serviceManager.ProductServices.GetProductByIdAsync(id);

            if(result is null) return NotFound();

            return Ok(result);
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandResultDto>> GetAllBrands()
        {
            var result = await serviceManager.ProductServices.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }


        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof( IEnumerable<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductServices.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }
    }
}
