using API.Entities.OrderAggregate;

namespace API.DTOs
{
  public class OrderItemDto : ProductItemOrdered
  {
    public long Price { get; set; }
    public int Quantity { get; set; }
  }
}