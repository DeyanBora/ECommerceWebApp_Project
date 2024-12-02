namespace ECommerceWebApp.Entities.Entities.Products
{
    using System.ComponentModel.DataAnnotations;

    public class Manufacturer : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(250)]
        public string? ContactInfo { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? Slug { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public Manufacturer()
        {
            Products = new HashSet<Product>();
        }
    }
}
