// 创建时间：2026-07-06
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Celeritas.Proto;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.User;
using Game.Scripts.Hotfix.HotfixCommon.Network.Model;
using Game.Scripts.Hotfix.HotfixCommon.Network.Packet;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Network;
using Game.Scripts.Main.Runtime.Network.Packet;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using Google.Protobuf;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using NetworkClosedEventArgs = UnityGameFramework.Runtime.NetworkClosedEventArgs;
using NetworkConnectedEventArgs = UnityGameFramework.Runtime.NetworkConnectedEventArgs;
using NetworkCustomErrorEventArgs = UnityGameFramework.Runtime.NetworkCustomErrorEventArgs;
using NetworkErrorEventArgs = UnityGameFramework.Runtime.NetworkErrorEventArgs;
using NetworkMissHeartBeatEventArgs = UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs;

namespace Game.Scripts.Hotfix.HotfixCommon.Network
{
    /// <summary>
    ///     网络频道辅助器。
    /// </summary>
    public class NetworkChannelHelper : INetworkChannelHelper
    {
        /// <summary>
        ///     用于缓存序列化数据的内存流。
        /// </summary>
        private readonly MemoryStream _cachedStream = new(1024 * 8);

        /// <summary>
        ///     服务器发送给客户端的协议包 ID 到包类型的映射字典。
        /// </summary>
        private readonly Dictionary<int, Type> _serverToClientPacketTypes = new();

        /// <summary>
        ///     关联的网络频道。
        /// </summary>
        private INetworkChannel _networkChannel;

        /// <summary>
        ///     登录消息的 RPC 序列号。
        /// </summary>
        public static int LoginRpcId { get; private set; }


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
            _networkChannel = networkChannel;

            RegisterPacketsAndHandlers();
            SubscribeEvents();
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

            _networkChannel = null;
        }

        /// <summary>
        ///     准备进行连接。
        /// </summary>
        public void PrepareForConnecting()
        {
            _networkChannel.Socket.ReceiveBufferSize = 1024 * 64;
            _networkChannel.Socket.SendBufferSize = 1024 * 64;
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

            _networkChannel.Send(packet);

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
            var validationResult = ValidatePacket(packet);
            return validationResult.IsValid && TrySerializeAndRelease(validationResult, destination);
        }

        /// <summary>
        ///     反序列化消息包头。
        /// </summary>
        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            customErrorData = null;
            try
            {
                return ReadPacketHeader(source);
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
        public GameFramework.Network.Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            if (packetHeader is MessageHeader messageHeader)
            {
                return TryParseAndRelease(messageHeader, source, out customErrorData);
            }

            customErrorData = null;
            Log.Warning("Packet header is not a MessageHeader.");
            return null;
        }

        /// <summary>
        ///     从源数据流读取并生成网络消息包头。
        /// </summary>
        private static MessageHeader ReadPacketHeader(Stream source)
        {
            using var reader = new BinaryReader(source, Encoding.UTF8, true);
            var header = new MessageHeader();
            header.ReadFrom(reader);
            return header;
        }

