
namespace TestServer.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Возвращает либо сам массив как есть, либо пустой массив если аргумент <c>null</c>.
        /// </summary>
        public static T[] EmptyIfNull<T>(this T[] array)
        {
            return array != null ? array : [];
        }
    }
}
