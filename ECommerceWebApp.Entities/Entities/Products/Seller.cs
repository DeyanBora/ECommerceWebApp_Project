namespace ECommerceWebApp.Entities.Entities.Products
{
    using System.ComponentModel.DataAnnotations;

    public class Seller : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(250)]
        public string? ContactInfo { get; set; }

        [StringLength(250)]
        public string? Website { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Seller()
        {
            Products = new HashSet<Product>();
        }
    }
}
