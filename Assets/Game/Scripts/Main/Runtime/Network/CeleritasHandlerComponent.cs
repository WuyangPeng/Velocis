using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Network
{
    public class CeleritasHandlerComponent : GameFrameworkComponent
    {
        private readonly Dictionary<Type, object> m_CeleritasHandlers = new();

        private void Start()
        {
            // 反射注册包和包处理函数。

            var celeritasHandlerBaseType = typeof(ICeleritasHandler);
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                if (celeritasHandlerBaseType.IsAssignableFrom(type))
                {
                    var handler = Activator.CreateInstance(type);

                    var baseType = type.BaseType;
                    while (baseType != null)
                    {
                        if (baseType.IsGenericType &&
                            baseType.GetGenericTypeDefinition() == typeof(CeleritasHandlerBase<>))
                        {
                            var messageType = baseType.GetGenericArguments()[0];
                            if (m_CeleritasHandlers.ContainsKey(messageType))
                            {
                                Log.Warning("Duplicate handler for message type '{0}': '{1}' and '{2}'",
                                    messageType.Name, m_CeleritasHandlers[messageType].GetType().Name, type.Name);
                            }
                            else
                            {
                                m_CeleritasHandlers.Add(messageType, handler);
                            }

                            break;
                        }

                        baseType = baseType.BaseType;
                    }
                }
            }
        }

        public CeleritasHandlerBase<T> GetCeleritasHandler<T>()
        {
            if (m_CeleritasHandlers.TryGetValue(typeof(T), out var handler))
            {
                return handler as CeleritasHandlerBase<T>;
            }

            return null;
        }
    }
}