﻿using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Extensions.Logging;
using ExchangeResult = BussinesServices.ServiceResult.IResult<BussinesServices.Dto.ExchangeResponseDto>;

namespace BussinesServices.Services
{

    public class ExchangeService(ExchangeDbContext dbContext, ILogger<ExchangeService> logger)
    {
        public async Task<ExchangeResult> ExchangeAsync(ExchangeRequestDto dto, CancellationToken cancellationToken)
        {
            return ValidateRequest(dto) ?? await RunExchangeTxWithRetryAsync(dto, cancellationToken);
        }

        private async Task<ExchangeResult> RunExchangeTxWithRetryAsync(ExchangeRequestDto dto, CancellationToken cancellationToken)
        {
            const int maxRetry = 3;
            
            for (var i = 1; i <= maxRetry; i++)
            {
                try
                {
                    return await RunExchangeTxAsync(dto, cancellationToken);
                }
                catch (Exception e)
                {
                    logger.LogDebug(e, $"Exchange tx failed. Attempt:{i+1}");
                    if (i == maxRetry)
                    {
                        throw;
                    }
                }
            }

            return Result.Fail<ExchangeResponseDto>(Errors.IntrenalServerError());
        }

        private async Task<ExchangeResult> RunExchangeTxAsync(ExchangeRequestDto dto, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken);
            try
            {
                var history = await dbContext.ExchangeHistory.SingleOrDefaultAsync(x => x.Id == dto.IdempotencyKey, cancellationToken);

                if (history == null)
                {
                    var accounts = await GetAccountsAsync(dto, cancellationToken);

                    if (accounts.from.Balance < dto.Amount)
                    {
                        await transaction.CommitAsync(cancellationToken);
                        return Result.Fail<ExchangeResponseDto>(Errors.SmallBalance(accounts.from.Balance, dto.From, dto.Amount));
                    }

                    var toAmount = dto.Amount * dto.Rate;
                    var feeAmount = toAmount * dto.Fee;

                    accounts.from.Balance -= dto.Amount;
                    accounts.to.Balance += toAmount - feeAmount;

                    history = new ExchangeHistory
                    {
                        Id = dto.IdempotencyKey,
                        UserId = dto.UserId,
                        FromCurrencyId = dto.From,
                        ToCurrencyId = dto.To,
                        FromAmount = dto.Amount,
                        ToAmount = toAmount,
                        Rate = dto.Rate,
                        Fee = dto.Fee,
                        FeeAmount = feeAmount

                    };
                    await dbContext.ExchangeHistory.AddAsync(history, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }

                return CreateSuccessResult(history);
            }
            catch (Exception)
            {
                dbContext.ChangeTracker.Clear();
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private ExchangeResult? ValidateRequest(ExchangeRequestDto dto)
        {
            var error = ValidateDecimal("Amount", dto.Amount);

            if (error != null)
            {
                return Result.Fail<ExchangeResponseDto>(error);
            }

            error = ValidateDecimal("Rate", dto.Rate);
            if (error != null)
            {
                return Result.Fail<ExchangeResponseDto>(error);
            }

            return null;
        }

        private static ServiceError? ValidateDecimal(string name, decimal value)
        {
            return value <= 0 ? Errors.PositiveValue(name) : null;
        }

        private ExchangeResult CreateSuccessResult(ExchangeHistory history)
        {
            return Result.Success(new ExchangeResponseDto
            {
                IdempotencyKey = history.Id,
                Fee = history.Fee,
                FeeAmount = history.FeeAmount,
                From = history.FromCurrencyId,
                To = history.ToCurrencyId,
                FromAmount = history.FromAmount,
                ToAmount = history.ToAmount,
                UserId = history.UserId,
                Rate = history.Rate
            });
        }

        private async Task<(Account from, Account to)> GetAccountsAsync(ExchangeRequestDto dto, CancellationToken cancellationToken)
        {
            var accounts = await dbContext
                .Accounts
                .Where(x => x.UserId == dto.UserId && (x.CurrencyId == dto.From || x.CurrencyId == dto.To))
                .ToArrayAsync(cancellationToken);


            var fromAcc = accounts.SingleOrDefault(x => x.CurrencyId == dto.From);
            var toAcc = accounts.SingleOrDefault(x => x.CurrencyId == dto.To);

            return (
                from: fromAcc ?? await CreateAccountAsync(dto.UserId, dto.From, cancellationToken),
                to: toAcc ?? await CreateAccountAsync(dto.UserId, dto.To, cancellationToken)
            );
        }

        private async Task<Account> CreateAccountAsync(Guid userId, string currencyId, CancellationToken cancellationToken)
        {

            var acc = new Account
            {
                Balance = 0,
                CurrencyId = currencyId,
                UserId = userId,
            };

            await dbContext.Accounts.AddAsync(acc, cancellationToken);

            return acc;
        }
    }
}
