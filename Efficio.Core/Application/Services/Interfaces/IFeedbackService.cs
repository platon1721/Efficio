using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;

namespace Efficio.Core.Application.Services.Interfaces;

public interface IFeedbackService : IBaseService<FeedbackDto, CreateFeedbackDto, UpdateFeedbackDto>
{
    Task<BaseResponse<FeedbackDto>> GetWithCommentsAsync(Guid id);
    Task<BaseResponse<FeedbackDto>> GetWithTagsAsync(Guid id);
    Task<BaseResponse<IEnumerable<FeedbackDto>>> GetByAuthorAsync(Guid authorId);
    Task<BaseResponse<IEnumerable<FeedbackDto>>> GetByTagAsync(Guid tagId);
}