using System.Collections.Generic;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public class ProtoMessage
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public List<ProtoField> Fields { get; set; } = new();
        public List<string> OneOfs { get; set; } = new();
    }
}