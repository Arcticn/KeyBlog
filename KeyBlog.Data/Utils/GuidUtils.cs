using System.Security.Cryptography;

namespace KeyBlog.Data.Utils;

public static class GuidUtils
{
    /// <summary>
    /// 没有连字符分隔的32位数字
    /// </summary>
    /// <returns></returns>
    public static string GetGuid()
    {
        var guid = Guid.NewGuid();
        return guid.ToString("N");
    }

    /// <summary>  
    /// 根据GUID获取16位的唯一字符串  
    /// </summary>  
    /// <returns></returns>  
    // public static string GuidTo16String()
    // {
    //     long i = 1;
    //     foreach (var b in Guid.NewGuid().ToByteArray())
    //     {
    //         i *= b + 1;
    //     }

    //     return $"{i - DateTime.Now.Ticks:x}";
    // }

    /// <summary>  
    /// 根据GUID获取19位的唯一数字序列  
    /// </summary>  
    /// <returns></returns>  
    public static long GuidToLongID()
    {
        var buffer = Guid.NewGuid().ToByteArray();
        return BitConverter.ToInt64(buffer, 0);
    }
}