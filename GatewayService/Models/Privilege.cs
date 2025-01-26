namespace GatewayService.Models
{
    public class Privilege
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Privilege(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
