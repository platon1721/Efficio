// Efficio.Core/Application/Mappings/MappingProfile.cs (Update)
using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Input;
using Efficio.Core.Domain.Entities.Common;
using Efficio.Core.Domain.Entities.Communication;

namespace Efficio.Core.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.FullPhoneNumber))
            .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments));

        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Department mappings
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        
        // Feedback mappings
        CreateMap<Feedback, FeedbackDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));
        CreateMap<CreateFeedbackDto, Feedback>();
        
        // Post mappings
        CreateMap<Post, PostDto>()
            .IncludeBase<Feedback, FeedbackDto>()
            .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments));
        CreateMap<CreatePostDto, Post>();
        
        // Tag mappings
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();
        
        // Comment mappings
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentDto, Comment>();
    }
}