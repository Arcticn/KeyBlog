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

}