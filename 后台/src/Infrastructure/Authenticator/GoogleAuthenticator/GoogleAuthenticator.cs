using OtpNet;
using System;
using System.Text;

namespace SourcePoint.Infrastructure.Authenticator.GoogleAuthenticator
{
    public class GoogleAuthenticator
    {
        /// <summary>
        /// 对验证码进行验证
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="publicKey">base32密钥</param>
        /// <returns></returns>
        public bool AuthenticationCode(string code, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            if (string.IsNullOrWhiteSpace(publicKey)) return false;

            return code == VerificationCode(publicKey);
        }
        /// <summary>
        /// 获取 验证码
        /// </summary>
        /// <param name="publicKey">base32密钥</param>
        /// <returns></returns>
        public string VerificationCode(string publicKey)
        {
            var key = Base32Encoding.ToBytes(publicKey);
            Totp totp = new Totp(key);
            var result = totp.ComputeTotp(DateTime.UtcNow);
            return result;
        }

        /// <summary>
        /// 根据提供的Key 生成google验证 base32密钥
        /// </summary>
        /// <param name="key">如果为空 采用随机生成方式</param>
        /// <returns></returns>
        public string GoogleKeyForKey(string key = null)
        {
            if (string.IsNullOrEmpty(key)) return GoogleKeyForRand(10);

            var keys = Encoding.UTF8.GetBytes(key);

            var keystr = Base32Encoding.ToString(keys);

            return keystr;
        }

        /// <summary>
        /// 随机生成google验证 base32密钥  
        /// </summary>
        /// <param name="length">种子长度</param>
        /// <returns></returns>
        public string GoogleKeyForRand(int length = 10)
        {
            var keys = KeyGeneration.GenerateRandomKey(length);
            var keystr = Base32Encoding.ToString(keys);
            return keystr;
        }
    }
}