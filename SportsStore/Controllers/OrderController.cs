using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers {
    public class OrderController : Controller {
        private IOrderRepository _repository;
        private Cart _cart;
        public OrderController(IOrderRepository repo, Cart cartService)
        {
            this._repository = repo;
            this._cart = cartService;
        }

        [Authorize]
        public ViewResult List() =>
            View(_repository.Orders.Where(o => !o.Shipped));
        
        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderId) {
            Order order = _repository.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null) {
                order.Shipped = true;
                _repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order) {
            if (_cart.Lines.Count() == 0) {
                ModelState.AddModelError("", "Your cart is empty.");
            }
            if (ModelState.IsValid) {
                order.Lines = _cart.Lines.ToArray();
                _repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            } else {
                return View();
            }
        }
        public IActionResult Completed()
        {
            _cart.Clear();
            return View();
        }
        
    }
}