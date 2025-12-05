using Game.Scripts.Main.Runtime.UIItem.UICreate;

namespace Game.Scripts.Main.Runtime.UIObject.UICreate
{
    public class AvatarItemObject : ItemObjectBase<AvatarItem>
    {
        public static AvatarItemObject Create(AvatarItem item)
        {
            return ItemObjectBase<AvatarItem>.Create<AvatarItemObject>(item);
        }
    }
}