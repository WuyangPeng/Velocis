using System.Runtime.InteropServices;
using Game.Scripts.Main.Runtime.Definition.Enum;

namespace Game.Scripts.Main.Runtime.Definition.DataStruct
{
    [StructLayout(LayoutKind.Auto)]
    public readonly struct ImpactData
    {
        public ImpactData(CampType camp, int hp, int attack, int defense)
        {
            Camp = camp;
            Hp = hp;
            Attack = attack;
            Defense = defense;
        }

        public CampType Camp { get; }

        public int Hp { get; }

        public int Attack { get; }

        public int Defense { get; }
    }
}
