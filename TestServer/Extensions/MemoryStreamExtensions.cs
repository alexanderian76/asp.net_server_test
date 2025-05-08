namespace TestServer.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static string ToBase64String(this MemoryStream stream)
        {
            byte[] bytes;

            bytes = stream.ToArray();

            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
    }
}
