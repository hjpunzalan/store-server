using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BasketController:BaseApiController
    {
        private readonly StoreContext _context;
        public BasketController(StoreContext context)
        {
            this._context = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket() {
            var basket = await this._context.Baskets.Include(i => i.Items).ThenInclude(p => p.Product).FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);

            if (basket == null) return NotFound();

            return basket;
        }

        [HttpPost]
        public Task<ActionResult> AddItemToBasket (int productId, int quantity) {
            // get basket
            // get product
            // add item
            // save changes


            return StatusCode(201);
        }

        [HttpDelete]
        public async Task<ActionResult>  RemoveBasketItem(int productId, int quantity) {
            // get basket
            // remove item or reduce quanitty,
            // Save changes
            return Ok();
        }
        
    }
}