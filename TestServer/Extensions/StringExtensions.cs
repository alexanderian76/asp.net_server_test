namespace TestServer.Extensions
{
    public static class StringExtensions
    {
        public static string ToCapitalize(this string input, bool enforceLowingOthers = false)
        {
            static void CapitalizeFirstLetterOnly(Span<char> chars, string str)
            {
                chars[0] = char.ToUpperInvariant(str[0]);
                str.AsSpan(1).CopyTo(chars[1..]);
            }

            static void CapitalizeAndLowingOthers(Span<char> chars, string str)
            {
                chars[0] = char.ToUpperInvariant(str[0]);
                for (int i = 1; i < str.Length; i++)
                {
                    chars[i] = char.ToLowerInvariant(str[i]);
                }
            }

            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return string.Create(input.Length, input,
                enforceLowingOthers ? CapitalizeAndLowingOthers : CapitalizeFirstLetterOnly);
        }
    }

}