namespace Dal.Entities
{
    public class Account
    {
        private string _currencyId;
        public string CurrencyId {
            get { return _currencyId; }
            set { _currencyId = value.ToUpper(); }
        }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
