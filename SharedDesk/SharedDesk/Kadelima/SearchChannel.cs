using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDesk.UDP_protocol;

namespace SharedDesk.Kadelima
{
    public class SearchChannel
    {
        private PeerInfo mCurrentClosest;
        private int mCurrentTargetGUID;
        private int mPreviousGUID;
        private Peer mOwner;

        public SearchChannel(Peer owner, int guid, UDPResponder responder)
        {
            mOwner = owner;
            mCurrentTargetGUID = guid;
            
            //responder.requestRindClosest
        }

        public int getTargetGUID() 
        {
            return mCurrentTargetGUID;
        }

        public void onReceiveClosest(PeerInfo pInfo)
        {
            if (mCurrentTargetGUID != pInfo.getGUID() && mPreviousGUID != pInfo.getGUID())
            {
                mPreviousGUID = pInfo.getGUID();
                //Trigger askForClosestPeer
            }
            else
            {
                //TODO: do this check on return in the main thread
                //IF found PeerInfo does not exist in the table, THEN add it to the table.
                mOwner.addPeerInfo(pInfo);
            }


        }
    }
}
