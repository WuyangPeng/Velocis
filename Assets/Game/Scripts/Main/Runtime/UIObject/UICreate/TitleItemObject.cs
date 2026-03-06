using Game.Scripts.Main.Runtime.UIItem.UICreate;

namespace Game.Scripts.Main.Runtime.UIObject.UICreate
{
    public class TitleItemObject : ItemObjectBase<TitleItem>
    {
        public static TitleItemObject Create(TitleItem item)
        {
            return ItemObjectBase<TitleItem>.Create<TitleItemObject>(item);
        }
    }
}
