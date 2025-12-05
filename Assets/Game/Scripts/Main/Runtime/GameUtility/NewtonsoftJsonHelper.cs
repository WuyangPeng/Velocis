using GameFramework;
using Newtonsoft.Json;
using System;

namespace Game.Scripts.Main.Runtime.GameUtility
{
    public class NewtonsoftJsonHelper : Utility.Json.IJsonHelper
    {
        /// <summary>
        /// 全局序列化设置（可自行调整）。
        /// </summary>
        private static readonly JsonSerializerSettings Settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore, // 忽略空值
            DefaultValueHandling = DefaultValueHandling.Ignore, // 忽略默认值
            Formatting = Formatting.None, // 不格式化，节省体积
        };

        /// <summary>
        /// 将对象序列化为 JSON 字符串。
        /// </summary>
        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象（泛型）。
        /// </summary>
        public T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象（运行时指定类型）。
        /// </summary>
        public object ToObject(Type objectType, string json)
        {
            return objectType == null ? throw new ArgumentNullException(nameof(objectType)) : JsonConvert.DeserializeObject(json, objectType, Settings);
        }
    }
}