using Org.BouncyCastle.Crypto.Digests;
using System.Text;

public static class SM3Hash
{
    public static string ComputeHash(string input)
    {
        // 创建 SM3 摘要对象
        SM3Digest digest = new SM3Digest();

        // 将输入字符串转换为字节数组
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        // 更新摘要对象
        digest.BlockUpdate(inputBytes, 0, inputBytes.Length);

        // 计算哈希值
        byte[] result = new byte[digest.GetDigestSize()];
        digest.DoFinal(result, 0);

        // 将字节数组转换为十六进制字符串
        StringBuilder sb = new StringBuilder();
        foreach (byte b in result)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
