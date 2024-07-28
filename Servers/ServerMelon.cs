using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using Il2Cpp;
using UnityEngine;
using Packets;

namespace SlapshotCustomServers
{
    public class ServerMelon : MelonMod
    {
        private CustomServer serverInstance;

        public override void OnInitializeMelon()
        {
            Melon<ServerMelon>.Logger.Msg("Initialized");
            serverInstance = new CustomServer();
            serverInstance.Start();
            Melon<ServerMelon>.Logger.Msg("Server started, starting Slapshot server");
            serverInstance.SetupSlapServer();

        }
        public override void OnUpdate()
        {
            if (serverInstance != null)
            {
                serverInstance.Tick();
            }
        }
    }

    

    public class CustomServer : INetEventListener
    {
        private NetManager server;
        public Server SlapshotServer;

        private NetDataWriter writer;
        private NetPacketProcessor packetProcessor;

        private ServerPacketHandler slapPacketHandler;

        private Dictionary<int, NetPeer> Peers = new Dictionary<int, NetPeer>();
        private Dictionary<int, ServerConnection> connections = new Dictionary<int, ServerConnection>();

        public void Start()
        {
            server = new NetManager(this)
            {
                AutoRecycle = true
            };
            Melon<ServerMelon>.Logger.Msg("Starting server");
            server.Start(9050);
            writer = new NetDataWriter();
            slapPacketHandler = new ServerPacketHandler(SlapshotServer);
            packetProcessor = new NetPacketProcessor();
            //packetProcessor.RegisterNestedType<CustomJoinRequestPacket>();
            packetProcessor.SubscribeReusable<JoinRequestPacket>(OnReceivePacket);
        }

        public void Tick()
        {
            server.PollEvents();
        }

        public void SetupSlapServer()
        {
            SlapshotServer = new GameObject().AddComponent<Server>();
            GameObject.DontDestroyOnLoad(SlapshotServer.gameObject);
            Server.Instance = SlapshotServer;
            Melon<ServerMelon>.Logger.Msg("Slapshot server created");
            //SlapshotServer;
        }

        public void SendPacket<T>(T packet, NetPeer peer, DeliveryMethod deliveryMethod) where T : class, new()
        {
            if (peer != null)
            {
                writer.Reset();
                packetProcessor.Write(writer, packet);
                peer.Send(writer, deliveryMethod);
            }
        }

        public void OnReceivePacket(BasePacket packet)
        {
            
            Melon<ServerMelon>.Logger.Msg("packet Received");
            //Type type;
            switch (packet.PacketType)
            {
                case PacketType.JoinRequest:
                    var newpacket = (JoinRequestPacket)packet;
                    slapPacketHandler.OnPacketReceived(connections.FirstOrDefault().Value, (JoinRequestPacket)packet);
                    break;
                case PacketType.PlayerLeaveEvent:
                    break;
                case PacketType.PlayerJoinEvent:
                    //packet = (PlayerJoinEventPacket)packet;
                    break;
                case PacketType.InitialMatchState:
                    break;
                case PacketType.WorldState:
                    break;
                case PacketType.MatchState:
                    break;
                case PacketType.PlayerStats:
                    break;
                case PacketType.ChatMessageEvent:
                    break;
                case PacketType.GoalScored:
                    break;
                case PacketType.SaveMade:
                    break;
                case PacketType.Pong:
                    break;
                case PacketType.StatsReport:
                    break;
                case PacketType.PodiumState:
                    break;
                case PacketType.PuckAddEvent:
                    break;
                case PacketType.PuckRemoveEvent:
                    break;
                case PacketType.SumoState:
                    break;
                case PacketType.PlayerInput:
                    break;
                case PacketType.SkipReplay:
                    break;
                case PacketType.Ping:
                    break;
                case PacketType.ForfeitVote:
                    break;
                case PacketType.StartForfeit:
                    break;
                default:
                    break;
            }
            //new ServerConnection(SlapshotServer, peer)
            //slapPacketHandler.OnPacketReceived(, (JoinRequestPacket)packet);
            Melon<ServerMelon>.Logger.Msg("packet Handeled");
        }


        #region INetEventListener
        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (server.ConnectedPeersCount < 10)
            {
                request.AcceptIfKey("Slapshot");
            }
            else
            {
                request.Reject();
            }
        }
        
        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Melon<ServerMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            Melon<ServerMelon>.Logger.Msg(peer.Id+"'s latency: " + latency);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            Melon<ServerMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            packetProcessor.ReadAllPackets(reader,peer);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Melon<ServerMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Melon<ServerMelon>.Logger.Msg(peer.Id + " Connected to server!");
            Peers.Add(peer.Id, peer);
            connections.Add(peer.Id, new ServerConnection(SlapshotServer, (Il2CppLiteNetLib.NetPeer)(Il2CppSystem.Object)(System.Object)peer));
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Melon<ServerMelon>.Logger.Msg(peer.Id + " disconnected, Reason: " + disconnectInfo.Reason);
        }
    }
    #endregion
}
