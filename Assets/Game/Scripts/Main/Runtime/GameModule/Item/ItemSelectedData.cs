using Celeritas.Config;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class ItemSelectedData
    {
        public ItemSelectedData()
        {
        }

        public ItemSelectedData(long id, item_type itemType, item_selected_child_type childType, long operationId, int parameter, long selectedId)
        {
            Id = id;
            ItemType = itemType;
            ChildType = childType;
            OperationId = operationId;
            Parameter = parameter;
            SelectedId = selectedId;
        }

        public long Id { get; private set; }

        public item_type ItemType { get; set; }

        private item_selected_child_type ChildType { get; set; }

        private long OperationId { get; set; }

        private int Parameter { get; set; }

        private long SelectedId { get; set; }

        public ItemSelectedData Clone()
        {
            return new ItemSelectedData(Id, ItemType, ChildType, OperationId, Parameter, SelectedId);
        }

        public void Reset()
        {
            Id = 0;
            ItemType = item_type.none;
            ChildType = item_selected_child_type.none;
            OperationId = 0;
            Parameter = 0;
            SelectedId = 0;
        }

        public override string ToString()
        {
            return $"ItemSelectedData(Id={Id}, ItemType={ItemType}, ChildType={ChildType}, OperationId={OperationId}, Parameter={Parameter}, SelectedId={SelectedId})";
        }
    }
}