namespace ChatApp.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) ? value : value.Length <= maxLength ? value : value[..maxLength] + "...";
        }

        public static int ToInt32(this string value)
            => int.Parse(value);
    }
}
