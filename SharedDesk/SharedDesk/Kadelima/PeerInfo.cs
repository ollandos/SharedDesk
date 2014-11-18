using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
{
    [Serializable()]
    public class PeerInfo
    {
        private int GUID;
        private string IP;
        private int PORT;
        public PeerInfo(int guid, string ip, int port)
        {
            this.GUID = guid;
            this.IP = ip;
            this.PORT = port;
        }

        public int getGUID() {
            return this.GUID;
        }

        public string getIP()
        {
            return this.IP;
        }

        public int getPORT()
        {
            return this.PORT;
        }

        public string toString()
        {
            return "GUID: " + GUID + " IP: " + IP + " PORT: " + PORT;
        }
    }
}
