
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PosNet.Domain.Constants;
using PosNet.Domain.Interfaces;
using PosNet.Infrastructure.Authentication;
using PosNet.Infrastructure.Persistence;
using PosNet.Infrastructure.ProblemsDetail;
using PosNet.Infrastructure.Repositories;
using PosNet.Infrastructure.Security;
using PosNet.UseCases.Interfaces;
using System.Text;
using System.Text.Json.Serialization;

namespace PosNet.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
             var configBuilder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddUserSecrets<AppDbContext>()
                .Build();

            services.AddControllers(options =>
            {
                options.Filters.Add<CustomValidationFilter>();
            });

            // Database context
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configBuilder.GetConnectionString("DefaultConnection"))
                    .UseSeeding((context, _) =>
                    {
                        SeedAdminUser(context, configBuilder, CancellationToken.None);
                    })
                    .UseAsyncSeeding(async (context, _, cancellationToken) =>
                    {
                        await SeedAdminUserAsync(context, configBuilder, cancellationToken);
                    });
            });

            // Context Accessor
            services.AddHttpContextAccessor();

            // Repositories and other services instances
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<ProblemTypes>();
            services.AddScoped<IHandleBusinessError, HandleBusinessError>();
            services.AddScoped<IPasswordEncrypt, BCryptHasher>();
            services.AddScoped<ITokenAuthService, AuthTokenService>();
            services.Configure<JwtSettings>(configBuilder.GetSection(key: "Jwt"));

            // Authenticaton Configuration
            var jwt = configBuilder.GetSection(key: "Jwt").Get<JwtSettings>();
            var secretKeyBytes = Encoding.UTF8.GetBytes(jwt.SecretKey);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
                    };
                });

            return services;
        }

        // Seeding Configuration
        private static void SeedAdminUser(DbContext context, IConfiguration configuration, CancellationToken ct)
        {
            SeedAdminUserAsync(context, configuration, ct).GetAwaiter().GetResult();
        }

        private static async Task SeedAdminUserAsync(DbContext context, IConfiguration configuration, CancellationToken ct)
        {
            // Get serviceProvider
            var serviceProvider = ((IInfrastructure<IServiceProvider>)context).Instance;
            var logger = serviceProvider.GetService<ILogger<AppDbContext>>();

            // User admin data
            var adminUsername = configuration["User:Name"];
            var adminEmail = configuration["User:Email"];
            var adminPassword = configuration["User:Password"];

            if(string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword) || string.IsNullOrEmpty(adminUsername))
            {
                logger?.LogError("Configuration Error, User:Email and password does not exist in environment variables");
                return;
            }

            // Verify if admin role and user exist
            var rolesData = await context.Set<Role>().AnyAsync();

            if(!rolesData)
            {
                context.Set<Role>().AddRange(
                    new Role { Name = Roles.Admin },
                    new Role { Name = Roles.Supervisor },
                    new Role { Name = Roles.Cashier },
                    new Role { Name = Roles.User }
                );

                try
                {
                    await context.SaveChangesAsync(ct);
                }
                catch (Exception error)
                {
                    logger?.LogError(error, "Roles were not created");
                    throw;
                }
            }


            var adminUser = await context.Set<User>().AnyAsync(u => u.Username == adminUsername, ct);
            if (!adminUser)
            {
                // Create Permissions for the admin user
                await context.Set<Permission>().AddRangeAsync(Permissions.All().Select(p => Permission.Create(p)));

                try
                {
                    await context.SaveChangesAsync(ct);
                }
                catch (Exception error)
                {
                    logger?.LogError(error, "Roles were not created");
                    throw;
                }

                // Get Admin Role Id
                var adminRole = await context.Set<Role>()
                    .Include(r => r.RolesPermissions)
                    .FirstOrDefaultAsync(r => r.Name == Roles.Admin, ct);

                if(adminRole != null)
                {
                    // Add Permissions to Admin role
                    var getPermissions = await context.Set<Permission>().ToListAsync();
                    adminRole.AddPermissions(getPermissions);
                    

                    var admin = new User()
                    {
                        Username = adminUsername,
                        Email = adminEmail,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                        RoleId = adminRole.Id
                    };

                    context.Set<User>().Add(admin);
                    
                    try
                    {
                        await context.SaveChangesAsync(ct);
                    } catch(Exception error)
                    {
                        logger?.LogError(error,"Admin user was not created");
                        throw;
                    }
                }
            }
        }

    }
}
