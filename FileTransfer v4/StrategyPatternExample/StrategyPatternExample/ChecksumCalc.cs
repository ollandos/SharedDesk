using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternExample
{
    /// <summary>
    /// Class for calculating checksum of file
    /// 
    /// In the pre-buffer of the file transfer, send a MD5 hash
    /// When the receiving client has received the whole file, check the hash again
    /// We can then comfirm that the file received is 100% identical to the one sent
    /// </summary>
    class ChecksumCalc
    {

        public string GetMD5ChecksumString(string filePath)
        {
        
            HashAlgorithm MD5 = new MD5CryptoServiceProvider();

            using (var stream = new BufferedStream(File.OpenRead(filePath), 100000))
            {
                byte[] hash = MD5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public byte[] GetMD5ChecksumByteArray(string filePath)
        {
        
            HashAlgorithm MD5 = new MD5CryptoServiceProvider();

            using (var stream = new BufferedStream(File.OpenRead(filePath), 100000))
            {
                return MD5.ComputeHash(stream);
            }
        }

    }

}

