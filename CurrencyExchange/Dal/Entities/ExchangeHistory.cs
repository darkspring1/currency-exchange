namespace Dal.Entities
{
    public class ExchangeHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CurrencyFromId { get; set; }
        public string CurrencyToId { get; set; }
        public decimal Rate { get; set; }
        public decimal amount { get; set; }
    }
}
