using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
     
        //public int ShoppingCartItemId { get; set; }
        //public User User { get; set; }
        //public Product P1 { get; set; }
        //public int Amount1 { get; set; }
        //public Product P2 { get; set; }
        //public int Amount2 { get; set; }
        //public Product P3 { get; set; }
        //public int Amount3 { get; set; }
        //public Product P4 { get; set; }
        //public int Amount4 { get; set; }
        //public Product P5 { get; set; }
        //public int Amount5 { get; set; }
        //public int countActual { get; set; }
    }
}
