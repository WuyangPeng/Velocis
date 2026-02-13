namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class AvatarData
    {
        public AvatarData()
        {
            Inventory = new InventoryData();
        }

        public AvatarData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public InventoryData Inventory { get; set; }

        public AvatarData Clone()
        {
            return new AvatarData(Inventory?.Clone());
        }

        public void Reset()
        {
            Inventory?.Reset();
        }

        public override string ToString()
        {
            return $"AvatarData(Inventory={Inventory})";
        }
    }
}