namespace TestServer.Attributes
{
    /// <summary>
    /// Предназначен для определния "внутреннего" имени для значения enum'ов.
    /// </summary>
    /// <seealso cref="EnumExtensions.Name()"/>
    [AttributeUsage(AttributeTargets.Field)]
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }

}