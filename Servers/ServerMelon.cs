using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using Il2Cpp;
using UnityEngine;

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

        public void Start()
        {
            server = new NetManager(this)
            {
                AutoRecycle = true
            };
            Melon<ServerMelon>.Logger.Msg("Starting server");
            server.Start(9050);
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
            throw new NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Melon<ServerMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Melon<ServerMelon>.Logger.Msg(peer.Id + " Connected to server!");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Melon<ServerMelon>.Logger.Msg(peer.Id + " disconnected, Reason: " + disconnectInfo.Reason);
        }
    }
}
