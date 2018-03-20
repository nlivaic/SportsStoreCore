using System.Linq;

namespace SportsStore.Models {
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0) {
                _context.Products.Add(product);
            } else {
                Product dbEntry = _context.Products.FirstOrDefault(p =>p.ProductId == product.ProductId);
                if (dbEntry != null) {
                    dbEntry.Name = product.Name;
                    dbEntry.Category = product.Category;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                }
            }
            _context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null) {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return product;
        }
    }
}