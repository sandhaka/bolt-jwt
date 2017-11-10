namespace BoltJwt.Domain.Model
{
    public class GroupRole
    {
        public Group Group { get; set; }
        public int GroupId { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}