using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Interfaces;

namespace Efficio.Core.Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<FeedbackDto>> GetByIdAsync(Guid id)
    {
        var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
        if (feedback == null)
        {
            return BaseResponse<FeedbackDto>.FailResult($"Feedback with ID {id} not found.");
        }

        var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
        return BaseResponse<FeedbackDto>.SuccessResult(feedbackDto);
    }

    public async Task<BaseResponse<IEnumerable<FeedbackDto>>> GetAllAsync()
    {
        var feedbacks = await _unitOfWork.Feedbacks.GetAllAsync();
        var feedbackDtos = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        return BaseResponse<IEnumerable<FeedbackDto>>.SuccessResult(feedbackDtos);
    }

    public async Task<BaseResponse<FeedbackDto>> CreateAsync(CreateFeedbackDto createDto, Guid userId)
    {
        try
        {
            // Create feedback
            var feedback = _mapper.Map<Feedback>(createDto);
            feedback.MadeBy = userId;
            await _unitOfWork.Feedbacks.AddAsync(feedback);

            // Add tags if provided
            if (createDto.TagIds.Any())
            {
                foreach (var tagId in createDto.TagIds)
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        var feedbackTag = new FeedbackTag
                        {
                            FeedbackId = feedback.Id,
                            TagId = tagId
                        };
                        feedback.FeedbackTags.Add(feedbackTag);
                    }
                }
            }

            await _unitOfWork.CompleteAsync();
            
            // Get created feedback with relationships
            var createdFeedback = await _unitOfWork.Feedbacks.GetWithTagsAsync(feedback.Id);
            var feedbackDto = _mapper.Map<FeedbackDto>(createdFeedback);
            
            return BaseResponse<FeedbackDto>.SuccessResult(feedbackDto, "Feedback created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<FeedbackDto>.FailResult($"Error creating feedback: {ex.Message}");
        }
    }

    public async Task<BaseResponse<FeedbackDto>> UpdateAsync(Guid id, UpdateFeedbackDto updateDto, Guid userId)
    {
        var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
        if (feedback == null)
        {
            return BaseResponse<FeedbackDto>.FailResult($"Feedback with ID {id} not found.");
        }

        // Check if user is the author
        if (feedback.MadeBy != userId)
        {
            return BaseResponse<FeedbackDto>.FailResult("You do not have permission to update this feedback.");
        }

        try
        {
            // Update feedback properties
            _mapper.Map(updateDto, feedback);
            await _unitOfWork.Feedbacks.UpdateAsync(feedback);

            // Update tags if provided
            if (updateDto.TagIds != null)
            {
                // Get current feedback with tags
                var feedbackWithTags = await _unitOfWork.Feedbacks.GetWithTagsAsync(id);
                var currentTagIds = feedbackWithTags.FeedbackTags.Select(ft => ft.TagId).ToList();
                
                // Tags to remove
                var tagIdsToRemove = currentTagIds.Except(updateDto.TagIds).ToList();
                foreach (var tagId in tagIdsToRemove)
                {
                    var feedbackTag = feedbackWithTags.FeedbackTags.FirstOrDefault(ft => ft.TagId == tagId);
                    if (feedbackTag != null)
                    {
                        feedbackWithTags.FeedbackTags.Remove(feedbackTag);
                    }
                }
                
                // Tags to add
                var tagIdsToAdd = updateDto.TagIds.Except(currentTagIds).ToList();
                foreach (var tagId in tagIdsToAdd)
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        feedbackWithTags.FeedbackTags.Add(new FeedbackTag
                        {
                            FeedbackId = id,
                            TagId = tagId
                        });
                    }
                }
            }

            await _unitOfWork.CompleteAsync();
            
            // Get updated feedback
            var updatedFeedback = await _unitOfWork.Feedbacks.GetWithTagsAsync(id);
            var feedbackDto = _mapper.Map<FeedbackDto>(updatedFeedback);
            
            return BaseResponse<FeedbackDto>.SuccessResult(feedbackDto, "Feedback updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<FeedbackDto>.FailResult($"Error updating feedback: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
        if (feedback == null)
        {
            return BaseResponse<bool>.FailResult($"Feedback with ID {id} not found.");
        }

        // Check if user is the author
        if (feedback.MadeBy != userId)
        {
            return BaseResponse<bool>.FailResult("You do not have permission to delete this feedback.");
        }

        try
        {
            // Delete feedback
            await _unitOfWork.Feedbacks.RemoveAsync(feedback);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "Feedback deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error deleting feedback: {ex.Message}");
        }
    }

    public async Task<BaseResponse<FeedbackDto>> GetWithCommentsAsync(Guid id)
    {
        var feedback = await _unitOfWork.Feedbacks.GetWithCommentsAsync(id);
        if (feedback == null)
        {
            return BaseResponse<FeedbackDto>.FailResult($"Feedback with ID {id} not found.");
        }

        var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
        return BaseResponse<FeedbackDto>.SuccessResult(feedbackDto);
    }

    public async Task<BaseResponse<FeedbackDto>> GetWithTagsAsync(Guid id)
    {
        var feedback = await _unitOfWork.Feedbacks.GetWithTagsAsync(id);
        if (feedback == null)
        {
            return BaseResponse<FeedbackDto>.FailResult($"Feedback with ID {id} not found.");
        }

        var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
        return BaseResponse<FeedbackDto>.SuccessResult(feedbackDto);
    }

    public async Task<BaseResponse<IEnumerable<FeedbackDto>>> GetByAuthorAsync(Guid authorId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(authorId);
        if (user == null)
        {
            return BaseResponse<IEnumerable<FeedbackDto>>.FailResult($"User with ID {authorId} not found.");
        }
        
        var feedbacks = await _unitOfWork.Feedbacks.GetByAuthorAsync(authorId);
        var feedbackDtos = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        
        return BaseResponse<IEnumerable<FeedbackDto>>.SuccessResult(feedbackDtos);
    }

    public async Task<BaseResponse<IEnumerable<FeedbackDto>>> GetByTagAsync(Guid tagId)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
        if (tag == null)
        {
            return BaseResponse<IEnumerable<FeedbackDto>>.FailResult($"Tag with ID {tagId} not found.");
        }
        
        var feedbacks = await _unitOfWork.Feedbacks.GetByTagAsync(tagId);
        var feedbackDtos = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        
        return BaseResponse<IEnumerable<FeedbackDto>>.SuccessResult(feedbackDtos);
    }
}