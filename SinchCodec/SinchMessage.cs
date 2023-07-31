using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SinchCodec
{
    public class SinchMessage
    {
        private const int MaxHeaders = 63;

        private Dictionary<string, string> _headers;
        public byte[] Payload { get; set; }
        public SinchMessage()
        {
            Headers = new Dictionary<string, string>();
            Payload = new byte[0];
        }

        public Dictionary<string, string> Headers
        {
            get => _headers;
            set
            {
                if (value.Count > MaxHeaders)
                    throw new ArgumentOutOfRangeException(nameof(Headers), "Headers cannot exceed 63");

                _headers = value;
            }
        }
    }
}