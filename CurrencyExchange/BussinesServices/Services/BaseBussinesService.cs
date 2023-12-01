using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;

namespace BussinesServices.Services
{
    public class BaseBussinesService(ExchangeDbContext dbContext)
    {
        protected ExchangeDbContext DbContext { get; private set; } = dbContext;

        protected ServiceError? ValidatePositive(string name, decimal value)
        {
            if (value <= 0)
            {
                return Errors.PositiveValue(name);
            }

            return null;
        }

        protected ServiceError? ValidateEmptyString(string name, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                return Errors.EmptyString(name);
            }

            return null;
        }
        
        protected ServiceError? ValidateCurrencyIdLen(string name, string value)
        {
            return value.Length != Currency.IdLen ? Errors.Len(name, Currency.IdLen) : null;
        }
    }
}
