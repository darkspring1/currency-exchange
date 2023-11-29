
namespace BussinesServices.ServiceResult
{
    static class Errors
    {
        public static ServiceError RateNotFound(string currencyFrom, string currencyTo)
            => new ServiceError(1, $"Rate not found. {currencyFrom}:{currencyTo}");

        public static ServiceError InvalidAmount(decimal amount)
            => new ServiceError(2, $"Invalid amount value: {amount}");

        public static ServiceError SmallBalance(decimal userBalance, string currency, decimal amount)
            => new ServiceError(3, $"User balance too small. UserBalance:{userBalance}{currency} Amount:{amount}");
    }
}
