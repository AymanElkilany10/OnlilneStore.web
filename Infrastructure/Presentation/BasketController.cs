using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await serviceManager.BasketServices.GetBasketAsync(id);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var result = await serviceManager.BasketServices.UpdateBasketAsync(basketDto);
            return Ok(result);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBasketById(string id)
        {
            await serviceManager.BasketServices.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
