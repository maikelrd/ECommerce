using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        // public string PicByte { get; set; }
        public string Url { get; set; }
        public int ProductId { get; set; }
    }
}
