using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinesServices.Services
{

    public class BalanceService(ExchangeDbContext dbContext) : BaseBussinesService()
    {

        public async Task<IResult<BalanceResponseDto>> GetAsync(BalanceRequestDto dto, CancellationToken cancellationToken)
        {
            var badResponse = ValidateRequest(dto);

            if (badResponse != null)
            {
                return badResponse;
            }

            var account = await LoadAccountAsync(dto, cancellationToken);

            return Success(account);

        }

        public async Task<IResult<BalanceResponseDto>> CreateOrUpdateAsync(CreateBalanceRequestDto dto, CancellationToken cancellationToken)
        {
            var badResponse = ValidateCreateRequest(dto);

            if(badResponse != null)
            {
                return badResponse;
            }

            var account = await LoadAccountAsync(dto, cancellationToken);

            if (account == null)
            {
                account = new Account { CurrencyId = dto.CurrencyId!, UserId = dto.UserId, Balance = dto.Balance };
                await dbContext.Accounts.AddAsync(account, cancellationToken);
            }
            else
            {
                account.Balance = dto.Balance;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Success(account);
        }


        private IResult<BalanceResponseDto>? ValidateCreateRequest(CreateBalanceRequestDto dto)
        {
            var error = ValidatePositive("Balance", dto.Balance);

            if (error != null) { return Fail(error); }

            return ValidateRequest(dto);
        }

        private IResult<BalanceResponseDto>? ValidateRequest(BalanceRequestDto dto)
        {
            var error = ValidateEmptyString("CurrencyId", dto.CurrencyId);
            if (error != null) { return Fail(error); }

            error = ValidateCurrencyIdLen(nameof(dto.CurrencyId), dto.CurrencyId);
            return error != null ? Fail(error) : null;
        }

        private Task<Account?> LoadAccountAsync(BalanceRequestDto dto, CancellationToken cancellation)
        {
            return dbContext.Accounts.SingleOrDefaultAsync(x => x.UserId == dto.UserId && x.CurrencyId == dto.CurrencyId!.ToUpper(), cancellation);
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
