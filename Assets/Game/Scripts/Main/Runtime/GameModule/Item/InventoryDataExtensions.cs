using Celeritas.Proto.Client;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public static class InventoryDataExtensions
    {
        public static InventoryData ToInventoryData(this inventory_data src)
        {
            if (src == null)
            {
                return null;
            }

            var inv = new InventoryData();
            inv.ItemId = src.ItemId;
            inv.TemplateId = src.TemplateId;
            inv.Count = src.Count;
            inv.Position = src.Position;
            return inv;
        }
    }
}