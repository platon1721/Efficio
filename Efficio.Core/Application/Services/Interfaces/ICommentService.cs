using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Domain.Entities.Enums;

namespace Efficio.Core.Application.Services.Interfaces;

public interface ICommentService : IBaseService<CommentDto, CreateCommentDto, UpdateCommentDto>
{
    Task<BaseResponse<IEnumerable<CommentDto>>> GetByAuthorAsync(Guid authorId);
    Task<BaseResponse<IEnumerable<CommentDto>>> GetByCommentableAsync(CommentableEntityType type, Guid commentableId);
}