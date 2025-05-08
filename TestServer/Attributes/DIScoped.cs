
namespace TestServer.Attributes
{
    public class DIScoped : DIAttribute
    {
        public DIScoped()
        {
            ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped;
        }
        public DIScoped(string key)
        {
            Key = key;
            ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped;
        }
    }
    public class DIScoped<T> : DIScoped where T : Enum
    {
        public DIScoped(T key) : base(EnumHelper.GetName(key))
        {
        }
    }
}