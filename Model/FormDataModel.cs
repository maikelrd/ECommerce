using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class FormDataModel
    {
        //public ProductModel Product { get; set; }
       // public ImageFile ImageFile { get; set; }
        // public ProductModel Product { get; set; }
        //public IFormFile file { get; set; }
        //public string name { get; set; }
       public IFormFile File { get; set; }
        //public double lastModified { get; set; }
        //public DateTime lastModifiedDate  { get; set; }
        public string Name { get; set; }
        //public int size { get; set; }

        //public string type { get; set; }
        //public string webkitRelativePath { get; set; }

    }
}
