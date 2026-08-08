// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Config;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    /// 物品已选择/装配的数据实体类。
    /// </summary>
    public class ItemSelectedData
    {
        /// <summary>
        /// 初始化 <see cref="ItemSelectedData"/> 类的新实例。
        /// </summary>
        public ItemSelectedData()
        {
        }

        /// <summary>
        /// 初始化 <see cref="ItemSelectedData"/> 类的新实例。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        /// <param name="itemType">物品类型。</param>
        /// <param name="childType">子选择类型。</param>
        /// <param name="operationId">操作类型ID。</param>
        /// <param name="parameter">参数。</param>
        /// <param name="selectedId">被选择的目标道具唯一ID。</param>
        public ItemSelectedData(long id, item_type itemType, item_selected_child_type childType, long operationId, int parameter, long selectedId)
        {
            Id = id;
            ItemType = itemType;
            ChildType = childType;
            OperationId = operationId;
            Parameter = parameter;
            SelectedId = selectedId;
        }

        /// <summary>
        /// 获取选择记录的唯一ID。
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 获取或设置选中的物品大类类型。
        /// </summary>
        public item_type ItemType { get; set; }

        private item_selected_child_type ChildType { get; set; }

        private long OperationId { get; set; }

        private int Parameter { get; set; }

        /// <summary>
        /// 获取或设置选中的道具唯一实例ID。
        /// </summary>
        public long SelectedId { get; set; }

        /// <summary>
        /// 克隆当前装配数据。
        /// </summary>
        /// <returns>新的 ItemSelectedData 实例。</returns>
        public ItemSelectedData Clone()
        {
            return new ItemSelectedData(Id, ItemType, ChildType, OperationId, Parameter, SelectedId);
        }

        /// <summary>
        /// 重置装配选择数据为默认值。
        /// </summary>
        public void Reset()
        {
            Id = 0;
            ItemType = item_type.none;
            ChildType = item_selected_child_type.none;
            OperationId = 0;
            Parameter = 0;
            SelectedId = 0;
        }

        /// <summary>
        /// 返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"ItemSelectedData(Id={Id}, ItemType={ItemType}, ChildType={ChildType}, OperationId={OperationId}, Parameter={Parameter}, SelectedId={SelectedId})";
        }
    }
}