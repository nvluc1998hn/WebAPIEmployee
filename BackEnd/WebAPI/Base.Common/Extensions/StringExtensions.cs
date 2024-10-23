namespace Base.Common.Extensions
{
    public static class StringExtensions
    {
        public static string CreateClientMessage(this string languageKey, params object[] paramValues)
        {
            var message = $"{languageKey}";

            if (paramValues?.Length > 0)
            {
                message += "@@@" + string.Join("|||", paramValues);
            }

            return message;
        }
    }
}
