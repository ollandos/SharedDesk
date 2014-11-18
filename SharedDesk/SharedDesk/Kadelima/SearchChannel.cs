using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk.Kadelima
{
    public class SearchChannel
    {
        private PeerInfo mCurrentClosest;
        private int mCurrentTargetGUID;
        private int mPreviousGUID;
        private Peer mOwner;

        public SearchChannel(Peer owner, int guid)
        {
            mOwner = owner;
            mCurrentTargetGUID = guid;


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
