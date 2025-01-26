using System.Data;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public int RepoId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public Role[] Roles { get; set; }

        public User(int id, string username, string name, string lastName, string passwordHash, Role[] roles, int repoId)
        {
            Id = id;
            Username = username;
            Name = name;
            LastName = lastName;
            PasswordHash = passwordHash;
            Roles = roles;
            RepoId = repoId;
        }
    }
}
