namespace E2E.Tests.Extensions
{
    static class StringExtensions
    {
        public static bool IsUpper(this string str) => str.All(c => char.IsDigit(c) || char.IsUpper(c));
    }
 
}
