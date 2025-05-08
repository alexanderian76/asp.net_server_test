namespace TestServer.Attributes
{
    public class DISingleton : DIAttribute
    {
        public DISingleton()
        {
            ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton;
        }
        public DISingleton(string key)
        {
            Key = key;
            ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton;
        }
    }
    public class DISingleton<T> : DISingleton where T : Enum
    {
        public DISingleton(T key) : base(EnumHelper.GetName(key))
        {
        }
    }
}