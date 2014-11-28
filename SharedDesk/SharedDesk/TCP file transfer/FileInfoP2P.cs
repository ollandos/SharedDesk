using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk.TCP_file_transfer
{
    public class FileInfoP2P
    {
        public string name;
        public long size;
        public byte[] md5;

        public FileInfoP2P(string name, long size, byte[] md5)
        {
            this.name = name;
            this.size = size;
            this.md5 = md5;
        }

        public string getMd5AsString()
        {
            //string md5String = BitConverter.ToString(md5);
            return BitConverter.ToString(md5).Replace("-", "");
        }



    }
}
