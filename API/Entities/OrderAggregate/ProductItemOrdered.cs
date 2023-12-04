using Microsoft.EntityFrameworkCore;

namespace API.Entities.OrderAggregate
{

  [Owned]
  public class ProductItemOrdered
  {
    public string ProductId { get; set; }
    public string Name { get; set; }

    public string PictureUrl { get; set; }


  }
}