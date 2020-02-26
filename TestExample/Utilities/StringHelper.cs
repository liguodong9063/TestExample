namespace TestExample.Utilities
{
    public static class StringHelper
    {
        public static string UpperInitial(string text)
        {
            return !string.IsNullOrEmpty(text) ? text.Substring(0, 1).ToUpper() + text.Substring(1) : string.Empty;
        }

        public static string LowerInitial(string text)
        {
            return !string.IsNullOrEmpty(text) ? text.Substring(0, 1).ToLower() + text.Substring(1) : string.Empty;
        }
    }
}
