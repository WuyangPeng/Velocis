using System.IO;
using System.Net;
using GameFramework;
using GameFramework.Network;

namespace Game.Scripts.Main.Runtime.Network
{
    public class MessageHeader : IReference, IPacketHeader
    {
        public int bodySize;
        public short headerSize;
        public short headerType;

        public int PacketLength => headerSize + bodySize;

        public void Clear()
        {
            bodySize = 0;
            headerSize = 0;
            headerType = 0;
        }

        public bool IsEffective()
        {
            return headerSize <= 0xFF && bodySize <= 16 * 1024 * 1024;
        }

        /// <summary>
        ///     将当前 MessageHeader 的内容序列化到流中。
        /// </summary>
        /// <param name="writer">二进制写入器</param>
        public void WriteTo(BinaryWriter writer)
        {
            // 在写入前，将字段从主机字节序转换成网络字节序 (Big-Endian)
            var headerTypeNet = IPAddress.HostToNetworkOrder(headerType);
            var headerSizeNet = IPAddress.HostToNetworkOrder(headerSize);
            var bodySizeNet = IPAddress.HostToNetworkOrder(bodySize);

            writer.Write(headerTypeNet);
            writer.Write(headerSizeNet);
            writer.Write(bodySizeNet);
        }

        /// <summary>
        ///     从流中读取数据，填充当前 MessageHeader。
        /// </summary>
        /// <param name="reader">二进制读取器</param>
        public void ReadFrom(BinaryReader reader)
        {
            // 从流中读取网络字节序的数据
            var headerTypeNet = reader.ReadInt16();
            var headerSizeNet = reader.ReadInt16();
            var bodySizeNet = reader.ReadInt32();

            // 将读取到的数据转换回主机字节序
            headerType = IPAddress.NetworkToHostOrder(headerTypeNet);
            headerSize = IPAddress.NetworkToHostOrder(headerSizeNet);
            bodySize = IPAddress.NetworkToHostOrder(bodySizeNet);
        }
    }
}