using System.ComponentModel.DataAnnotations;

namespace ECommerceWebApp.Entities.Entities.Users
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }


        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();

        public Role()
        {
            Users = new HashSet<User>();
        }
    }
}
