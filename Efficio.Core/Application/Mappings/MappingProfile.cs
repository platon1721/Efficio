// Efficio.Core/Application/Mappings/MappingProfile.cs (Update)
using AutoMapper;
using Efficio.Core.Application.DTOs;
using Efficio.Core.Application.DTOs.Update;
using Efficio.Core.Application.DTOs.Create;
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
            .ForAllMembers(opts
                => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        // Department mappings
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>()
            .ForAllMembers(opts
                => opts.Condition((src, dest, srcMember) => srcMember != null));

        
        // Feedback mappings
        CreateMap<Feedback, FeedbackDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));
        CreateMap<CreateFeedbackDto, Feedback>();
        CreateMap<UpdateFeedbackDto, Feedback>()
            .ForAllMembers(opts
                => opts.Condition((src, dest, srcMember) => srcMember != null));

        
        // Post mappings
        CreateMap<Post, PostDto>()
            .IncludeBase<Feedback, FeedbackDto>()
            .ForMember(dest => dest.Departments, opt
                => opt.MapFrom(src => src.Departments));
        CreateMap<CreatePostDto, Post>();
        CreateMap<UpdatePostDto, Post>()
            .ForAllMembers(opts
                => opts.Condition((src, dest, srcMember) => srcMember != null));

        
        // Tag mappings
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();
        CreateMap<UpdateTagDto, Tag>()
            .ForAllMembers(opts
                => opts.Condition((src, dest, srcMember) => srcMember != null));

        
        // Comment mappings
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<UpdateCommentDto, Comment>()
            .ForAllMembers(opts
                => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}