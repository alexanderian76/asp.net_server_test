
namespace TestServer.Attributes
{
    public class DITransient : DIAttribute
    {
        public DITransient()
        {
            ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient;
        }
        public DITransient(string key)
        {
            Key = key;
            ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient;
        }
    }
    public class DITransient<T> : DITransient where T : Enum
    {
        public DITransient(T key) : base(EnumHelper.GetName(key))
        {
        }
    }
}