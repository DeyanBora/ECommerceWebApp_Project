using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWebApp.Shared.DTOs.ElasticSearchDtos
{
    public class ElasticProductDto
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public string ErpCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } 
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; } 
    }

    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public int CreatedBy { get; set; }
    }
}
