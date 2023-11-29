namespace Dal.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public const int MaxNameLen = 500;
    }
}
