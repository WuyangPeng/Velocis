using Game.Scripts.Main.Runtime.GameEnum;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class SectBaseData
    {
        public long ID { get; set; }

        public int SectId { get; set; }

        public MoralityType MoralityType { get; set; } = MoralityType.Moderation;

    
    }
}