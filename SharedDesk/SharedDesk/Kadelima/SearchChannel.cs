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
                UDPResponder responder = new UDPResponder(remotePoint, mOwner.getRoutingTable.MyInfo.getPORT());
                responder.sendRequestClosest(mOwner.getGUID, mCurrentTargetGUID) ;
            }
            else
            {
                if (mCurrentTargetGUID != pInfo.getGUID)
                {
                    //IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
                    //UDPResponder responder = new UDPResponder(remotePoint, mOwner.getRoutingTable.MyInfo.getPORT());
                    //responder.sendRequestJoin(mCurrentTargetGUID, mOwner.getRoutingTable.MyInfo);
                    mOwner.addPeerInfo(pInfo);
                }
                else
                {
                    //IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(pInfo.getIP()), pInfo.getPORT());
                    //UDPResponder responder = new UDPResponder(remotePoint, mOwner.getRoutingTable.MyInfo.getPORT());
                    //responder.sendRequestJoin(mOwner.getRoutingTable.MyInfo.getGUID, mOwner.getRoutingTable.MyInfo);
                    mOwner.addPeerInfo(pInfo);
                }
                
            }
        }
    }
}
