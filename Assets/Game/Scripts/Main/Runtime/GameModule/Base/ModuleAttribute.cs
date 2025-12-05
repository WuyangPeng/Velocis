using System;

namespace Game.Scripts.Main.Runtime.GameModule.Base
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleAttribute : Attribute { }
}