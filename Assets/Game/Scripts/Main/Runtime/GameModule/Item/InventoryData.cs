namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class InventoryData
    {
        public InventoryData()
        {
        }

        public InventoryData(long itemId, int templateId, long count, int position)
        {
            ItemId = itemId;
            TemplateId = templateId;
            Count = count;
            Position = position;
        }

        public long ItemId { get; set; }

        public int TemplateId { get; set; }

        public long Count { get; set; }

        public int Position { get; set; }

        public InventoryData Clone()
        {
            return new InventoryData(ItemId, TemplateId, Count, Position);
        }

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