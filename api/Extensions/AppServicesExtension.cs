using System.Text;
using api.Data;
using api.Entities;
using api.Interfaces;
using api.Middleware;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace api.Extensions {
    public static class AppServicesExtension {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config) {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(opt => {
                opt.UseSqlite(config.GetConnectionString("ConnString"));
            });
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddIdentityCore<AppUser>(opt => {
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 8;
                }).AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                    };
                });

            return services;
        }
    }
}