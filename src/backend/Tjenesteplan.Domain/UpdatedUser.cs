namespace WebApi.Features.Users.UpdateUser
{
    public class UpdatedUser
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Username { get; }
        public string Password { get; }

        public UpdatedUser(
            int id,
            string firstName, 
            string lastName, 
            string username, 
            string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username.ToLower();
            Password = password;
        }
    }
}