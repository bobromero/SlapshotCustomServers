using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Reflection;
using System.Net;
using System.Net.Sockets;

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
            if (clientInstance != null)
            {
                clientInstance.Tick();
            }
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
            client.Start();
            client.Connect("localhost", 9050, "Slapshot");
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
            Melon<ClientMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
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
    }
}
