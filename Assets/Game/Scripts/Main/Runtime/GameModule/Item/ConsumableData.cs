namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class ConsumableData
    {
        private InventoryData Inventory { get; set; }

        public ConsumableData() { Inventory = new InventoryData(); }

        public ConsumableData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public ConsumableData Clone() => new ConsumableData(Inventory?.Clone());

        public void Reset() { Inventory?.Reset(); }

        public override string ToString() => $"ConsumableData(Inventory={Inventory})";
    }
}
