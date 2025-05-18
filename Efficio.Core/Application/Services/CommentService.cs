using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Entities.Enums;
using Efficio.Core.Domain.Interfaces;

namespace Efficio.Core.Application.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<CommentDto>> GetByIdAsync(Guid id)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null)
        {
            return BaseResponse<CommentDto>.FailResult($"Comment with ID {id} not found.");
        }

        var commentDto = _mapper.Map<CommentDto>(comment);
        return BaseResponse<CommentDto>.SuccessResult(commentDto);
    }

    public async Task<BaseResponse<IEnumerable<CommentDto>>> GetAllAsync()
    {
        var comments = await _unitOfWork.Comments.GetAllAsync();
        var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
        return BaseResponse<IEnumerable<CommentDto>>.SuccessResult(commentDtos);
    }

    public async Task<BaseResponse<CommentDto>> CreateAsync(CreateCommentDto createDto, Guid userId)
    {
        try
        {
            // Validate the commentable entity exists
            bool entityExists = false;
            
            switch (createDto.CommentableType)
            {
                case CommentableEntityType.Feedback:
                    var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(createDto.CommentableId);
                    entityExists = feedback != null;
                    break;
                case CommentableEntityType.Post:
                    var post = await _unitOfWork.Posts.GetByIdAsync(createDto.CommentableId);
                    entityExists = post != null;
                    break;
                default:
                    return BaseResponse<CommentDto>.FailResult($"Invalid commentable type: {createDto.CommentableType}");
            }

            if (!entityExists)
            {
                return BaseResponse<CommentDto>.FailResult($"Entity of type {createDto.CommentableType} with ID {createDto.CommentableId} not found.");
            }

            // Create comment
            var comment = _mapper.Map<Comment>(createDto);
            comment.MadeBy = userId;
            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.CompleteAsync();
            
            // Get created comment
            var createdComment = await _unitOfWork.Comments.GetByIdAsync(comment.Id);
            var commentDto = _mapper.Map<CommentDto>(createdComment);
            
            return BaseResponse<CommentDto>.SuccessResult(commentDto, "Comment created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<CommentDto>.FailResult($"Error creating comment: {ex.Message}");
        }
    }

    public async Task<BaseResponse<CommentDto>> UpdateAsync(Guid id, UpdateCommentDto updateDto, Guid userId)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null)
        {
            return BaseResponse<CommentDto>.FailResult($"Comment with ID {id} not found.");
        }

        // Check if user is the author
        if (comment.MadeBy != userId)
        {
            return BaseResponse<CommentDto>.FailResult("You do not have permission to update this comment.");
        }

        try
        {
            // Update comment
            _mapper.Map(updateDto, comment);
            await _unitOfWork.Comments.UpdateAsync(comment);
            await _unitOfWork.CompleteAsync();
            
            // Get updated comment
            var updatedComment = await _unitOfWork.Comments.GetByIdAsync(id);
            var commentDto = _mapper.Map<CommentDto>(updatedComment);
            
            return BaseResponse<CommentDto>.SuccessResult(commentDto, "Comment updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<CommentDto>.FailResult($"Error updating comment: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null)
        {
            return BaseResponse<bool>.FailResult($"Comment with ID {id} not found.");
        }

        // Check if user is the author
        if (comment.MadeBy != userId)
        {
            return BaseResponse<bool>.FailResult("You do not have permission to delete this comment.");
        }

        try
        {
            // Delete comment
            await _unitOfWork.Comments.RemoveAsync(comment);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "Comment deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error deleting comment: {ex.Message}");
        }
    }

    public async Task<BaseResponse<IEnumerable<CommentDto>>> GetByAuthorAsync(Guid authorId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(authorId);
        if (user == null)
        {
            return BaseResponse<IEnumerable<CommentDto>>.FailResult($"User with ID {authorId} not found.");
        }
        
        var comments = await _unitOfWork.Comments.GetByAuthorAsync(authorId);
        var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
        
        return BaseResponse<IEnumerable<CommentDto>>.SuccessResult(commentDtos);
    }

    public async Task<BaseResponse<IEnumerable<CommentDto>>> GetByCommentableAsync(CommentableEntityType type, Guid commentableId)
    {
        // Validate the commentable entity exists
        bool entityExists = false;
        
        switch (type)
        {
            case CommentableEntityType.Feedback:
                var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(commentableId);
                entityExists = feedback != null;
                break;
            case CommentableEntityType.Post:
                var post = await _unitOfWork.Posts.GetByIdAsync(commentableId);
                entityExists = post != null;
                break;
            default:
                return BaseResponse<IEnumerable<CommentDto>>.FailResult($"Invalid commentable type: {type}");
        }

        if (!entityExists)
        {
            return BaseResponse<IEnumerable<CommentDto>>.FailResult($"Entity of type {type} with ID {commentableId} not found.");
        }
        
        var comments = await _unitOfWork.Comments.GetByCommentableAsync(type, commentableId);
        var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
        
        return BaseResponse<IEnumerable<CommentDto>>.SuccessResult(commentDtos);
    }
}