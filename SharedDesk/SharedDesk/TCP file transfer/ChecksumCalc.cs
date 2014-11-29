using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
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

        /// <summary>
        /// get md5 hash of file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public byte[] GetMD5Checksum(string filePath)
        {
        
            HashAlgorithm MD5 = new MD5CryptoServiceProvider();

            using (var stream = new BufferedStream(File.OpenRead(filePath), 100000))
            {
                return MD5.ComputeHash(stream);
            }
        }

        static public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        /// <summary>
        /// Check if arrays is identical to each other
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if they are identical, false other vice</returns>
        public bool checkIfHashisIdentical(byte[] a, byte[] b)
        {
            return Enumerable.SequenceEqual(a, b);
        }

    }

}

