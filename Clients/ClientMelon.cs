using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using Il2Cpp;
using UnityEngine;
using Packets;

namespace SlapshotCustomClients
{
    public class ClientMelon : MelonMod
    {
        private CustomClient clientInstance;

        public override void OnInitializeMelon()
        {
            Melon<ClientMelon>.Logger.Msg("Initialized");
            clientInstance = new CustomClient();
            clientInstance.Connect();
        }
        public override void OnUpdate()
        {
            if (clientInstance == null)
            {
                return;
            }
            clientInstance.Tick();
            if (Input.GetKeyDown(KeyCode.P))
            {
                clientInstance.JoinGame();
            }
        }
    }

    public class CustomClient : INetEventListener
    {
        private NetManager client;
        private NetPeer server;

        private NetDataWriter writer;
        private NetPacketProcessor packetProcessor;

        private ClientPacketHandler slapPacketHandler;

        public void Connect()
        {
            client = new NetManager(this)
            {
                AutoRecycle = true
            };
            client.Start();
            client.Connect("localhost", 9050, "Slapshot");
            writer = new NetDataWriter();
            slapPacketHandler = new ClientPacketHandler(Client.Instance);
            packetProcessor = new NetPacketProcessor();
            packetProcessor.SubscribeReusable<BasePacket>(OnReceivePacket);
        }

        public void Tick()
        {
            if (client != null)
            {
                client.PollEvents();
            }
        }

        public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
        {
            if (server != null)
            {
                writer.Reset();
                packetProcessor.Write(writer, packet);
                server.Send(writer, deliveryMethod);
            }
        }

        public void JoinGame()
        {
            SendPacket(new JoinRequestPacket
            {
                JerseyNumber = "69",
                Username = "Rob",
                UUID = "80822",
                RightHandedness = true
            }, DeliveryMethod.ReliableOrdered);
        }

        public void OnReceivePacket(BasePacket packet)
        {
            Melon<ClientMelon>.Logger.Msg("packet Received");
            //Type type;
            switch (packet.PacketType)
            {
                case PacketType.JoinRequest:
                    //packet = (JoinRequestPacket)packet;
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
            slapPacketHandler.OnPacketReceived((PlayerJoinEventPacket)packet);
            Melon<ClientMelon>.Logger.Msg("packet Handeled");
        }


        #region INetEventListener
        public void OnConnectionRequest(ConnectionRequest request)
        {
            Melon<ClientMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Melon<ClientMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            Melon<ClientMelon>.Logger.Msg("latency to server: " + latency);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            packetProcessor.ReadAllPackets(reader);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Melon<ClientMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Melon<ClientMelon>.Logger.Msg("Connected to server");
            server = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Melon<ClientMelon>.Logger.Msg("Server disconnected");
        }
        #endregion
    }

}
