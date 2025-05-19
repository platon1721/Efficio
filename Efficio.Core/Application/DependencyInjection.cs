using Efficio.Core.Application.Mappings;
using Efficio.Core.Application.Services;
using Efficio.Core.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Efficio.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        // Register application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentService, CommentService>();
        
        return services;
    }
}