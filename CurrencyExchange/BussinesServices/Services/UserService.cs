using BussinesServices.Dto;
using BussinesServices.ServiceResult;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussinesServices.Services
{
    public class UserService(ExchangeDbContext dbContext)
    {
        public async Task<IResult<UserResponseDto>> CreateAsync(CreateUserRequestDto dto, CancellationToken cancellationToken)
        {
            var badResponse = ValidateRequest(dto);

            if (badResponse != null)
            {
                return badResponse;
            }
            
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
            
            if (user == null)
            {
                user = new User
                {
                    Id = dto.Id,
                    Name = dto.Name!,
                };
                await dbContext.Users.AddAsync(user, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            return Result.Success(new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name
            });
        }

        public async Task<IResult<UserResponseDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user == null)
            {
                return Result.Success<UserResponseDto>();
            }

            return Result.Success(new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name
            });
        }

        private IResult<UserResponseDto?> ValidateRequest(CreateUserRequestDto dto)
        {
            var error = ValidateString(nameof(dto.Name), dto.Name, User.MaxNameLen);
            if (error != null)
            {
                return Result.Fail<UserResponseDto>(error);
            }

            return null;
        }

        //todo: move to base class
        private ServiceError? ValidateString(string name, string? value, int maxLen)
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
