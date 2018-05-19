namespace BoltJwt.Domain.Model.Aggregates
{
    public class UserRole
    {
        public User.User User { get; set; }
        public int UserId { get; set; }
        public Role.Role Role { get; set; }
        public int RoleId { get; set; }
    }
}