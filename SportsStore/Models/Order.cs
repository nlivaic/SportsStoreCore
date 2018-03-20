using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models {
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [BindNever]
        public bool Shipped { get; set; }
        [Required(ErrorMessage = "Name mandatory.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Line mandatory.")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        
        [Required(ErrorMessage = "City mandatory.")]
        public string City { get; set; }
        
        [Required(ErrorMessage = "State mandatory.")]
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        [Required(ErrorMessage = "Country mandatory.")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}