        /// <summary>
        ///     尝试读取解析数据包并释放包头引用。
        /// </summary>
        private static SCCeleritas TryParseAndRelease(MessageHeader messageHeader, Stream source, out object customErrorData)
        {
            customErrorData = null;
            try
            {
                return ReadAndParsePacket(messageHeader, source);
            }
            catch (Exception e)
            {
                customErrorData = e.ToString();
                Log.Error("Deserialize packet failed: {0}", e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     从数据流中读取并解析完整的协议包。
        /// </summary>
        private static SCCeleritas ReadAndParsePacket(MessageHeader messageHeader, Stream source)
        {
            var commonData = ReadBytes(source, messageHeader.HeaderSize, "Common header");
            var common = header.Parser.ParseFrom(commonData);

            var bodyData = ReadBytes(source, messageHeader.BodySize, "Celeritas body");
            var celeritasBody = celeritas.Parser.ParseFrom(bodyData);

            var packet = ReferencePool.Acquire<SCCeleritas>();
            packet.Common = common;
            packet.Celeritas = celeritasBody;

            return packet;
        }

        /// <summary>
        ///     从数据流中读取指定大小的字节数据。
        /// </summary>
        private static byte[] ReadBytes(Stream stream, int size, string description)
        {
            var buffer = new byte[size];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            return bytesRead < buffer.Length ? throw new EndOfStreamException($"Expected to read {buffer.Length} bytes for {description}, but only got {bytesRead}.") : buffer;
        }

        /// <summary>
        ///     尝试序列化协议包并安全释放引用池对象。
        /// </summary>
        private bool TrySerializeAndRelease(PacketValidationResult validationResult, Stream destination)
        {
            try
            {
                SerializePacket(validationResult.Celeritas);
                _cachedStream.WriteTo(destination);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Serialize packet failed: {0}", ex.ToString());
                return false;
            }
            finally
            {
                ReferencePool.Release(validationResult.PacketImpl);
            }
        }

        /// <summary>
        ///     验证待发送的消息包是否合法。
        /// </summary>
        private static PacketValidationResult ValidatePacket<T>(T packet) where T : GameFramework.Network.Packet
        {
            if (packet is not PacketBase impl)
            {
                Log.Warning("Packet is invalid.");
                return PacketValidationResult.Fail();
            }

            if (impl.PacketType != PacketType.ClientToServer)
            {
                Log.Warning("Send packet invalid.");
                return PacketValidationResult.Fail();
            }

            if (packet is CSCeleritas celeritasPacket)
            {
                return PacketValidationResult.Success(impl, celeritasPacket);
            }

            Log.Error("Packet '{0}' is not a CSCeleritas.", packet.GetType().FullName);
            return PacketValidationResult.Fail();
        }

        /// <summary>
        ///     将 CSCeleritas 消息包序列化到内部缓存流。
        /// </summary>
        private void SerializePacket(CSCeleritas celeritas)
        {
            _cachedStream.SetLength(0);

            var packetHeader = new MessageHeader();
            WritePacketData(packetHeader, celeritas);
        }

        /// <summary>
        ///     将协议头和消息体数据写入到内部缓存流。
        /// </summary>
        private void WritePacketData(MessageHeader packetHeader, CSCeleritas celeritas)
        {
            packetHeader.HeaderSize = (short)celeritas.Common.CalculateSize();
            packetHeader.BodySize = celeritas.Celeritas.CalculateSize();

            using (var writer = new BinaryWriter(_cachedStream, Encoding.UTF8, true))
            {
                packetHeader.WriteTo(writer);
            }

            celeritas.Common.WriteTo(_cachedStream);
            celeritas.Celeritas.WriteTo(_cachedStream);
        }

        /// <summary>
        ///     通过反射注册协议包和协议包处理器。
        /// </summary>
        private void RegisterPacketsAndHandlers()
        {
            var packetBaseType = typeof(SCPacketBase);
            var packetHandlerBaseType = typeof(PacketHandlerBase);
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                RegisterType(type, packetBaseType, packetHandlerBaseType);
            }
        }

        /// <summary>
        ///     注册单个类型（协议包或协议包处理器）。
        /// </summary>
        /// <param name="type">要注册的类型。</param>
        /// <param name="packetBaseType">协议包基类类型。</param>
        /// <param name="packetHandlerBaseType">协议包处理器基类类型。</param>
        private void RegisterType(Type type, Type packetBaseType, Type packetHandlerBaseType)
        {
            if (!type.IsClass || type.IsAbstract)
            {
                return;
            }

            if (type.BaseType == packetBaseType)
            {
                var packetBase = (PacketBase)Activator.CreateInstance(type);
                var packetType = GetServerToClientPacketType(packetBase.Id);
                if (packetType != null)
                {
                    Log.Warning("Already exist packet type '{0}', check '{1}' or '{2}'?.",
                        packetBase.Id.ToString(),
                        packetType.Name,
                        packetBase.GetType().Name);
                    return;
                }

                _serverToClientPacketTypes.Add(packetBase.Id, type);
            }
            else if (type.BaseType == packetHandlerBaseType)
            {
                var packetHandler = (IPacketHandler)Activator.CreateInstance(type);
                _networkChannel.RegisterHandler(packetHandler);
            }
        }

        /// <summary>
        ///     订阅网络事件。
        /// </summary>
        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Event.Subscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
            GameEntry.Event.Subscribe(NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
            GameEntry.Event.Subscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            GameEntry.Event.Subscribe(NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
        }


        /// <summary>
        ///     根据协议包 ID 获取对应的服务器到客户端的协议包类型。
        /// </summary>
        /// <param name="id">协议包 ID。</param>
        /// <returns>协议包类型。</returns>
        private Type GetServerToClientPacketType(int id)
        {
            return _serverToClientPacketTypes.GetValueOrDefault(id);
        }

        /// <summary>
        ///     网络频道连接成功的回调函数。
        /// </summary>
        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            var ne = (NetworkConnectedEventArgs)e;
            if (ne.NetworkChannel != _networkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' connected, local address '{1}', remote address '{2}'.",
                ne.NetworkChannel.Name,
                ne.NetworkChannel.Socket.LocalEndPoint.ToString(),
                ne.NetworkChannel.Socket.RemoteEndPoint.ToString());

            SendLoginMessage();
        }

        /// <summary>
        ///     发送登录消息包。
        /// </summary>
        private void SendLoginMessage()
        {
            var packet = ProtoHelper.GetProto();
            LoginRpcId = packet.Common.Client.Rpc;
            var login = packet.Mutable_ClientPlayer_ClientLogin_Login();

            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            login.Token = accountModule.GetToken();
            login.GameServerId = accountModule.GetCurrentGameServerId();
            login.DeviceId = SystemInfo.deviceUniqueIdentifier;
            login.AppVersion = GameEntry.Account.appVersion;

            _networkChannel.Send(packet);
        }

        /// <summary>
        ///     网络频道关闭的回调函数。
        /// </summary>
        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            var ne = (NetworkClosedEventArgs)e;
            if (ne.NetworkChannel != _networkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' closed.", ne.NetworkChannel.Name);

            GameEntry.Event.Fire(this, NetworkCloseEventArgs.Create());
        }

        /// <summary>
        ///     丢失心跳包的回调函数。
        /// </summary>
        private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
        {
            var ne = (NetworkMissHeartBeatEventArgs)e;
            if (ne.NetworkChannel != _networkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' miss heart beat '{1}' times.",
                ne.NetworkChannel.Name,
                ne.MissCount.ToString());

            if (ne.MissCount < 2)
            {
                return;
            }

            ne.NetworkChannel.Close();
        }

        /// <summary>
        ///     网络通道错误的回调函数。
        /// </summary>
        private void OnNetworkError(object sender, GameEventArgs e)
        {
            var ne = (NetworkErrorEventArgs)e;
            if (ne.NetworkChannel != _networkChannel)
            {
                return;
            }

            Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.",
                ne.NetworkChannel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);

            ne.NetworkChannel.Close();

            GameEntry.Event.Fire(this, NetworkCloseEventArgs.Create());
        }

        /// <summary>
        ///     自定义网络错误的回调函数。
        /// </summary>
        private void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            var ne = (NetworkCustomErrorEventArgs)e;
            if (ne.NetworkChannel != _networkChannel)
            {
                return;
            }

            Log.Error("Network channel '{0}' custom error, error data: {1}",
                ne.NetworkChannel.Name,
                ne.CustomErrorData != null ? ne.CustomErrorData.ToString() : "Unknown");
        }
    }
}