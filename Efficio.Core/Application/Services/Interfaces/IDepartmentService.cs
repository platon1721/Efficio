using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;

namespace Efficio.Core.Application.Services.Interfaces;

public interface IDepartmentService : IBaseService<DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto>
{
    Task<BaseResponse<DepartmentDto>> GetWithSubDepartmentsAsync(Guid id);
    Task<BaseResponse<DepartmentDto>> GetWithUsersAsync(Guid id);
    Task<BaseResponse<IEnumerable<DepartmentDto>>> GetByHeadIdAsync(Guid headId);
}