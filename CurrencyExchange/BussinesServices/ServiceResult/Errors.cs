
namespace BussinesServices.ServiceResult
{
    static class Errors
    {
        public static ServiceError InvalidRate(decimal rate)
            => new ServiceError(1, $"Invalid Rate value: {rate}");

        public static ServiceError InvalidAmount(decimal amount)
            => new ServiceError(2, $"Invalid Amount value: {amount}");

        public static ServiceError SmallBalance(decimal userBalance, string currency, decimal amount)
            => new ServiceError(3, $"User balance too small. UserBalance:{userBalance}{currency} Amount:{amount}");
    }
}
