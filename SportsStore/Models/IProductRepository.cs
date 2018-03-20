using System.Linq;
using SportsStore.Models;

namespace SportsStore.Models {
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);
    }
}