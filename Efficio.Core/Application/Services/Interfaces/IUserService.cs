using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;

namespace Efficio.Core.Application.Services.Interfaces;

public interface IUserService : IBaseService<UserDto, CreateUserDto, UpdateUserDto>
{
    Task<BaseResponse<UserDto>> GetUserWithDepartmentsAsync(Guid id);
    Task<BaseResponse<IEnumerable<UserDto>>> GetUsersByDepartmentAsync(Guid departmentId);
    Task<BaseResponse<UserDto>> GetUserByEmailAsync(string email);
}