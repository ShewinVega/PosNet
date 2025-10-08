using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PosNet.Automappers;
using PosNet.Constants;
using PosNet.DTOs;
using PosNet.Middlewares;
using PosNet.Models;
using PosNet.Repository;
using PosNet.Repository.Auth;
using PosNet.Services;
using PosNet.Validators.User;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding((context, _) =>
        {
            // Seed Roles
            var rolesExist = context.Set<Roles>().Any();
            if (!rolesExist)
            {
                context.Set<Roles>().AddRange(
                    new Roles { Name = RolesConstants.Admin },
                    new Roles { Name = RolesConstants.User }
                );

                context.SaveChanges();
            }


            // Seed Permissions
            var permissionsExist = context.Set<Permissions>().Any();
            if (!permissionsExist)
            {
                context.Set<Permissions>().AddRange(
                    new Permissions { Name = PermissionsConstants.UserRead },
                    new Permissions { Name = PermissionsConstants.UserWrite },
                    new Permissions { Name = PermissionsConstants.UserUpdate },
                    new Permissions { Name = PermissionsConstants.UserDelete }
                );

                context.SaveChanges();
            }

            // Seed Role-Permission Relationships
            var adminRole = context.Set<Roles>().Include(r => r.Permissions).FirstOrDefault(r => r.Name == RolesConstants.Admin);
            var userRole = context.Set<Roles>().Include(r => r.Permissions).FirstOrDefault(r => r.Name == RolesConstants.User);

            var readPermission = context.Set<Permissions>().FirstOrDefault(p => p.Name == PermissionsConstants.UserRead);
            var writePermission = context.Set<Permissions>().FirstOrDefault(p => p.Name == PermissionsConstants.UserWrite);
            var updatePermission = context.Set<Permissions>().FirstOrDefault(p => p.Name == PermissionsConstants.UserUpdate);
            var deletePermission = context.Set<Permissions>().FirstOrDefault(p => p.Name == PermissionsConstants.UserDelete);

            if (adminRole != null && userRole != null && readPermission != null && writePermission != null && updatePermission != null && deletePermission != null)
            {
                if (!adminRole.Permissions.Contains(readPermission))
                    adminRole.Permissions.Add(readPermission);
                if (!adminRole.Permissions.Contains(writePermission))
                    adminRole.Permissions.Add(writePermission);
                if (!adminRole.Permissions.Contains(updatePermission))
                    adminRole.Permissions.Add(updatePermission);
                if (!adminRole.Permissions.Contains(deletePermission))
                    adminRole.Permissions.Add(deletePermission);

                context.SaveChanges();
            }


            // Seed Admin User
            if(adminRole != null) // Here we verify that the adminRole is not null
            {
                var adminUserExists = context.Set<User>().Any(item => item.RoleId == adminRole.Id);
                if (!adminUserExists)
                {
                    // get the user instance
                    var newUser = new User()
                    {
                        Username = builder.Configuration["User:username"],
                        PasswordHash = builder.Configuration["User:password"],
                        RoleId = adminRole.Id
                    };

                    // Hashing Password
                    var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, newUser.PasswordHash);
                    newUser.PasswordHash = hashedPassword;

                    context.Set<User>().Add(newUser);

                    context.SaveChanges();
                }
            }

        }).UseAsyncSeeding(async (context, _, CancellationToken) =>
        {
            // Seed Roles
            var rolesExist = await context.Set<Roles>().AnyAsync();
            if (!rolesExist)
            {
                context.Set<Roles>().AddRange(
                    new Roles { Name = RolesConstants.Admin },
                    new Roles { Name = RolesConstants.User }
                );

                await context.SaveChangesAsync();
            }


            // Seed Permissions
            var permissionsExist = await context.Set<Permissions>().AnyAsync();
            if (!permissionsExist)
            {
                context.Set<Permissions>().AddRange(
                    new Permissions { Name = PermissionsConstants.UserRead },
                    new Permissions { Name = PermissionsConstants.UserWrite },
                    new Permissions { Name = PermissionsConstants.UserUpdate },
                    new Permissions { Name = PermissionsConstants.UserDelete }
                );

                await context.SaveChangesAsync();
            }

            // Seed Role-Permission Relationships
            var adminRole = await context.Set<Roles>().FirstOrDefaultAsync(r => r.Name == RolesConstants.Admin);
            var userRole = await context.Set<Roles>().FirstOrDefaultAsync(r => r.Name == RolesConstants.User);

            var readPermission = await context.Set<Permissions>().FirstOrDefaultAsync(p => p.Name == PermissionsConstants.UserRead);
            var writePermission = await context.Set<Permissions>().FirstOrDefaultAsync(p => p.Name == PermissionsConstants.UserWrite);
            var updatePermission = await context.Set<Permissions>().FirstOrDefaultAsync(p => p.Name == PermissionsConstants.UserUpdate);
            var deletePermission = await context.Set<Permissions>().FirstOrDefaultAsync(p => p.Name == PermissionsConstants.UserDelete);

            if (adminRole != null && userRole != null && readPermission != null && writePermission != null && updatePermission != null && deletePermission != null)
            {
                if (!adminRole.Permissions.Contains(readPermission))
                    adminRole.Permissions.Add(readPermission);
                if (!adminRole.Permissions.Contains(writePermission))
                    adminRole.Permissions.Add(writePermission);
                if (!adminRole.Permissions.Contains(updatePermission))
                    adminRole.Permissions.Add(updatePermission);
                if (!adminRole.Permissions.Contains(deletePermission))
                    adminRole.Permissions.Add(deletePermission);

                await context.SaveChangesAsync();
            }


            // Seed Admin User
            if (adminRole != null) // Here we verify that the adminRole is not null
            {
                var adminUserExists = await context.Set<User>().AnyAsync(item => item.RoleId == adminRole.Id);
                if (!adminUserExists)
                {
                    // get the user instance
                    var newUser = new User()
                    {
                        Username = builder.Configuration["User:username"],
                        PasswordHash = builder.Configuration["User:password"],
                        RoleId = adminRole.Id
                    };

                    // Hashing Password
                    var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, newUser.PasswordHash);
                    newUser.PasswordHash = hashedPassword;

                    await context.Set<User>().AddAsync(newUser);

                    await context.SaveChangesAsync();
                }
            }
        });
});

// Services
builder.Services.AddScoped<IAuthService, AuthService>();

// Repository
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Validators
builder.Services.AddScoped<IValidator<AuthDto>, RegisterValidation>();

// Authentication & Authorization Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:secretKey"])),
        ValidateIssuerSigningKey = true
    };
});


// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Read configuration by levels from appsettings.json
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

// Seeders
//var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
