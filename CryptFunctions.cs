using System.Security.Cryptography;
using System.IO;
namespace Checksum_files_test
{
    public class CryptFunctions
    {
        public string GetMD5HashFromFile(string fileName, MD5 md5passed)
        {

                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5passed.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }


        public static byte[] ComputeMd5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }
    }
}

