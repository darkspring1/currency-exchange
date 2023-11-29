namespace Dal.Entities
{
    public class ExchangeHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FromCurrencyId { get; set; }
        public string ToCurrencyId { get; set; }
        public decimal Rate { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Fee { get; set; }
        public decimal FeeAmount { get; set; }
    }
}
