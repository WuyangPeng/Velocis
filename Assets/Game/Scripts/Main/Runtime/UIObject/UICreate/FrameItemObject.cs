using Game.Scripts.Main.Runtime.UIItem.UICreate;

namespace Game.Scripts.Main.Runtime.UIObject.UICreate
{
    public class FrameItemObject : ItemObjectBase<FrameItem>
    {
        public static FrameItemObject Create(FrameItem item)
        {
            return ItemObjectBase<FrameItem>.Create<FrameItemObject>(item);
        }
    }
}
