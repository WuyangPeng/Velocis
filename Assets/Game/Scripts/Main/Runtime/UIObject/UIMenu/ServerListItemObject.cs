using Game.Scripts.Main.Runtime.UIItem.UIMenu;

namespace Game.Scripts.Main.Runtime.UIObject.UIMenu
{
    public class ServerListItemObject : ItemObjectBase<ServerListItem>
    {
        public static ServerListItemObject Create(ServerListItem item)
        {
            return Create<ServerListItemObject>(item);
        }
    }
}