using Game.Scripts.Main.Runtime.UIItem.UICreate;
using Game.Scripts.Main.Runtime.UIItem.UIHome;

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
