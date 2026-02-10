using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Celeritas.Proto;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Login;
using Game.Scripts.Main.Runtime.Network.Packet;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using Google.Protobuf;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using NetworkClosedEventArgs = UnityGameFramework.Runtime.NetworkClosedEventArgs;
using NetworkConnectedEventArgs = UnityGameFramework.Runtime.NetworkConnectedEventArgs;
using NetworkCustomErrorEventArgs = UnityGameFramework.Runtime.NetworkCustomErrorEventArgs;
using NetworkErrorEventArgs = UnityGameFramework.Runtime.NetworkErrorEventArgs;
using NetworkMissHeartBeatEventArgs = UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs;

namespace Game.Scripts.Main.Runtime.Network
{
    public class NetworkChannelHelper : INetworkChannelHelper
    {
        private readonly MemoryStream m_CachedStream = new(1024 * 8);
        private readonly Dictionary<int, Type> m_ServerToClientPacketTypes = new();
        private INetworkChannel m_NetworkChannel;


        /// <summary>
        ///     获取消息包头长度。
        /// </summary>
        public int PacketHeaderLength => sizeof(short) + sizeof(short) + sizeof(int);

        /// <summary>
        ///     初始化网络频道辅助器。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        public void Initialize(INetworkChannel networkChannel)
        {
            m_NetworkChannel = networkChannel;

            // 反射注册包和包处理函数。
            var packetBaseType = typeof(SCPacketBase);
            var packetHandlerBaseType = typeof(PacketHandlerBase);
            var celeritasHandlerBaseType = typeof(ICeleritasHandler);
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                if (type.BaseType == packetBaseType)
                {
                    var packetBase = (PacketBase)Activator.CreateInstance(type);
                    var packetType = GetServerToClientPacketType(packetBase.Id);
                    if (packetType != null)
                    {
                        Log.Warning("Already exist packet type '{0}', check '{1}' or '{2}'?.", packetBase.Id.ToString(),
                            packetType.Name, packetBase.GetType().Name);
                        continue;
                    }

                    m_ServerToClientPacketTypes.Add(packetBase.Id, type);
                }
                else if (type.BaseType == packetHandlerBaseType)
                {
                    var packetHandler = (IPacketHandler)Activator.CreateInstance(type);
                    m_NetworkChannel.RegisterHandler(packetHandler);
                }
            }

            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Subscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Subscribe(NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Subscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Subscribe(NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
        }

        /// <summary>
        ///     关闭并清理网络频道辅助器。
        /// </summary>
        public void Shutdown()
        {
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Unsubscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Unsubscribe(NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Unsubscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Unsubscribe(NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);

            m_NetworkChannel = null;
        }

        /// <summary>
        ///     准备进行连接。
        /// </summary>
        public void PrepareForConnecting()
        {
            m_NetworkChannel.Socket.ReceiveBufferSize = 1024 * 64;
            m_NetworkChannel.Socket.SendBufferSize = 1024 * 64;
        }

        /// <summary>
        ///     发送心跳消息包。
        /// </summary>
        /// <returns>是否发送心跳消息包成功。</returns>
        public bool SendHeartBeat()
        {
            Log.Info("Send Heart Beat");

            var packet = ProtoHelper.GetProto();

            packet.Mutable_ClientPlayer_ClientHeartbeat_Heartbeat();

            m_NetworkChannel.Send(packet);

            return true;
        }

        /// <summary>
        ///     序列化消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要序列化的消息包。</param>
        /// <param name="destination">要序列化的目标流。</param>
        /// <returns>是否序列化成功。</returns>
        public bool Serialize<T>(T packet, Stream destination) where T : GameFramework.Network.Packet
        {
            if (packet is not PacketBase packetImpl)
            {
                Log.Warning("Packet is invalid.");
                return false;
            }

            if (packetImpl.PacketType != PacketType.ClientToServer)
            {
                Log.Warning("Send packet invalid.");
                return false;
            }

            if (packet is not CSCeleritas celeritas)
            {
                Log.Error("Packet '{0}' is not a CSCeleritas.", packet.GetType().FullName);
                return false;
            }

            m_CachedStream.SetLength(0);

            var packetHeader = ReferencePool.Acquire<MessageHeader>();
            packetHeader.headerSize = (short)celeritas.Common.CalculateSize();
            packetHeader.bodySize = celeritas.Celeritas.CalculateSize();

            using (var writer = new BinaryWriter(m_CachedStream, Encoding.UTF8, true))
            {
                packetHeader.WriteTo(writer);
            }

            celeritas.Common.WriteTo(m_CachedStream);
            celeritas.Celeritas.WriteTo(m_CachedStream);

            ReferencePool.Release(packetHeader);
            ReferencePool.Release(packetImpl);

            m_CachedStream.WriteTo(destination);

            return true;
        }

        /// <summary>
        ///     反序列化消息包头。
        /// </summary>
        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            customErrorData = null;
            try
            {
                using var reader = new BinaryReader(source, Encoding.UTF8, true);

                var header = ReferencePool.Acquire<MessageHeader>();

                header.ReadFrom(reader);

                return header;
            }
            catch (Exception ex)
            {
                customErrorData = ex.ToString();
                return null;
            }
        }

        /// <summary>
        ///     反序列化消息包。
        /// </summary>
        /// <param name="packetHeader">消息包头。</param>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包。</returns>
        public GameFramework.Network.Packet DeserializePacket(IPacketHeader packetHeader, Stream source,
            out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            customErrorData = null;

            var messageHeader = packetHeader as MessageHeader;
            if (messageHeader == null)
            {
                Log.Warning("Packet header is not a MessageHeader.");
                return null;
            }


            try
            {
                var commonData = new byte[messageHeader.headerSize];
                var bytesRead = source.Read(commonData, 0, commonData.Length);
                if (bytesRead < commonData.Length)
                {
                    throw new EndOfStreamException(
                        $"Expected to read {commonData.Length} bytes for Common header, but only got {bytesRead}.");
                }

                var common = header.Parser.ParseFrom(commonData);


                var bodyData = new byte[messageHeader.bodySize];
                bytesRead = source.Read(bodyData, 0, bodyData.Length);
                if (bytesRead < bodyData.Length)
                {
                    throw new EndOfStreamException(
                        $"Expected to read {bodyData.Length} bytes for Celeritas body, but only got {bytesRead}.");
                }

                var celeritasBody = celeritas.Parser.ParseFrom(bodyData);


                var packet = ReferencePool.Acquire<SCCeleritas>();
                packet.Common = common;
                packet.Celeritas = celeritasBody;

                ReferencePool.Release(messageHeader);
                return packet;
            }
            catch (Exception e)
            {
                customErrorData = e.ToString();
                Log.Error("Deserialize packet failed: {0}", e.ToString());
                ReferencePool.Release(messageHeader);
                return null;
            }
        }


        private Type GetServerToClientPacketType(int id)
        {
            return m_ServerToClientPacketTypes.GetValueOrDefault(id);
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            var ne = (NetworkConnectedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' connected, local address '{1}', remote address '{2}'.",
                ne.NetworkChannel.Name, ne.NetworkChannel.Socket.LocalEndPoint.ToString(),
                ne.NetworkChannel.Socket.RemoteEndPoint.ToString());

            SendLogin.SendMessage();
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            var ne = (NetworkClosedEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' closed.", ne.NetworkChannel.Name);

            GameEntry.Event.Fire(this, NetworkEventArgs.Create());
        }

        private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
        {
            var ne = (NetworkMissHeartBeatEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' miss heart beat '{1}' times.", ne.NetworkChannel.Name,
                ne.MissCount.ToString());

            if (ne.MissCount < 2)
            {
                return;
            }

            ne.NetworkChannel.Close();
        }

        private void OnNetworkError(object sender, GameEventArgs e)
        {
            var ne = (NetworkErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.",
                ne.NetworkChannel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);

            ne.NetworkChannel.Close();


            GameEntry.Event.Fire(this, NetworkEventArgs.Create());
        }

        private void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            var ne = (NetworkCustomErrorEventArgs)e;
            if (ne.NetworkChannel != m_NetworkChannel)
            {
            }
        }
    }
}