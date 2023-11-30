
namespace BussinesServices.ServiceResult
{
    static class Errors
    {
        public static ServiceError PositiveValue(string name)
            => new ServiceError(1, $"{name} value must be > 0");

        public static ServiceError SmallBalance(decimal userBalance, string currency, decimal amount)
            => new ServiceError(2, $"User balance too small. UserBalance:{userBalance}{currency} Amount:{amount}");

        public static ServiceError MaxLen(string name, int maxLen)
            => new ServiceError(3, $"{name} lenght must be <= {maxLen}.");

        public static ServiceError NotNull(string name)
            => new ServiceError(4, $"{name} must not be null");

        public static ServiceError EmptyString(string name)
           => new ServiceError(5, $"{name} must not be null empty");

    }
}
