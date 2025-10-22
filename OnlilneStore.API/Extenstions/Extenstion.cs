using System.Text;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.API.Middlewares;
using Persistence;
using Persistence.Data.Identity;
using Services;
using Shared;
using Shared.ErrorModels;

namespace OnlineStore.API.Extenstions
{
    public static class Extenstion
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();
            services.AddSwaggerServices();
            services.ConfigureServices();
            services.AddIdentityServices();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices(configuration);
            services.ConfigureJwtServices(configuration);



            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services, IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = JwtOptions.Issuer,
                    ValidAudience = JwtOptions.audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey)),
                };
            });
            return services;
        }
        
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(); 
            services.AddEndpointsApiExplorer();
            return services;
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            return services;
        }
        
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Any())
                        .Select(m => new ValidationError()
                        {
                            Field = m.Key,
                            Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                        });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }

        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {
            await app.InitializeDataBaseAsync();
            app.UseGlobalErrorHandling();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbIntitializer>();
            await dbIntializer.InitializeAsync();


            return app;
        }
        
        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
             app.UseMiddleware<ErrorHandlingMiddleware>();

            return app;
        }
    }
}
