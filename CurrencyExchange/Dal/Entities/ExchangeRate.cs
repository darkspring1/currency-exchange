namespace Dal.Entities
{
    public class ExchangeRate {

        public string CurrencyFromId { get; set; }
        public string CurrencyToId { get; set; }
        public decimal Rate { get; set; }
    }
}
