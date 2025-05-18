using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;

namespace Efficio.Core.Application.Services.Interfaces;

public interface ITagService : IBaseService<TagDto, CreateTagDto, UpdateTagDto>
{
    Task<BaseResponse<IEnumerable<TagDto>>> GetByFeedbackAsync(Guid feedbackId);
    Task<BaseResponse<IEnumerable<TagDto>>> GetByPostAsync(Guid postId);
    Task<BaseResponse<TagDto>> GetByTitleAsync(string title);
}