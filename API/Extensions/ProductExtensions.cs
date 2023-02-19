
namespace API.Extensions
{
    public class ProductExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy){

            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.name);
            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            return query;
        }
    };
}