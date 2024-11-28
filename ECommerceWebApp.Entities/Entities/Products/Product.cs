using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Entities.Entities.Products
{
    public class Product : BaseEntity
    {
        [Required]
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
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Url]
        [StringLength(100)]
        public required string ImageUri { get; set; }

        public string Slug { get; set; }
    }
}
