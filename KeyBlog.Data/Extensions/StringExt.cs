using KeyBlog.Data.Utils;

namespace KeyBlog.Data.Extensions;

public static class StringExt
{
    public static string Limit(this string str, int length)
    {
        return str.Length <= length ? str : str[..length];
    }

    /// <summary>
    /// 限制字符串显示长度并在末尾添加省略号
    /// </summary>
    public static string LimitWithEllipsis(this string str, int length)
    {
        return str.Length <= length ? str : $"{str[..length]}...";
    }

    public static string ToSM3Hash(this string source)
    {
        return SM3Hash.ComputeHash(source);
    }
}