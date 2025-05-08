namespace TestServer.Extensions
{
    public static class EnumExtensions
    {
        public static string ToName<T>(this T e)
            where T : Enum
        {
            return EnumHelper.GetName(e);
        }
    }
}