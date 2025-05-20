using core.Entities;
using core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
 
    public class CartController(ICartService cartService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCart(string id)
        {
            var data = await cartService.GetCartAsync(id);
            return Ok(data ?? new ShoppingCart { Id = id});
        }
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await cartService.SetCartAsync(cart);
            if(updatedCart == null) return BadRequest(new ProblemDetails { Title = "Problem with your cart" });
            return Ok(updatedCart);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCart(string id)
        {
            var deleted = await cartService.DeleteCartAsync(id);
            if (!deleted) return BadRequest(new ProblemDetails { Title = "Problem deleting cart" });
            return Ok(deleted);
        }


    }
}
