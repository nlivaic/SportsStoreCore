using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models {
    public class Product {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a price.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please enter a category.")]
        public string Category { get; set; }
    }

}