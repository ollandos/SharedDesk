using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDesk.UDP_protocol;
using System.Net;

namespace SharedDesk.Kadelima
{
    public class SearchChannel
    {
        private int mCurrentTargetGUID;
        private int mPreviousGUID = -1;
        private Peer mOwner;

        // Initialized by passing the target peer (for the search) guid
        public SearchChannel(Peer owner, int guid)
        {
            mOwner = owner;
            mCurrentTargetGUID = guid;
        }

        // Called by passing closest peer. Searches if closer peer to the target is available
        public void onReceiveClosest(PeerInfo pInfo)
        {
            // If target not found && current search is not our previous search
            if (mCurrentTargetGUID != pInfo.getGUID && mPreviousGUID != pInfo.getGUID)
            {
                mPreviousGUID = pInfo.getGUID;

                IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
                UDPResponder responder = new UDPResponder(remotePoint, mOwner.getRoutingTable.MyInfo.getPORT());
                responder.sendRequestClosest(mOwner.getGUID, mCurrentTargetGUID) ;
            }
            else
            {
                // Add peer event
                mOwner.addPeerInfo(pInfo);
            }
        }

        // Gets target peer GUID
        public int getTargetGUID()
        {
            return mCurrentTargetGUID;
        }
    }
}
