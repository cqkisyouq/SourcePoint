using System.Security.Cryptography;
using System.Text;

namespace SourcePoint.Core.Helpers.Common
{
    public class MD5Helper
    {
        public static string GetStringHash(string str)
        {
            StringBuilder sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var bytes = UTF8Encoding.UTF8.GetBytes(str);
                bytes = md5.ComputeHash(bytes);
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));//将字节转换为字符串
                } 
            }
            return sb.ToString();
        }
    }
}