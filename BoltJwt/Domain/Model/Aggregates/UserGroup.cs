namespace BoltJwt.Domain.Model.Aggregates
{
    public class UserGroup
    {
        public User.User User { get; set; }
        public int UserId { get; set; }
        public Group.Group Group { get; set; }
        public int GroupId { get; set; }
    }
}