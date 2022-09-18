using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class UserDTO
    {      
        
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
        public bool IsAuthenticated { get; set; }
        


        public UserDTO(string userName, string eMail)        {
         
            
            UserName = userName;
            Email = eMail;
            IsAuthenticated = false;


        }
    }
}
