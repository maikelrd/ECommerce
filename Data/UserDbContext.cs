using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class UserDbContext: IdentityDbContext<UsersEcommerce, IdentityRole, string>
    {
        public UserDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<UserDbContext> options) : base(options)
        {

        }
    }
}
