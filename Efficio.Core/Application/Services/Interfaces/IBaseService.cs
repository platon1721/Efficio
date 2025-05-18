using Efficio.Core.Application.DTOs.Base;

namespace Efficio.Core.Application.Services.Interfaces;

public interface IBaseService<TDto, TCreateDto, TUpdateDto>
{
    Task<BaseResponse<TDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync();
    Task<BaseResponse<TDto>> CreateAsync(TCreateDto createDto, Guid userId);
    Task<BaseResponse<TDto>> UpdateAsync(Guid id, TUpdateDto updateDto, Guid userId);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId);
}