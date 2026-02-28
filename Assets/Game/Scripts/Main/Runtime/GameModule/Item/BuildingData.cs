namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class BuildingData
    {
        public BuildingData()
        {
            Inventory = new InventoryData();
        }

        public BuildingData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public BuildingData(InventoryData inventory, int level)
        {
            Inventory = inventory ?? new InventoryData();
            Level = level;
        }

        private InventoryData Inventory { get; }

        public int Level { get; set; }

        public BuildingData Clone()
        {
            return new BuildingData(Inventory?.Clone(), Level);
        }

        public void Reset()
        {
            Inventory?.Reset();
            Level = 0;
        }

        public override string ToString()
        {
            return $"BuildingData(Inventory={Inventory}, Level={Level})";
        }
    }
}
