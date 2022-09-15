using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class ShoppingCartModel
    {
        public int ShoppingCartItemId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public string UserEmail { get; set; }
    }
}
