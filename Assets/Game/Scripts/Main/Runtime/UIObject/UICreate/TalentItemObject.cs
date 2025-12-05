using Game.Scripts.Main.Runtime.UIItem.UICreate;

namespace Game.Scripts.Main.Runtime.UIObject.UICreate
{
    public class TalentItemObject : ItemObjectBase<TalentItem>
    {
        public static TalentItemObject Create(TalentItem item)
        {
            return ItemObjectBase<TalentItem>.Create<TalentItemObject>(item);
        }
    }
}