using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Base;
using Efficio.Core.Application.DTOs.Create;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.Services.Interfaces;
using Efficio.Core.Domain.Entities.Communication;
using Efficio.Core.Domain.Interfaces;

namespace Efficio.Core.Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<PostDto>> GetByIdAsync(Guid id)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);
        if (post == null)
        {
            return BaseResponse<PostDto>.FailResult($"Post with ID {id} not found.");
        }

        var postDto = _mapper.Map<PostDto>(post);
        return BaseResponse<PostDto>.SuccessResult(postDto);
    }

    public async Task<BaseResponse<IEnumerable<PostDto>>> GetAllAsync()
    {
        var posts = await _unitOfWork.Posts.GetAllAsync();
        var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
        return BaseResponse<IEnumerable<PostDto>>.SuccessResult(postDtos);
    }

    public async Task<BaseResponse<PostDto>> CreateAsync(CreatePostDto createDto, Guid userId)
    {
        try
        {
            // Validate departments exist
            if (!createDto.DepartmentIds.Any())
            {
                return BaseResponse<PostDto>.FailResult("At least one department must be specified for a post.");
            }
            
            foreach (var departmentId in createDto.DepartmentIds)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                if (department == null)
                {
                    return BaseResponse<PostDto>.FailResult($"Department with ID {departmentId} not found.");
                }
            }

            // Create post
            var post = _mapper.Map<Post>(createDto);
            post.MadeBy = userId;
            await _unitOfWork.Posts.AddAsync(post);

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
                            FeedbackId = post.Id,
                            TagId = tagId
                        };
                        post.FeedbackTags.Add(feedbackTag);
                    }
                }
            }

            // Add departments
            foreach (var departmentId in createDto.DepartmentIds)
            {
                var postDepartment = new PostDepartment
                {
                    PostId = post.Id,
                    DepartmentId = departmentId
                };
                post.PostDepartments.Add(postDepartment);
            }

            await _unitOfWork.CompleteAsync();
            
            // Get created post with relationships
            var createdPost = await _unitOfWork.Posts.GetWithDepartmentsAsync(post.Id);
            var postDto = _mapper.Map<PostDto>(createdPost);
            
            return BaseResponse<PostDto>.SuccessResult(postDto, "Post created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<PostDto>.FailResult($"Error creating post: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PostDto>> UpdateAsync(Guid id, UpdatePostDto updateDto, Guid userId)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);
        if (post == null)
        {
            return BaseResponse<PostDto>.FailResult($"Post with ID {id} not found.");
        }

        // Check if user is the author
        if (post.MadeBy != userId)
        {
            return BaseResponse<PostDto>.FailResult("You do not have permission to update this post.");
        }

        try
        {
            // Update post properties
            _mapper.Map(updateDto, post);
            await _unitOfWork.Posts.UpdateAsync(post);

            // Update tags if provided
            if (updateDto.TagIds != null)
            {
                // Get current post with tags
                var postWithTags = await _unitOfWork.Posts.GetWithTagsAsync(id);
                var currentTagIds = postWithTags.FeedbackTags.Select(ft => ft.TagId).ToList();
                
                // Tags to remove
                var tagIdsToRemove = currentTagIds.Except(updateDto.TagIds).ToList();
                foreach (var tagId in tagIdsToRemove)
                {
                    var feedbackTag = postWithTags.FeedbackTags.FirstOrDefault(ft => ft.TagId == tagId);
                    if (feedbackTag != null)
                    {
                        postWithTags.FeedbackTags.Remove(feedbackTag);
                    }
                }
                
                // Tags to add
                var tagIdsToAdd = updateDto.TagIds.Except(currentTagIds).ToList();
                foreach (var tagId in tagIdsToAdd)
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        postWithTags.FeedbackTags.Add(new FeedbackTag
                        {
                            FeedbackId = id,
                            TagId = tagId
                        });
                    }
                }
            }

            // Update departments if provided
            if (updateDto.DepartmentIds != null)
            {
                if (!updateDto.DepartmentIds.Any())
                {
                    return BaseResponse<PostDto>.FailResult("At least one department must be specified for a post.");
                }
                
                // Validate departments exist
                foreach (var departmentId in updateDto.DepartmentIds)
                {
                    var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                    if (department == null)
                    {
                        return BaseResponse<PostDto>.FailResult($"Department with ID {departmentId} not found.");
                    }
                }
                
                // Get current post with departments
                var postWithDepartments = await _unitOfWork.Posts.GetWithDepartmentsAsync(id);
                var currentDeptIds = postWithDepartments.PostDepartments.Select(pd => pd.DepartmentId).ToList();
                
                // Departments to remove
                var deptIdsToRemove = currentDeptIds.Except(updateDto.DepartmentIds).ToList();
                foreach (var deptId in deptIdsToRemove)
                {
                    var postDept = postWithDepartments.PostDepartments.FirstOrDefault(pd => pd.DepartmentId == deptId);
                    if (postDept != null)
                    {
                        postWithDepartments.PostDepartments.Remove(postDept);
                    }
                }
                
                // Departments to add
                var deptIdsToAdd = updateDto.DepartmentIds.Except(currentDeptIds).ToList();
                foreach (var deptId in deptIdsToAdd)
                {
                    postWithDepartments.PostDepartments.Add(new PostDepartment
                    {
                        PostId = id,
                        DepartmentId = deptId
                    });
                }
            }

            await _unitOfWork.CompleteAsync();
            
            // Get updated post
            var updatedPost = await _unitOfWork.Posts.GetWithDepartmentsAsync(id);
            var postDto = _mapper.Map<PostDto>(updatedPost);
            
            return BaseResponse<PostDto>.SuccessResult(postDto, "Post updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<PostDto>.FailResult($"Error updating post: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);
        if (post == null)
        {
            return BaseResponse<bool>.FailResult($"Post with ID {id} not found.");
        }

        // Check if user is the author
        if (post.MadeBy != userId)
        {
            return BaseResponse<bool>.FailResult("You do not have permission to delete this post.");
        }

        try
        {
            // Delete post
            await _unitOfWork.Posts.RemoveAsync(post);
            await _unitOfWork.CompleteAsync();
            
            return BaseResponse<bool>.SuccessResult(true, "Post deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.FailResult($"Error deleting post: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PostDto>> GetWithCommentsAsync(Guid id)
    {
        var post = await _unitOfWork.Posts.GetWithCommentsAsync(id);
        if (post == null)
        {
            return BaseResponse<PostDto>.FailResult($"Post with ID {id} not found.");
        }

        var postDto = _mapper.Map<PostDto>(post);
        return BaseResponse<PostDto>.SuccessResult(postDto);
    }

    public async Task<BaseResponse<PostDto>> GetWithTagsAsync(Guid id)
    {
        var post = await _unitOfWork.Posts.GetWithTagsAsync(id);
        if (post == null)
        {
            return BaseResponse<PostDto>.FailResult($"Post with ID {id} not found.");
        }

        var postDto = _mapper.Map<PostDto>(post);
        return BaseResponse<PostDto>.SuccessResult(postDto);
    }

    public async Task<BaseResponse<PostDto>> GetWithDepartmentsAsync(Guid id)
    {
        var post = await _unitOfWork.Posts.GetWithDepartmentsAsync(id);
        if (post == null)
        {
            return BaseResponse<PostDto>.FailResult($"Post with ID {id} not found.");
        }

        var postDto = _mapper.Map<PostDto>(post);
        return BaseResponse<PostDto>.SuccessResult(postDto);
    }

    public async Task<BaseResponse<IEnumerable<PostDto>>> GetByDepartmentAsync(Guid departmentId)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
        if (department == null)
        {
            return BaseResponse<IEnumerable<PostDto>>.FailResult($"Department with ID {departmentId} not found.");
        }
        
        var posts = await _unitOfWork.Posts.GetByDepartmentAsync(departmentId);
        var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
        
        return BaseResponse<IEnumerable<PostDto>>.SuccessResult(postDtos);
    }

    public async Task<BaseResponse<IEnumerable<PostDto>>> GetByAuthorAsync(Guid authorId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(authorId);
        if (user == null)
        {
            return BaseResponse<IEnumerable<PostDto>>.FailResult($"User with ID {authorId} not found.");
        }
        
        var posts = await _unitOfWork.Posts.GetByAuthorAsync(authorId);
        var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
        
        return BaseResponse<IEnumerable<PostDto>>.SuccessResult(postDtos);
    }
}