using System.Security.Cryptography;
using System.Text;

namespace CoreDashboard.Extensions
{
    public class Cryptography
    {
        public static string CreateSHA256(string input) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
    }
}
