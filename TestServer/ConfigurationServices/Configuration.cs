using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class Configuration
{
    public Configuration(IServiceCollection services)
    {
        services.AddDbContext<MobileContext>();
        services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
        services.AddControllers();
        services.AddSingleton<IChatService, ChatService>();
        services.AddScoped<IBaseEditableRepository<Phone>, PhoneRepository>();
        services.AddScoped<IBaseEditableRepository<Company>, CompanyRepository>();
        //services.AddScoped<IBaseRepository<Phone>>(x => x.GetService<PhoneRepository>());
        services.AddScoped<IPhoneService, PhoneService>();
        services.AddScoped<ICompanyService, CompanyService>();
        /* services.AddMvc().AddJsonOptions(options => {
             options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;


             });*/


        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
        services.AddAuthorization();
    }
}


