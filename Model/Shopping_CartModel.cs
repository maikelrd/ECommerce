using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class Shopping_CartModel
    {
        public int ShoppingCartId { get; set; }
        //public User User { get; set; }
        //public int UserId { get; set; }
        public string UserEmail { get; set; }
        // public int UserId { get; set; }
         public List<ProductInShoppingCart> ProductsInShoppingCart { get; set; }
       // public ProductInShoppingCart[] ProductsInShoppingCart { get; set; }
    }
}
