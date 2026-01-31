namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class FrameData
    {
        private InventoryData Inventory { get; set; }

        public FrameData() { Inventory = new InventoryData(); }

        public FrameData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public FrameData Clone() => new FrameData(Inventory?.Clone());

        public void Reset() { Inventory?.Reset(); }

        public override string ToString() => $"FrameData(Inventory={Inventory})";
    }
}
