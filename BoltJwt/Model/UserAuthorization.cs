namespace BoltJwt.Model
{
    public class UserAuthorization : Entity
    {
        public string AuthorizationName { get; set; }

        public int UserId { get; set; }
    }
}