using Microsoft.Extensions.DependencyInjection;

namespace TestServer.Attributes
{
    public class DIAttribute : Attribute
    {
        public string? Key { get; set; }
        public ServiceLifetime ServiceLifetime { get; set; }
    }

}