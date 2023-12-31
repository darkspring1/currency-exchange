﻿using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinesServices.Services
{
    public class CurrencyService(ExchangeDbContext dbContext) : BaseBussinesService
    {
    public async Task<IResult<CurrencyResponseDto>> CreateAsync(CreateCurrencyDto dto,
        CancellationToken cancellationToken)
    {
        var badResponse = ValidateRequest(dto);
        if (badResponse != null)
        {
            return badResponse;
        }

        var currency =
            await dbContext.Currencies.SingleOrDefaultAsync(x => x.Id == dto.Id!.ToUpper(), cancellationToken);

        if (currency == null)
        {
            currency = new Currency
            {
                Id = dto.Id!,
                Name = dto.Name!,
            };
            await dbContext.Currencies.AddAsync(currency, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Success(currency);
    }

    public async Task<IResult<CurrencyResponseDto>> GetAsync(string? id, CancellationToken cancellationToken)
    {
        var error = ValidateString("Id", id, Currency.IdLen);
        if (error != null)
        {
            return Fail(error);
        }

        var entity = await dbContext.Currencies.SingleOrDefaultAsync(x => x.Id == id!.ToUpper(), cancellationToken);

        return Success(entity);
    }


    private IResult<CurrencyResponseDto>? ValidateRequest(CreateCurrencyDto dto)
    {
        var error = ValidateCurrencyId(nameof(dto.Id), dto.Id);
        if (error != null)
        {
            return Fail(error);
        }

        error = ValidateString(nameof(dto.Name), dto.Name, Currency.MaxNameLen);
        if (error != null)
        {
            return Fail(error);
        }

        return null;
    }
    

    private IResult<CurrencyResponseDto> Fail(ServiceError error)
    {
        return Result.Fail<CurrencyResponseDto>(error);
    }

    private IResult<CurrencyResponseDto> Success(Currency? entity)
    {
        if (entity == null)
        {
            return Result.Success<CurrencyResponseDto>();
        }

        return Result.Success(new CurrencyResponseDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
    }
}
