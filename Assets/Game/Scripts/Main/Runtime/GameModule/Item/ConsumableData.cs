namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class ConsumableData
    {
        public ConsumableData()
        {
            Inventory = new InventoryData();
        }

        public ConsumableData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        public ConsumableData(InventoryData inventory, long expireTime)
        {
            Inventory = inventory ?? new InventoryData();
            ExpireTime = expireTime;
        }

        private InventoryData Inventory { get; }
        public long ExpireTime { get; set; }

        public ConsumableData Clone()
        {
            return new ConsumableData(Inventory?.Clone(), ExpireTime);
        }

        public void Reset()
        {
            Inventory?.Reset();
            ExpireTime = 0;
        }

        public override string ToString()
        {
            return $"ConsumableData(Inventory={Inventory}, ExpireTime={ExpireTime})";
        }
    }
}