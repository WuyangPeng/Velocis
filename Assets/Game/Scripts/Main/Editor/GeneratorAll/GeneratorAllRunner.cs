using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.GeneratorAll
{
    /// <summary>
    /// 自动扫描带 [MenuItem("Generator/...")] 的静态方法并按目录前缀批量执行。
    /// 新增 Generator 单项时无需修改 GeneratorAll 代码。
    /// </summary>
    public static class GeneratorAllRunner
    {
        private const string GeneratorRoot = "Generator/";

        public static void Run(params string[] categoryPaths)
        {
            if (categoryPaths == null || categoryPaths.Length == 0)
            {
                RunCategory(null);
                return;
            }

            foreach (string categoryPath in categoryPaths)
            {
                RunCategory(categoryPath);
            }
        }

        private static void RunCategory(string categoryPath)
        {
            string prefix = string.IsNullOrEmpty(categoryPath)
                ? GeneratorRoot
                : $"{GeneratorRoot}{categoryPath.TrimEnd('/')}/";

            List<(MethodInfo Method, MenuItem Attribute)> entries = CollectGeneratorEntries(prefix);
            if (entries.Count == 0)
            {
                Debug.LogWarning($"[GeneratorAll] No generators found for '{prefix}'.");
                return;
            }

            Debug.Log($"[GeneratorAll] Running {entries.Count} generator(s) under '{prefix}'...");

            foreach ((MethodInfo method, MenuItem menuItem) in entries)
            {
                try
                {
                    method.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[GeneratorAll] Failed: {menuItem.menuItem}\n{ex}");
                }
            }

            string label = string.IsNullOrEmpty(categoryPath) ? "All" : categoryPath;
            Debug.Log($"[GeneratorAll] {label}: completed.");
        }

        private static List<(MethodInfo Method, MenuItem Attribute)> CollectGeneratorEntries(string prefix)
        {
            return TypeCache.GetMethodsWithAttribute<MenuItem>()
                .Select(method => (Method: method, Attribute: GetMenuItemAttribute(method)))
                .Where(entry => entry.Attribute != null)
                .Where(entry => entry.Method.ReturnType == typeof(void))
                .Where(entry => entry.Attribute.menuItem.StartsWith(prefix, StringComparison.Ordinal))
                .Where(entry => !entry.Attribute.menuItem.StartsWith("GeneratorAll/", StringComparison.Ordinal))
                .OrderBy(entry => entry.Attribute.priority)
                .ThenBy(entry => entry.Attribute.menuItem, StringComparer.Ordinal)
                .ToList();
        }

        private static MenuItem GetMenuItemAttribute(MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(MenuItem), false)
                .Cast<MenuItem>()
                .FirstOrDefault();
        }
    }
}
