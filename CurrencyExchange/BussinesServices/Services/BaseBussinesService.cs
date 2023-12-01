using BussinesServices.ServiceResult;
using Dal.Entities;

namespace BussinesServices.Services
{
    public class BaseBussinesService()
    {
        
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
        
        protected ServiceError? ValidateMaxLen(string name, string? value, int maxLen)
        {
            if (value.Length > maxLen)
            {
                return Errors.MaxLen(name, maxLen);
            }

            return null;
        }
        
        protected ServiceError? ValidateString(string name, string? value, int maxLen)
        {
            var error = ValidateEmptyString(name, value);
            if (error != null)
            {
                return error;
            }

            error = ValidateMaxLen(name, value, maxLen);
            return error ?? null;
        }
        
        protected ServiceError? ValidateCurrencyId(string name, string? value)
        {
            var error = ValidateEmptyString(name, value);
            if (error != null)
            {
                return error;
            }
            
            return value!.Length != Currency.IdLen ? Errors.Len(name, Currency.IdLen) : null;
        }
    }
}
