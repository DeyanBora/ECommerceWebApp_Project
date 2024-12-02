namespace ECommerceWebApp.Entities.Entities.Products
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class Category : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? Slug { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
