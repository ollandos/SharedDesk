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

        public SearchChannel(Peer owner, int guid)
        {
            mOwner = owner;
            mCurrentTargetGUID = guid;
        }

        public int getTargetGUID() 
        {
            return mCurrentTargetGUID;
        }

        public void onReceiveClosest(PeerInfo pInfo)
        {
            if (mCurrentTargetGUID != pInfo.getGUID && mPreviousGUID != pInfo.getGUID)
            {
                mPreviousGUID = pInfo.getGUID;

                IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
                UDPResponder responder = new UDPResponder(remotePoint, pInfo.getPORT());
                responder.sendRequestClosest(mOwner.getGUID, mCurrentTargetGUID) ;
            }
            else
            {
                //TODO: do this check on return in the main thread
                //IF found PeerInfo does not exist in the table, THEN add it to the table.
                mOwner.addPeerInfo(mCurrentTargetGUID, pInfo);
            }


        }
    }
}
