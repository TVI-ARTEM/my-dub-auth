using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyDub.Auth.Configure;
using MyDub.Auth.Extensions;
using MyDub.Auth.Helpers;

namespace MyDub.Auth;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JWTOptions>(configuration.GetSection(nameof(JWTOptions)));
        services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSingleton<JWTHelper>();
            
        services.AddDal(configuration);
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                    ValidIssuer = configuration.GetSection("JWTOptions:Issuer").Value,
                    ValidAudience = configuration.GetSection("JWTOptions:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetSection("JWTOptions:SecretKey").Value!))
                };
            });
            
            services.AddAuthorization();
    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseEndpoints(o =>
        {
            o.MapControllers();
        });
    }
}