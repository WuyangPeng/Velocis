using ProtoBuf;

namespace Game.Scripts.Main.Runtime.Network
{
    public abstract class PacketBase : GameFramework.Network.Packet, IExtensible
    {
        private IExtension _extensionObject;

        public abstract PacketType PacketType { get; }

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref _extensionObject, createIfMissing);
        }
    }
}