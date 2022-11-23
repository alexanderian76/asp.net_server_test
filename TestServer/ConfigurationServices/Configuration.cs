using System;

public class Configuration
{
	public Configuration(IServiceCollection services)
	{
		services.AddDbContext<MobileContext>();
		services.AddControllersWithViews();
		services.AddControllers();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        services.AddScoped<IPhoneService, PhoneService>();
	}
}


