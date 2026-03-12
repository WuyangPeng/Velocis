using System.IO;
using System.Net;
using GameFramework;
using GameFramework.Network;

namespace Game.Scripts.Main.Runtime.Network
{
    public class MessageHeader : IReference, IPacketHeader
    {
        public int BodySize { get; set; }
        public short HeaderSize { get; set; }
        private short HeaderType { get; set; }

        public int PacketLength => HeaderSize + BodySize;

        public void Clear()
        {
            BodySize = 0;
            HeaderSize = 0;
            HeaderType = 0;
        }

        public bool IsEffective()
        {
            return HeaderSize <= 0xFF && BodySize <= 16 * 1024 * 1024;
        }

        /// <summary>
        ///     将当前 MessageHeader 的内容序列化到流中。
        /// </summary>
        /// <param name="writer">二进制写入器</param>
        public void WriteTo(BinaryWriter writer)
        {
            // 在写入前，将字段从主机字节序转换成网络字节序 (Big-Endian)
            var headerTypeNet = IPAddress.HostToNetworkOrder(HeaderType);
            var headerSizeNet = IPAddress.HostToNetworkOrder(HeaderSize);
            var bodySizeNet = IPAddress.HostToNetworkOrder(BodySize);

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
            HeaderType = IPAddress.NetworkToHostOrder(headerTypeNet);
            HeaderSize = IPAddress.NetworkToHostOrder(headerSizeNet);
            BodySize = IPAddress.NetworkToHostOrder(bodySizeNet);
        }
    }
}