using System.IO;
using UnityEngine;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public sealed partial class DataTableProcessor
    {
        private sealed class Color32Processor : GenericDataProcessor<Color32>
        {
            public override bool IsSystem => false;

            public override string LanguageKeyword => "Color32";

            public override string[] GetTypeStrings()
            {
                return new[]
                {
                    "color32",
                    "unityengine.color32"
                };
            }

            public override Color32 Parse(string value)
            {
                var splitValue = value.Split(',');
                return new Color32(byte.Parse(splitValue[0]), byte.Parse(splitValue[1]), byte.Parse(splitValue[2]), byte.Parse(splitValue[3]));
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var color32 = Parse(value);
                binaryWriter.Write(color32.r);
                binaryWriter.Write(color32.g);
                binaryWriter.Write(color32.b);
                binaryWriter.Write(color32.a);
            }
        }
    }
}
