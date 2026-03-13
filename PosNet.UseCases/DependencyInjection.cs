using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PosNet.UseCases.Dtos.Auth;
using PosNet.UseCases.Interfaces;
using PosNet.UseCases.Services;
using PosNet.UseCases.Validators.User;
using System.Reflection;

namespace PosNet.UseCases
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddUseCasesServices(this IServiceCollection services)
        {
            // Logging
            services.AddLogging(builder => builder.AddConsole());

            // Services instances
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();

            // Mapster configurations
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            // Validators
            services.AddScoped<IValidator<RegisterDto>, RegisterValidation>();

            return services;
        }

    }
}
