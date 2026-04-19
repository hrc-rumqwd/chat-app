using ChatApp.Application.Contracts.Encoders;
using System.IO.Hashing;
using System.Text;

namespace ChatApp.Infrastructure.Encoders
{
    public class Crc32Encoder : IApplicationEncoder
    {
        public string Decode(string input)
        {
            throw new NotImplementedException();
        }

        public string Encode(string input)
        {
            var crc32Hasher = new Crc32();
            byte[] hashingResult = Crc32.Hash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(hashingResult);
        }
    }
}
