using BussinesServices.ServiceResult;
using Dal;

namespace BussinesServices.Services
{
    public class BaseBussinesService(ExchangeDbContext dbContext)
    {
        protected ExchangeDbContext DbContext { get; private set; } = dbContext;

        protected private ServiceError? ValidatePositive(string name, decimal value)
        {
            if (value <= 0)
            {
                return Errors.PositiveValue(name);
            }

            return null;
        }

        protected ServiceError? ValidateString(string name, string? value, int maxLen)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                return Errors.EmptyString(name);
            }

            if (value.Length > maxLen)
            {
                return Errors.MaxLen(name, maxLen);
            }

            return null;
        }
    }
}
