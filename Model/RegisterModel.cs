using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string  LastName { get; set; }

        public RegisterModel(string _firstName, string _lastName)
        {
            FirstName = _firstName;
            LastName = _lastName;
        }
    }
}
