namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class InventoryData
    {
        private long ItemId { get; set; }

        private int TemplateId { get; set; }

        private long Count { get; set; }

        private int Position { get; set; }

        public InventoryData() { }

        public InventoryData(long itemId, int templateId, long count, int position)
        {
            ItemId = itemId;
            TemplateId = templateId;
            Count = count;
            Position = position;
        }

        public InventoryData Clone() => new InventoryData(ItemId, TemplateId, Count, Position);

        public void Reset()
        {
            ItemId = 0;
            TemplateId = 0;
            Count = 0;
            Position = 0;
        }

        public override string ToString()
        {
            return $"InventoryData(ItemId={ItemId}, TemplateId={TemplateId}, Count={Count}, Position={Position})";
        }
    }
}