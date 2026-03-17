namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class ResourceData
    {
        public ResourceData()
        {
            Inventory = new InventoryData();
        }

        public ResourceData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public InventoryData Inventory { get; set; }

        public ResourceData Clone()
        {
            return new ResourceData(Inventory?.Clone());
        }

        public void Reset()
        {
            Inventory?.Reset();
        }

        public override string ToString()
        {
            return $"ResourceData(Inventory={Inventory})";
        }
    }
}
