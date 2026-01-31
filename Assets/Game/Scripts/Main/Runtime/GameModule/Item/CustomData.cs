namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class CustomData
    {
        public CustomData()
        {
            Inventory = new InventoryData();
        }

        public CustomData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public CustomData(InventoryData inventory, long expireTime)
        {
            Inventory = inventory ?? new InventoryData();
            ExpireTime = expireTime;
        }

        private InventoryData Inventory { get; }

        public long ExpireTime { get; set; }

        public CustomData Clone()
        {
            return new CustomData(Inventory?.Clone(), ExpireTime);
        }

        public void Reset()
        {
            Inventory?.Reset();
            ExpireTime = 0;
        }

        public override string ToString()
        {
            return $"CustomData(Inventory={Inventory}, ExpireTime={ExpireTime})";
        }
    }
}