﻿using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinesServices.Services
{

    public class CurrencyService
    {

        public CurrencyService(ExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IResult<CurrencyResponseDto>> CreateAsync(CreateCurrencyDto dto, CancellationToken cancellationToken)
        {

            var error = ValidateString("Id", dto.Id, Currency.MaxIdLen);
            if(error != null)
            {
                return Fail(error);
            }

            error = ValidateString("Name", dto.Name, Currency.MaxNameLen);
            if (error != null)
            {
                return Fail(error);
            }

            var currency = await _dbContext.Currencies.SingleOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);

            if (currency == null)
            {
                currency = new Currency
                {
                    Id = dto.Id!,
                    Name = dto.Name!,
                };
                await _dbContext.Currencies.AddAsync(currency, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return Success(currency);
        }

        public async Task<IResult<CurrencyResponseDto>> GetAsync(string? id, CancellationToken cancellationToken)
        {
            var error = ValidateString("Id", id, Currency.MaxIdLen);
            if (error != null)
            {
                return Fail(error);
            }

            var entity = await _dbContext.Currencies.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return Success(entity);
        }

        //todo: move to base class
        private ServiceError? ValidateString(string name, string? value, int maxLen)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                return Errors.EmptyString(name);
            }

            if(value.Length > maxLen)
            {
                return Errors.MaxLen(name, maxLen);
            }

            return null;
        }

        private IResult<CurrencyResponseDto> Fail(ServiceError error)
        {
            return Result.Fail<CurrencyResponseDto>(error);
        }

        private IResult<CurrencyResponseDto> Success(Currency? entity)
        {
            if(entity == null)
            {
                return Result.Success<CurrencyResponseDto>();
            }

            return Result.Success(new CurrencyResponseDto
            {
                Id = entity.Id,
                Name = entity.Name
            });
        }
        
        private readonly ExchangeDbContext _dbContext;
    }
}