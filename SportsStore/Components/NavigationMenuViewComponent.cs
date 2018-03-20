using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components {
    public class NavigationMenuViewComponent : ViewComponent {
        private IProductRepository _repository;

        public NavigationMenuViewComponent(IProductRepository repo) {
            this._repository = repo;
        }

        public IViewComponentResult Invoke() {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(
                _repository.Products.Select(p => p.Category).Distinct().OrderBy(x => x)
            );
        }

    }
}