using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWebApp.Entities.Entities.Products
{

    public class Product : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public required string ErpCode { get; set; }

        [Required]
        [StringLength(50)]
        public required string Title { get; set; }

        [Required]
        [StringLength(100)]
        public required string Description { get; set; }

        [Required]
        public required int CategoryId { get; set; }

        [Required]
        public required int BrandId { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Url]
        [StringLength(100)]
        public required string ImageUri { get; set; }

        [MaxLength(100)]
        public string? Slug { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual Seller Seller { get; set; } = null!;
    }
}
