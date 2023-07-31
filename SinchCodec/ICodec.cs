namespace SinchCodec
{
    namespace WorkTestsSinch
    {
        public interface ICodec
        {
            byte[] Encode(SinchMessage message);
            SinchMessage Decode(byte[] data);
        }
    }
}