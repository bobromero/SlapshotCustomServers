using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Reflection;
using System.Net;
using System.Net.Sockets;

namespace SlapshotCustomClients
{
    public class MainMelon : MelonMod
    {
        private CustomClient clientInstance;

        public override void OnInitializeMelon()
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
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            Melon<MainMelon>.Logger.Msg("latency to server: " + latency);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Melon<MainMelon>.Logger.Msg("Connected to server");
            server = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }
    }
}
