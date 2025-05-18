using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Interfaces;

namespace Efficio.Core.Application.Services;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TagService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Efficio.Core/Application/Services/TagService.cs (jätkub)
    public async Task<BaseResponse<TagDto>> GetByIdAsync(Guid id)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null)
        {
            return BaseResponse<TagDto>.FailResult($"Tag with ID {id} not found.");
        }

        var tagDto = _mapper.Map<TagDto>(tag);
        return BaseResponse<TagDto>.SuccessResult(tagDto);
    }

    public async Task<BaseResponse<IEnumerable<TagDto>>> GetAllAsync()
    {
        var tags = await _unitOfWork.Tags.GetAllAsync();
        var tagDtos = _mapper.Map<IEnumerable<TagDto>>(tags);
        return BaseResponse<IEnumerable<TagDto>>.SuccessResult(tagDtos);
    }

    public async Task<BaseResponse<TagDto>> CreateAsync(CreateTagDto createDto, Guid userId)
    {
        try
        {
            // Check if tag with the same title already exists
            var existingTag = await _unitOfWork.Tags.GetByTitleAsync(createDto.Title);
            if (existingTag != null)
            {
                return BaseResponse<TagDto>.FailResult($"Tag with title '{createDto.Title}' already exists.");
            }

            // Create tag
            var tag = _mapper.Map<Tag>(createDto);
            tag.MadeBy = userId;
            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.CompleteAsync();
            
            // Get created tag
            var createdTag = await _unitOfWork.Tags.GetByIdAsync(tag.Id);
            var tagDto = _mapper.Map<TagDto>(createdTag);
            
            return BaseResponse<TagDto>.SuccessResult(tagDto, "Tag created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<TagDto>.FailResult($"Error creating tag: {ex.Message}");
        }
    }

    public async Task<BaseResponse<TagDto>> UpdateAsync(Guid id, UpdateTagDto updateDto, Guid userId)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null)
        {
            return BaseResponse<TagDto>.FailResult($"Tag with ID {id} not found.");
        }

        // Check if user is the author
        if (tag.MadeBy != userId)
        {
            return BaseResponse<TagDto>.FailResult("You do not have permission to update this tag.");
        }

        try
        {
            // Check title uniqueness if changing title
            if (!string.IsNullOrEmpty(updateDto.Title) && updateDto.Title != tag.Title)
            {
                var existingTag = await _unitOfWork.Tags.GetByTitleAsync(updateDto.Title);
                if (existingTag != null)
                {
                    return BaseResponse<TagDto>.FailResult($"Tag with title '{updateDto.Title}' already exists.");
                }
            }

            // Update tag
            _mapper.Map(updateDto, tag);
            await _unitOfWork.Tags.UpdateAsync(tag);
            await _unitOfWork.CompleteAsync();
            
            // Get updated tag
            var updatedTag = await _unitOfWork.Tags.GetByIdAsync(id);
            var tagDto = _mapper.Map<TagDto>(updatedTag);
            
            return BaseResponse<TagDto>.SuccessResult(tagDto, "Tag updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<TagDto>.FailResult($"Error updating tag: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null)
        {
            return BaseResponse<bool>.FailResult($"Tag with ID {id} not found.");
        }

        // Check if user is the author or has admin rights
        if (tag.MadeBy != userId)
        {
            // Add admin check logic if needed
            return BaseResponse<bool>.FailResult("You do not have permission to delete this tag.");
        }

        try
        {
            // Check if tag is used in any feedbacks or posts
            var taggedFeedbacks = await _unitOfWork.Feedbacks.GetByTagAsync(id);
            if (taggedFeedbacks.Any())
            {
                return BaseResponse<bool>.FailResult("Cannot delete tag as it is used in feedbacks. Remove the tag from all feedbacks first.");
            }

            // Delete tag
            await _unitOfWork.Tags.RemoveAsync(tag);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "Tag deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error deleting tag: {ex.Message}");
        }
    }

    public async Task<BaseResponse<IEnumerable<TagDto>>> GetByFeedbackAsync(Guid feedbackId)
    {
        var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(feedbackId);
        if (feedback == null)
        {
            return BaseResponse<IEnumerable<TagDto>>.FailResult($"Feedback with ID {feedbackId} not found.");
        }
        
        var tags = await _unitOfWork.Tags.GetByFeedbackAsync(feedbackId);
        var tagDtos = _mapper.Map<IEnumerable<TagDto>>(tags);
        
        return BaseResponse<IEnumerable<TagDto>>.SuccessResult(tagDtos);
    }

    public async Task<BaseResponse<IEnumerable<TagDto>>> GetByPostAsync(Guid postId)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(postId);
        if (post == null)
        {
            return BaseResponse<IEnumerable<TagDto>>.FailResult($"Post with ID {postId} not found.");
        }
        
        var tags = await _unitOfWork.Tags.GetByPostAsync(postId);
        var tagDtos = _mapper.Map<IEnumerable<TagDto>>(tags);
        
        return BaseResponse<IEnumerable<TagDto>>.SuccessResult(tagDtos);
    }

    public async Task<BaseResponse<TagDto>> GetByTitleAsync(string title)
    {
        var tag = await _unitOfWork.Tags.GetByTitleAsync(title);
        if (tag == null)
        {
            return BaseResponse<TagDto>.FailResult($"Tag with title '{title}' not found.");
        }

        var tagDto = _mapper.Map<TagDto>(tag);
        return BaseResponse<TagDto>.SuccessResult(tagDto);
    }
}
    