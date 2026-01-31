namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class EquipmentData
    {
        private InventoryData Inventory { get; set; }

        private int Strength { get; set; }

        private int Durability { get; set; }

        public EquipmentData() { Inventory = new InventoryData(); }

        public EquipmentData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public EquipmentData(InventoryData inventory, int strength, int durability)
        {
            Inventory = inventory ?? new InventoryData();
            Strength = strength;
            Durability = durability;
        }

        public EquipmentData Clone() => new EquipmentData(Inventory?.Clone(), Strength, Durability);

        public void Reset() { Inventory?.Reset(); Strength = 0; Durability = 0; }

        public override string ToString() => $"EquipmentData(Inventory={Inventory}, Strength={Strength}, Durability={Durability})";
    }
}
