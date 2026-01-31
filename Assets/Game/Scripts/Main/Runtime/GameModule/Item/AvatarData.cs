namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class AvatarData
    {
        private InventoryData Inventory { get; set; }

        public AvatarData() { Inventory = new InventoryData(); }

        public AvatarData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public AvatarData Clone() => new AvatarData(Inventory?.Clone());

        public void Reset() { Inventory?.Reset(); }

        public override string ToString() => $"AvatarData(Inventory={Inventory})";
    }
}
