using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;


namespace SlapshotCustomServers
{
    public class MainMelon : MelonMod
    {
        public MelonPreferences_Category CustomServers;
        public MelonPreferences_Entry<bool> IsServer;

        public CustomServer serverInstance;
        public CustomClient clientInstance;


        public override void OnInitializeMelon()
        {
            CustomServers = MelonPreferences.CreateCategory("CustomServers", "Custom Servers");
            IsServer = CustomServers.CreateEntry("IsServer", false);

            Melon<MainMelon>.Logger.Msg("Initialized");

            if (IsServer.Value)
            {
                serverInstance = new CustomServer();
                serverInstance.Start();
                Melon<MainMelon>.Logger.Msg("Server started");
            }
            else
            {
                clientInstance = new CustomClient();
                clientInstance.Connect();
                Melon<MainMelon>.Logger.Msg("Client started");
            }
            
        }
        public override void OnUpdate()
        {
            if (serverInstance != null)
            {
                serverInstance.Tick();
            }
            if (clientInstance != null)
            {
                clientInstance.Tick();
            }
        }
    }
    public class CustomServer : INetEventListener
    {
        private NetManager server;

        public void Start()
        {
            server = new NetManager(this)
            {
                AutoRecycle = true
            };
            server.Start(9050);
        }

        public void Tick()
        {
            if (server != null)
            {
                server.PollEvents();
            }
        }


        public void OnConnectionRequest(ConnectionRequest request)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            throw new NotImplementedException();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            throw new NotImplementedException();
        }
    }
    public class CustomClient : INetEventListener
    {
        private NetManager client;
        private NetPeer server;

        public void Connect()
        {
            client = new NetManager(this)
            {
                AutoRecycle = true
            };
        }

        public void Tick()
        {
            if (client != null)
            {
                client.PollEvents();
            }
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            throw new NotImplementedException();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            throw new NotImplementedException();
        }
    }
}
