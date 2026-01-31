namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class HeroData
    {
        private InventoryData Inventory { get; set; }

        public HeroData() { Inventory = new InventoryData(); }

        public HeroData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public HeroData Clone() => new HeroData(Inventory?.Clone());

        public void Reset() { Inventory?.Reset(); }

        public override string ToString() => $"HeroData(Inventory={Inventory})";
    }
}
