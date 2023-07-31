using NUnit.Framework;
using System.Text;

namespace SinchCodec.Tests
{
    public class MessageCodecTests
    {
        private Codec _messageCodec;

        [SetUp]
        public void Setup()
        {
            _messageCodec = new Codec();
        }

        [Test]
        public void TestEncodeDecode()
        {
            // Arrange
            var originalMessage = new SinchMessage
            {
                Headers = new Dictionary<string, string>
                {
                    {"header1", "value1"},
                    {"header2", "value2"}
                },
                Payload = Encoding.UTF8.GetBytes("Hello, Sinch! How are you today?")
            };

            // Act
            var encodedMessage = _messageCodec.Encode(originalMessage);
            var decodedMessage = _messageCodec.Decode(encodedMessage);

            // Assert
            CollectionAssert.AreEqual(originalMessage.Headers, decodedMessage.Headers);
            CollectionAssert.AreEqual(originalMessage.Payload, decodedMessage.Payload);
        }
    }
}
