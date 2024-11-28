namespace ECommerceWebApp.Entities.Entities.Users
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }


        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new HashSet<User>();
        }
    }
}
