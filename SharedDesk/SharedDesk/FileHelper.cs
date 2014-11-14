using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk
{

    /// <summary>
    /// Class that writes to file 
    /// for example in order to store the peer information to file
    /// 
    /// Also read from file
    /// for example in order to re-connect to the network
    /// Can go through list of peers saved from previous session
    /// Ping peers until we find one that is online (sort by timestamp first)
    /// then request routing table from that peer
    /// </summary>
    class FileHelper
    {

	/// <summary>
	/// Check 
	/// </summary>
	/// <returns></returns>
        public bool peerListFileExist()
        {
            return false;
        }

        public void savePeerList(List<PeerInfo> list)
        {
	    // store peer list to file
        }

        public List<PeerInfo> getPeerList()
        {
	    // get peer list
            return null;
        }


    }
}
