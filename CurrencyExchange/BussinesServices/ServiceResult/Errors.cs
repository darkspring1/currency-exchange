
namespace BussinesServices.ServiceResult
{
    public static class Errors
    {
        public static ServiceError PositiveValue(string name)
            => new (1, $"{name} value must be > 0");

        public static ServiceError SmallBalance(decimal userBalance, string currency, decimal amount)
            => new (2, $"User balance too small. UserBalance:{userBalance}{currency} Amount:{amount}");

        public static ServiceError MaxLen(string name, int maxLen)
            => new (3, $"{name} lenght must be <= {maxLen}.");

        public static ServiceError NotNull(string name)
            => new (4, $"{name} must not be null");

        public static ServiceError EmptyString(string name)
           => new (5, $"{name} must not be null empty");
        
        public static ServiceError Len(string name, int len) => new(6, $"{name} lenght must be == {len}.");
        
        public static ServiceError IntrenalServerError() => new(7, $"Internal Server Error");

    }
}
