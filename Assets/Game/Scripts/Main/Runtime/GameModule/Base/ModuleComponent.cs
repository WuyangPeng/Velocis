using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.GameModule.Base
{
    public class ModuleComponent : GameFrameworkComponent
    {
        private readonly Dictionary<Type, BaseModule> _modules = new();

        public void InitModule()
        {
            var attributes = ScanWithAttribute();
            foreach (var attribute in attributes)
            {
                var instance = (BaseModule)Activator.CreateInstance(attribute);
                if (instance.IsLoad)
                {
                    _modules.Add(attribute, instance);
                }
            }
        }


        public void ResetModule()
        {
            if (_modules.Count == 0)
            {
                InitModule();
                return;
            }

            var types = _modules.Select(baseModule => baseModule.Key).ToList();

            _modules.Clear();

            foreach (var type in types)
            {
                var instance = (BaseModule)Activator.CreateInstance(type);
                _modules.Add(type, instance);
            }
        }

        public static List<Type> ScanWithAttribute()
        {
            var list = new List<Type>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.IsDynamic)
                {
                    continue;
                }

                try
                {
                    var found = asm.GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(BaseModule)) &&
                                    t.GetCustomAttribute<ModuleAttribute>() != null);
                    list.AddRange(found);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex);
                }
            }

            return list;
        }

        public BaseModule GetBaseModule(Type type)
        {
            return _modules[type];
        }

        public T GetModule<T>() where T : BaseModule
        {
            return (T)GetBaseModule(typeof(T));
        }

        public void LoginFinish()
        {
            foreach (var module in _modules)
            {
                module.Value.LoginFinish();
            }
        }
    }
}