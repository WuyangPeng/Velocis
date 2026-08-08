using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Network
{
    public class CeleritasHandlerComponent : GameFrameworkComponent
    {
        private readonly Dictionary<Type, object> _celeritasHandlers = new();

        private void Start()
        {
            // 反射注册包和包处理函数（支持热更程序集中的 Handler）。
            var baseInterfaceType = typeof(ICeleritasHandler);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }

                foreach (var type in types)
                {
                    if (!type.IsClass || type.IsAbstract || !baseInterfaceType.IsAssignableFrom(type))
                    {
                        continue;
                    }

                    var interfaces = type.GetInterfaces();
                    foreach (var iface in interfaces)
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(ICeleritasHandler<>))
                        {
                            var messageType = iface.GetGenericArguments()[0];
                            var handler = Activator.CreateInstance(type);

                            if (_celeritasHandlers.TryGetValue(messageType, out var celeritasHandler))
                            {
                                Log.Warning("Duplicate handler for message type '{0}': '{1}' and '{2}'",
                                    messageType.Name,
                                    celeritasHandler.GetType().Name,
                                    type.Name);
                            }
                            else
                            {
                                _celeritasHandlers.Add(messageType, handler);
                            }

                            break;
                        }
                    }
                }
            }
        }

        public ICeleritasHandler<T> GetCeleritasHandler<T>()
        {
            if (_celeritasHandlers.TryGetValue(typeof(T), out var handler))
            {
                return handler as ICeleritasHandler<T>;
            }

            return null;
        }
    }
}