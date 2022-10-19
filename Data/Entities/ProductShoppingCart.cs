using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ProductShoppingCart
    {
        [Key]
        public int ProductShoppingCartId { get; set; }
        public int ShoppingCartId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
