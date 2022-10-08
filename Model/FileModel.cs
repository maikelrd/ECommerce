using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class FileModel
    {
        public IFormFile[] ImageFile { get; set; }
        // public IFormFile Product { get; set; }
        public string Product { get; set; }
    }
}
