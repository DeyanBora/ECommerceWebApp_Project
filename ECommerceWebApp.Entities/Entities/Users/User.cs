namespace ECommerceWebApp.Entities.Entities.Users
{
    using System.ComponentModel.DataAnnotations;

    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public Guid SecurityStamp { get; set; } = Guid.NewGuid();

        [Required]
        public bool IsEmailConfirmed { get; set; } = false;

        public bool IsPhoneNumberConfirmed { get; set; } = false;

        [MaxLength(256)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;


        //public ICollection<UserClaim>? Claims { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}

