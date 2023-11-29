﻿using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinesServices
{
    public class UserService
    {

        public UserService(ExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IResult<UserResponseDto>> CreateAsync(CreateUserRequestDto dto)
        {

            if (dto.Name.Length > User.MaxNameLen)
            {
                return Result.Fail<UserResponseDto>(Errors.MaxUserNameLen(User.MaxNameLen));
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == dto.Id);

           
            if(user == null)
            {
                user = new User
                {
                    Id = dto.Id,
                    Name = dto.Name,
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }


            return Result.Success(new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name
            });
        }

        public async Task<IResult<UserResponseDto>> GetAsync(Guid id)
        { 
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);

            if(user == null)
            {
                return Result.Success<UserResponseDto>();
            }

            return Result.Success(new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name
            });
        }

        private readonly ExchangeDbContext _dbContext;
    }
}
