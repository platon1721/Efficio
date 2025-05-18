using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;

namespace Efficio.Core.Application.Services.Interfaces;

public interface IPostService : IBaseService<PostDto, CreatePostDto, UpdatePostDto>
{
    Task<BaseResponse<PostDto>> GetWithCommentsAsync(Guid id);
    Task<BaseResponse<PostDto>> GetWithTagsAsync(Guid id);
    Task<BaseResponse<PostDto>> GetWithDepartmentsAsync(Guid id);
    Task<BaseResponse<IEnumerable<PostDto>>> GetByDepartmentAsync(Guid departmentId);
    Task<BaseResponse<IEnumerable<PostDto>>> GetByAuthorAsync(Guid authorId);
}