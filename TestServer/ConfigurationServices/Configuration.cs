using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using TestServer.Attributes;

public class Configuration
{
    private sealed class AttributeRegistrationStrategy<T> : RegistrationStrategy where T : DIAttribute
        {
            public override void Apply(IServiceCollection services, ServiceDescriptor descriptor)
            {
                var serviceType = descriptor.ImplementationType;
                if (serviceType == null)
                    return;
                var interfaseType = serviceType.GetInterfaces().FirstOrDefault(i =>
                {
                    if (i.Name.Contains('`'))
                        return serviceType.Name.Contains(i.Name.Split('`')[0].Substring(1)) && i.GenericTypeArguments.Count() > 0 && serviceType.Name.Contains(i.GenericTypeArguments.First().Name);
                    else
                        return serviceType.Name.Contains(i.Name.Substring(1));
                });
                var attribute = serviceType.GetCustomAttribute<T>();
                if (interfaseType != null && attribute != null)
                {
                    if (attribute.Key == null)
                        descriptor = ServiceDescriptor.Describe(interfaseType, serviceType, attribute.ServiceLifetime);
                    else
                        descriptor = ServiceDescriptor.DescribeKeyed(interfaseType, attribute.Key, serviceType, attribute.ServiceLifetime);
                    services.Add(descriptor);
                }
            }
        }

        private void ScanServices(ITypeSourceSelector selector)
        {
            var scanner = selector.FromAssembliesOf(GetType());

            scanner
                .AddClasses(classes => classes.WithAttribute<DITransient>())
                .UsingRegistrationStrategy(new AttributeRegistrationStrategy<DITransient>())
                .AsImplementedInterfaces()
                .WithTransientLifetime();
            scanner
                .AddClasses(classes => classes.WithAttribute<DIScoped>())
                .UsingRegistrationStrategy(new AttributeRegistrationStrategy<DIScoped>())
                .AsImplementedInterfaces()
                .WithScopedLifetime();
            scanner
                .AddClasses(classes => classes.WithAttribute<DISingleton>())
                .UsingRegistrationStrategy(new AttributeRegistrationStrategy<DISingleton>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime();
        }
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
        services.Scan(ScanServices);

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


