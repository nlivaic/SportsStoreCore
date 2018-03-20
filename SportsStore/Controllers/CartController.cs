using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Infrastructure;
using System;
using System.Linq;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers {
    public class CartController : Controller
    {
        private IProductRepository _repository;
        private Cart _cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            _repository = repo;
            _cart = cartService;
        }

        public IActionResult Index(string returnUrl) {
            return View(new CartIndexViewModel {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl) {
            Product product = _repository.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (product != null) {
                _cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl) {
            Product product = _repository.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (product != null) {
                _cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}