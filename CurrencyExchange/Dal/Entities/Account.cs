namespace Dal.Entities
{
    public class Account
    {
        public string CurrencyId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
