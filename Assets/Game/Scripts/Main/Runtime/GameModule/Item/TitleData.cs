namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class TitleData
    {
        private InventoryData Inventory { get; set; }

        public TitleData() { Inventory = new InventoryData(); }

        public TitleData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public TitleData Clone() => new TitleData(Inventory?.Clone());

        public void Reset() { Inventory?.Reset(); }

        public override string ToString() => $"TitleData(Inventory={Inventory})";
    }
}
