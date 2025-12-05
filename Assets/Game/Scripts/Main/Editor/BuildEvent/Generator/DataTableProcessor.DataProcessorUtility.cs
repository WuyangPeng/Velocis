using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public sealed partial class DataTableProcessor
    {
        private static class DataProcessorUtility
        {
            private static readonly IDictionary<string, DataProcessor> DataProcessors = new SortedDictionary<string, DataProcessor>(StringComparer.Ordinal);

            static DataProcessorUtility()
            {
                var dataProcessorBaseType = typeof(DataProcessor);
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsClass || type.IsAbstract)
                    {
                        continue;
                    }

                    if (!dataProcessorBaseType.IsAssignableFrom(type))
                    {
                        continue;
                    }

                    var dataProcessor = (DataProcessor)Activator.CreateInstance(type);
                    foreach (var typeString in dataProcessor.GetTypeStrings())
                    {
                        DataProcessors.Add(typeString.ToLowerInvariant(), dataProcessor);
                    }
                }
            }

            public static DataProcessor GetDataProcessor(string type)
            {
                type ??= string.Empty;

                return DataProcessors.TryGetValue(type.ToLowerInvariant(), out var dataProcessor) ? dataProcessor : throw new GameFrameworkException(Utility.Text.Format("Not supported data processor type '{0}'.", type));
            }
        }
    }
}
