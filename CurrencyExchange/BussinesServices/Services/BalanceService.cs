using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinesServices.Services
{

    public class BalanceService(ExchangeDbContext dbContext) : BaseBussinesService(dbContext)
    {

        public async Task<IResult<BalanceResponseDto>> GetAsync(BalanceRequestDto dto, CancellationToken cancellationToken)
        {
            var badResponse = ValidateRequest(dto);

            if (badResponse != null)
            {
                return badResponse;
            }

            var account = await DbContext.Accounts.SingleOrDefaultAsync(x => x.UserId == dto.UserId && x.CurrencyId == dto.CurrencyId, cancellationToken);

            return Success(account);

        }

        public async Task<IResult<BalanceResponseDto>> CreateOrUpdateAsync(BalanceRequestDto dto, CancellationToken cancellationToken)
        {
            var badResponse = ValidateRequest(dto);

            if(badResponse != null)
            {
                return badResponse;
            }

            var account = await LoadAccountAsync(dto, cancellationToken);

            if (account == null)
            {
                account = new Account { Balance = dto.Balance };
                await DbContext.Accounts.AddAsync(account, cancellationToken);
            }
            else
            {
                account.Balance = dto.Balance;
            }

            await DbContext.SaveChangesAsync(cancellationToken);

            return Success(account);
        }


        private IResult<BalanceResponseDto>? ValidateRequest(BalanceRequestDto dto)
        {
            var error = ValidatePositive("Balance", dto.Balance);

            if(error != null) { return Fail(error); }

            error = ValidateString("CurrencyId", dto.CurrencyId, Currency.MaxIdLen);

            if (error != null) { return Fail(error); }

            return null;
        }

        private Task<Account?> LoadAccountAsync(BalanceRequestDto dto, CancellationToken cancellation)
        {
            return DbContext.Accounts.SingleOrDefaultAsync(x => x.UserId == dto.UserId && x.CurrencyId == dto.CurrencyId, cancellation);
        }

        private IResult<BalanceResponseDto> Fail(ServiceError error) => Result.Fail<BalanceResponseDto>(error);

        private IResult<BalanceResponseDto> Success(Account? account)
        {
            if (account == null)
            {
                return Result.Success<BalanceResponseDto>();
            }

            return Result.Success(new BalanceResponseDto
            {
                Balance = account.Balance,
                CurrencyId = account.CurrencyId,
                UserId = account.UserId
            });
        }


    }
}
