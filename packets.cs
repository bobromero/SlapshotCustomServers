using System;

public static class packets
{
    public struct CustomJoinRequestPacket : INetSerializable
    {
        public PacketType PacketType => PacketType.JoinRequest;
        public string JerseyNumber;
        public bool RightHandedness;
        public string Username;
        public string UUID;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(JerseyNumber);
            writer.Put(RightHandedness);
            writer.Put(Username);
            writer.Put(UUID);
        }

        public void Deserialize(NetDataReader reader)
        {
            JerseyNumber = reader.GetString();
            RightHandedness = reader.GetBool();
            Username = reader.GetString();
            UUID = reader.GetString();
        }
    }
}
