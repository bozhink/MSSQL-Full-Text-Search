namespace FTSS.Common.Extensions
{
    using System.Collections.Generic;

    public static class LanguageExtensions
    {
        public static bool In<T>(this T source, params T[] items)
        {
            return (items as IList<T>).Contains(source);
        }
    }
}
