using System;
using Microsoft.IdentityModel.Tokens;

public class Configuration
{
	public Configuration(IServiceCollection services)
	{
		services.AddDbContext<MobileContext>();
		services.AddControllersWithViews();
		services.AddControllers();
		services.AddSingleton<IChatService, ChatService>();
        services.AddScoped<IBaseEditableRepository<Phone>, PhoneRepository>();
        //services.AddScoped<IBaseRepository<Phone>>(x => x.GetService<PhoneRepository>());
        services.AddScoped<IPhoneService, PhoneService>();
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


