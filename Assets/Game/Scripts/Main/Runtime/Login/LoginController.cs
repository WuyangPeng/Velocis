using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Main.Runtime.Game;
using Game.Scripts.Main.Runtime.Http;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Login
{
    public class LoginController
    {
        /// <summary>
        /// 登录成功后，包含从服务器返回的令牌和其他数据。
        /// </summary>
        public TokenHttpResponse TokenResponse { get; private set; }

        /// <summary>
        /// 执行游客登录的异步操作。
        /// </summary>
        /// <returns>一个布尔值，表示操作是否成功。</returns>
        public async Task<bool> GuestLoginAsync()
        {
            var url = "http://127.0.0.1:8888/api/login/guest";
            var queryParams = new Dictionary<string, string>
            {
                { "deviceId", SystemInfo.deviceUniqueIdentifier }
            };

            try
            {
                var responseJson = await HttpHelper.GetAsync(url, queryParams);
                if (string.IsNullOrEmpty(responseJson))
                {
                    Log.Error("Guest login request failed: The response was null or empty."); 
                    return false;
                }

                TokenResponse = JsonUtility.FromJson<TokenHttpResponse>(responseJson);
                if (TokenResponse == null)
                {
                    Log.Error("Failed to deserialize the login response JSON.");
                    return false;
                }

                // 假设 code == 0 表示成功
                if (TokenResponse.code == GameErrorType.Success)
                {
                    Log.Info("Guest login successful.");
                    return true;
                }
                else
                {
                    Log.Warning($"Guest login failed with code: {TokenResponse.code}, Message: {TokenResponse.message}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("An exception occurred during guest login: " + e);
                TokenResponse = null;
                return false;
            }
        }
    }
}
