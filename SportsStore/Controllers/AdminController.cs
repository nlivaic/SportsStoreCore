using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers {
    
    [Authorize]
    public class AdminController : Controller {
        IProductRepository _repository;
        
        public AdminController(IProductRepository repo) {
            this._repository = repo;
        }

        public IActionResult Index() => View(_repository.Products);

        public IActionResult Edit(int productId) {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null) {
                return View(product);
            } else {
                return View(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid) {
                _repository.SaveProduct(product);
                TempData["message"] = string.Format($"Product {product.Name} updated.");
                return RedirectToAction(nameof(Index));
            } else {
                return View(product);
            }
        }

        public ViewResult Create() => View(nameof(Edit), new Product());

        [HttpPost]
       public RedirectToActionResult Delete(int productId) {
           Product product = _repository.DeleteProduct(productId);
           if (product != null) {
            TempData["message"] = $"Product {product.Name} deleted.";
           }
           return RedirectToAction(nameof(Index));
       } 
    }
}