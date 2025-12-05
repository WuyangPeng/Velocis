using System.IO;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    public sealed partial class DataTableProcessor
    {
        private sealed class SingleProcessor : GenericDataProcessor<float>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "float";

            public override string[] GetTypeStrings()
            {
                return new[]
                {
                    "float",
                    "single",
                    "system.single"
                };
            }

            public override float Parse(string value)
            {
                return float.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}
