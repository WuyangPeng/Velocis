namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class CustomData
    {
        private InventoryData Inventory { get; set; }

        private long ExpireTime { get; set; }

        public CustomData() { Inventory = new InventoryData(); }

        public CustomData(InventoryData inventory) { Inventory = inventory ?? new InventoryData(); }

        public CustomData(InventoryData inventory, long expireTime)
        {
            Inventory = inventory ?? new InventoryData();
            ExpireTime = expireTime;
        }

        public CustomData Clone() => new CustomData(Inventory?.Clone(), ExpireTime);

        public void Reset() { Inventory?.Reset(); ExpireTime = 0; }

        public override string ToString() => $"CustomData(Inventory={Inventory}, ExpireTime={ExpireTime})";
    }
}
