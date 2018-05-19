namespace BoltJwt.Domain.Model.Aggregates
{
    public class GroupRole
    {
        public Group.Group Group { get; set; }
        public int GroupId { get; set; }
        public Role.Role Role { get; set; }
        public int RoleId { get; set; }
    }
}