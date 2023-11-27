using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class BasketController : BaseApiController
  {
    private readonly StoreContext _context;
    public BasketController(StoreContext context)
    {
      this._context = context;

    }

    [HttpGet(Name = "GetBasket")]
    public async Task<ActionResult<BasketDto>> GetBasket()
    {
      var basket = await RetrieveBasket(GetBuyerId());

      if (basket == null) return NotFound();
      return basket.MapBasketToDto();
    }


    [HttpPost]
    public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
    {
      // get basket
      var basket = await RetrieveBasket(GetBuyerId());
      if (basket == null)
      {
        basket = CreateBasket();
      }

      // get product
      var product = await this._context.Products.FindAsync(productId);
      if (product == null) return BadRequest(new ProblemDetails { Title = "Product Not Found" });

      // add item 
      basket.AddItem(product, quantity);
      // save changes
      var result = await this._context.SaveChangesAsync() > 0;
      if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());
      return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
    {
      // get basket
      var basket = await RetrieveBasket(GetBuyerId());
      if (basket == null) return NotFound();

      // remove item or reduce quantity,
      basket.RemoveItem(productId, quantity);
      // Save changes
      var result = await this._context.SaveChangesAsync() > 0;
      if (result) return StatusCode(204);
      return BadRequest(new ProblemDetails { Title = "Problem removing item from basket" });
    }

    private async Task<Basket> RetrieveBasket(string buyerId)
    {
      if (string.IsNullOrEmpty(buyerId))
      {
        Response.Cookies.Delete("buyerId");
        return null;

      }

      var basket = await this._context.Baskets
        .Include(i => i.Items)
        .ThenInclude(p => p.Product)
        .FirstOrDefaultAsync(x => x.BuyerId == buyerId);

      if (basket == null) return null;

      return basket;
    }

    private string GetBuyerId()
    {
      return User.Identity?.Name ?? Request.Cookies["buyerId"];
    }


    private Basket CreateBasket()
    {
      var buyerId = User.Identity?.Name;
      if (string.IsNullOrEmpty(buyerId))
      {
        buyerId = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
        Response.Cookies.Append("buyerId", buyerId, cookieOptions);
      }

      var basket = new Basket { BuyerId = buyerId };
      this._context.Baskets.Add(basket);

      return basket;
    }




  }
}