using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Entities.Entities.Products
{
    public class Brand : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(250)]
        public string? LogoUrl { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Slug { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public Brand()
        {
            Products = new HashSet<Product>();
        }
    }
}
