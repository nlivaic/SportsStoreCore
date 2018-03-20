using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Linq;

namespace SportsStore.Controllers {
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int PageSize = 2;

        public ProductController(IProductRepository repository)
        {
            this._repository = repository;
        }

        public IActionResult List(string category, int productPage = 1) 
            => View(new ProductsListViewModel {
                        Products = _repository.Products
                                .Where(c => string.IsNullOrEmpty(category) || c.Category == category)
                                .OrderBy(p => p.ProductId)
                                .Skip((productPage - 1) * PageSize)
                                .Take(PageSize),
                        PagingInfo = new PagingInfo {
                            TotalItems = _repository.Products.Where(c => string.IsNullOrEmpty(category) || c.Category == category).Count(),
                            ItemsPerPage = PageSize,
                            CurrentPage = productPage
                        },
                        CurrentCategory = category
            });
    }
}