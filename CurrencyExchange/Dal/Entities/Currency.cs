namespace Dal.Entities
{
    public class Currency {

        private string _id;

        public string Id
        {
            get => _id;
            set => _id = value.ToUpper();
        }
        public string Name { get; set; }
        
        public const int IdLen = 3;

        public const int MaxNameLen = 100;
    }
}
