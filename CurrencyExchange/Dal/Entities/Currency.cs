namespace Dal.Entities
{
    public class Currency {

        public string Id { get; set; }
        public string Name { get; set; }


        public const int MaxIdLen = 3;

        public const int MaxNameLen = 100;
    }
}
