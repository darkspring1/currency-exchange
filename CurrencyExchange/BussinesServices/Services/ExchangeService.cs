﻿using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ExchangeResult = BussinesServices.ServiceResult.IResult<BussinesServices.Dto.ExchangeResponseDto>;

namespace BussinesServices.Services
{

    public class ExchangeService(ExchangeDbContext dbContext)
    {
        public async Task<ExchangeResult> ExchangeAsync(ExchangeRequestDto dto, CancellationToken cancellationToken)
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
                    dbContext.ExchangeHistory.Add(history);
                    await transaction.CommitAsync(cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                return CreateSuccessResult(history);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
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
