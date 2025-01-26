namespace GatewayService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Privilege[] Privileges { get; set; }

        public Role(int id, string name, Privilege[] privileges)
        {
            Id = id;
            Name = name;
            Privileges = privileges;
        }
    }
}
