using Celeritas.Proto.Common;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public static class InventoryDataExtensions
    {
        public static InventoryData ToInventoryData(this inventory_data data)
        {
            if (data == null)
            {
                return null;
            }

            var inventoryData = new InventoryData
            {
                ItemId = data.ItemId,
                TemplateId = data.TemplateId,
                Count = data.Count,
                Position = data.Position
            };
            
            return inventoryData;
        }
    }
}