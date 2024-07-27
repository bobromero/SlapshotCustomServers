using MelonLoader;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Il2Cpp;
using UnityEngine;


namespace SlapshotCustomServers
{
    public class MainMelon : MelonMod
    {
        private CustomServer serverInstance;


        public override void OnInitializeMelon()
        {
            Melon<MainMelon>.Logger.Msg("Initialized");

            serverInstance = new CustomServer();
            serverInstance.Start();
            Melon<MainMelon>.Logger.Msg("Custom Server started, making slap server");

            //make slap server
            serverInstance.SlapServer = new GameObject().AddComponent<Server>();
            
            
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
        public Server SlapServer;

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
            Melon<MainMelon>.Logger.Msg("Connection request");
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
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            Melon<MainMelon>.Logger.Msg(peer.Id + "'s ping: " + latency);
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
            Melon<MainMelon>.Logger.Msg(peer.Address + " connected!");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Melon<MainMelon>.Logger.Msg("In" + MethodBase.GetCurrentMethod().Name);
            throw new NotImplementedException();
        }
    }
    
}
