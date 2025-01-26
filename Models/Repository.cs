namespace Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DefaultUserDiskSpace { get; set; }
        public string Label { get; set; }

        public Repository(int id, string name, int defaultUserDiskSpace, string label)
        {
            Id = id;
            Name = name;
            DefaultUserDiskSpace = defaultUserDiskSpace;
            Label = label;
        }
    }
}
