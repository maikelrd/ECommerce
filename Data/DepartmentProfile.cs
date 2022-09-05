using AutoMapper;
using ECommerce.Data.Entities;
using ECommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile()
        {
            this.CreateMap<Department, DepartmentModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();

            //revisar este
            CreateMap<User, LoginModel>().ReverseMap();
        }
    }
}
