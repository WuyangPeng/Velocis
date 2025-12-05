using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Http
{
    /// <summary>
    /// 一个使用静态 HttpClient 实例发出 HTTP 请求的帮助类。
    /// 此类设计简洁，用于通用的 HTTP 通信。
    /// </summary>
    public static class HttpHelper
    {
        // 为了提高性能并避免套接字耗尽，请使用单个静态 HttpClient 实例。
        private static readonly HttpClient HttpClient;

        static HttpHelper()
        {
            // 设置一个处理器以支持自动解压等功能，这是一种很好的做法。
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            HttpClient = new HttpClient(handler);
            // 设置一个合理的默认超时时间。如果需要，也可以在每个请求中单独配置。
            HttpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// 向指定的 URI 发送 GET 请求。
        /// </summary>
        /// <param name="requestUri">请求发送到的 URI。</param>
        /// <returns>一个代表异步操作的任务。任务结果包含响应体作为字符串；如果发生错误，则为 null。</returns>
        public static async Task<string> GetAsync(string requestUri)
        {
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode(); // 如果 HTTP 响应状态是错误代码，则会引发异常。
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            { 
                Log.Warning($"[HttpHelper] 向 {requestUri} 发送 GET 请求失败: {e.Message}");
                return null;
            }
            catch (TaskCanceledException e) // 捕获超时
            {
                Log.Warning($"[HttpHelper] 向 {requestUri} 发送 GET 请求超时: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Log.Warning($"[HttpHelper] 向 {requestUri} 发送 GET 请求时发生意外错误: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 发送带有字符串负载的 POST 请求。
        /// </summary>
        /// <param name="requestUri">请求发送到的 URI。</param>
        /// <param name="payload">要在请求体中发送的字符串数据。</param>
        /// <param name="mediaType">内容的内容类型，例如 "application/json"。</param>
        /// <returns>一个代表异步操作的任务。任务结果包含响应体作为字符串；如果发生错误，则为 null。</returns>
        public static async Task<string> PostAsync(string requestUri, string payload, string mediaType = "application/json")
        {
            try
            {
                var content = new StringContent(payload, Encoding.UTF8, mediaType);
                HttpResponseMessage response = await HttpClient.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Log.Warning($"[HttpHelper] 向 {requestUri} 发送 POST 请求失败: {e.Message}");
                return null;
            }
            catch (TaskCanceledException e) // 捕获超时
            {
                Log.Warning($"[HttpHelper] 向 {requestUri} 发送 POST 请求超时: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Log.Warning($"[HttpHelper] 向 {requestUri} 发送 POST 请求时发生意外错误: {e.Message}");
                return null;
            }
        }
    }
}

/*
 * 使用示例:
 *
 * using Game.Scripts.Main.Runtime.Http;
 * using System.Threading.Tasks;
 * using UnityEngine;
 *
 * public class MyApiService
 * {
 *     // 配合 JSON 库（如 Newtonsoft.Json (Json.NET) 或 Unity 的 JsonUtility）使用:
 *     //
 *     // 1. 定义你的数据结构:
 *     // [System.Serializable]
 *     // public class MyRequestData { public string name; }
 *     //
 *     // [System.Serializable]
 *     // public class MyResponseData { public string message; }
 *
 *     public async Task<MyResponseData> SendMyRequest()
 *     {
 *         var requestData = new MyRequestData { name = "Player1" };
 *         // 使用 JsonUtility (Unity内置) 或 JsonConvert (Newtonsoft.Json)
 *         string jsonPayload = JsonUtility.ToJson(requestData);
 *
 *         string responseJson = await HttpHelper.PostAsync("https://api.example.com/myendpoint", jsonPayload);
 *
 *         if (responseJson != null)
 *         {
 *             MyResponseData responseData = JsonUtility.FromJson<MyResponseData>(responseJson);
 *             Debug.Log("请求成功: " + responseData.message);
 *             return responseData;
 *         }
 *
 *         Debug.Log("请求失败。");
 *         return null;
 *     }
 * }
 */