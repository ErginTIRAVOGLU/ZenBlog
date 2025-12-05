using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Application.Options;
using ZenBlog.Domain.Entities;
using ZenBlog.Persistence.Concrete;
using ZenBlog.Persistence.Context;
using ZenBlog.Persistence.Interceptors;

namespace ZenBlog.Persistence.Extensions;

public static class ServiceRegistrations
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>{
            options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            options.AddInterceptors(new AuditContextInterceptor());
            options.UseLazyLoadingProxies();
        });

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<AppDbContext>();
    
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IJwtService, JwtService>();
        
        services.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));

        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
           var jwtTokenOptions = configuration.GetSection("JwtTokenOptions").Get<JwtTokenOptions>();
           
           options.TokenValidationParameters = new TokenValidationParameters
           {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtTokenOptions!.Issuer,
                ValidAudience = jwtTokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.SecretKey!)),
                ClockSkew = TimeSpan.Zero
           };
        });
        services.AddAuthorization();
    }
}