namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class EquipmentData
    {
        public EquipmentData()
        {
            Inventory = new InventoryData();
        }

        public EquipmentData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public EquipmentData(InventoryData inventory, int strength, int durability)
        {
            Inventory = inventory ?? new InventoryData();
            Strength = strength;
            Durability = durability;
        }

        private InventoryData Inventory { get; }

        public int Strength { get; set; }

        public int Durability { get; set; }

        public EquipmentData Clone()
        {
            return new EquipmentData(Inventory?.Clone(), Strength, Durability);
        }

        public void Reset()
        {
            Inventory?.Reset();
            Strength = 0;
            Durability = 0;
        }

        public override string ToString()
        {
            return $"EquipmentData(Inventory={Inventory}, Strength={Strength}, Durability={Durability})";
        }
    }
}