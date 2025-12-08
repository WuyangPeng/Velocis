using System;
using System.Security.Cryptography;
using System.Text;

namespace Game.Scripts.Main.Runtime.Utils
{
    /// <summary>
    /// HMAC-SHA256 签名工具类。
    /// </summary>
    public static class HmacSha256Util
    {
        /// <summary>
        /// 使用 HMAC-SHA256 算法计算签名。
        /// 该方法接受一个或多个字符串参数，并将它们按传入顺序直接拼接后进行签名。
        /// </summary>
        /// <param name="secret">用于签名的密钥。</param>
        /// <param name="parametersToSign">一个或多个待签名的字符串参数（不定参数）。</param>
        /// <returns>计算出的签名字符串（小写十六进制格式）。</returns>
        public static string ComputeHash(string secret, params string[] parametersToSign)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentNullException(nameof(secret), "密钥不能为空。");
            }

            // 1. 将所有参数按顺序拼接成一个单一的字符串。
            var stringToSign = string.Concat(parametersToSign);

            // 2. 使用 UTF-8 编码获取密钥和待签名字符串的字节。
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var dataBytes = Encoding.UTF8.GetBytes(stringToSign);

            // 3. 计算 HMAC-SHA256 哈希值。
            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);

                // 4. 将哈希字节数组转换为小写十六进制字符串。
                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

/*
 * 使用示例:
 *
 * using Game.Scripts.Main.Runtime.Utils;
 * using UnityEngine;
 *
 * public class SignatureExample
 * {
 *     public void GenerateSignature()
 *     {
 *         // 1. 你的密钥
 *         var apiSecret = "your_super_secret_key";
 *
 *         // 2. 你的请求参数值
 *         var userId = "12345";
 *         var timestamp = "1678886400";
 *         var action = "get_user_info";
 *
 *         // 3. 调用工具方法，按顺序传入需要签名的各个部分。
 *         // 方法内部会将它们拼接成 "123451678886400get_user_info" 后再计算签名。
 *         var signature = HmacSha256Util.ComputeHash(apiSecret, userId, timestamp, action);
 *
 *         // 输出结果
 *         Debug.Log("生成的签名: " + signature);
 *
 *         // 4. 现在你可以将签名添加到你的请求中了
 *         // ...
 *     }
 * }
 */