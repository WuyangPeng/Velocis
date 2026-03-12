namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class ExpData
    {
        public ExpData()
        {
            Inventory = new InventoryData();
        }

        public ExpData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public InventoryData Inventory { get; set; }

        public ExpData Clone()
        {
            return new ExpData(Inventory?.Clone());
        }

        public void Reset()
        {
            Inventory?.Reset();
        }

        public override string ToString()
        {
            return $"ExpData(Inventory={Inventory})";
        }
    }
}