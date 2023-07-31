using SinchCodec.WorkTestsSinch;
using System.Text;

namespace SinchCodec
{
    public class Codec : ICodec
    {
        public byte[] Encode(SinchMessage message)
        {
            if (message.Headers.Count > 63)
                throw new InvalidOperationException("Message has too many headers");

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Write headers
                bw.Write((byte)message.Headers.Count);
                foreach (var header in message.Headers)
                {
                    byte[] keyBytes = Encoding.ASCII.GetBytes(header.Key);
                    byte[] valueBytes = Encoding.ASCII.GetBytes(header.Value);
                    if (keyBytes.Length > 1023 || valueBytes.Length > 1023)
                        throw new InvalidOperationException("Header key/value size exceeded");

                    bw.Write((ushort)keyBytes.Length);
                    bw.Write(keyBytes);
                    bw.Write((ushort)valueBytes.Length);
                    bw.Write(valueBytes);
                }

                // Write payload
                if (message.Payload.Length > 262144)
                    throw new InvalidOperationException("Payload size exceeded");

                bw.Write((uint)message.Payload.Length);
                bw.Write(message.Payload);
                bw.Flush();
                return ms.ToArray();
            }
        }

        public SinchMessage Decode(byte[] data)
        {
            var message = new SinchMessage();
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                var headerCount = br.ReadByte();
                if (headerCount > 63)
                    throw new InvalidOperationException("Encoded message has too many headers");

                for (int i = 0; i < headerCount; i++)
                {
                    var keyLength = br.ReadUInt16();
                    var key = Encoding.ASCII.GetString(br.ReadBytes(keyLength));
                    var valueLength = br.ReadUInt16();
                    var value = Encoding.ASCII.GetString(br.ReadBytes(valueLength));
                    message.Headers[key] = value;
                }

                // Read payload
                var payloadLength = br.ReadUInt32();
                message.Payload = br.ReadBytes((int)payloadLength);
            }

            return message;
        }
    }
}