using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
    public class FileHelper
    {
        string peerListPath;
        XmlDocument xml;

        public FileHelper()
        {
            this.peerListPath = "peers.xml";
        }

        /// <summary>
        /// Check 
        /// </summary>
        /// <returns></returns>
        public bool peerListFileExist()
        {

            if (File.Exists(peerListPath))
            {
                return true;
            }

            return false;
        }

        private XmlNode peerInfoToXmlNode(PeerInfo p)
        {
            XmlNode peer = xml.CreateTextNode("Peer");
            return peer;
        }

        public void updatePeerList(PeerInfo p)
        {

            string guid = p.getGUID.ToString();

            string xmlString = File.ReadAllText(peerListPath);

            // load from file
            xml = new XmlDocument();
            xml.LoadXml(xmlString);

            XmlNodeList list = xml.SelectNodes("/Peers/Peer");

            foreach (XmlNode n in list)
            {
                string peerGuid = n["guid"].InnerText;

                if (peerGuid == guid)
                {

                    //xml.ReplaceChild(peerInfoToXmlNode(p), n);
                    return;


                    // update element
                    //n["last_seen_date"].Value = DateTime.Now.ToShortDateString();
                    //n["last_seen_time"].Value = DateTime.Now.ToLongTimeString();

                    // delete this element
                }

            }

            // peer does not exist in list
            
            // TODO: 
            // add peer to list



        }

        public void createNewPeerXML(PeerInfo p)
        {
            // store peer list to file
            using (XmlWriter writer = XmlWriter.Create(peerListPath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Peers");
                writer.WriteStartElement("Peer");

                writer.WriteElementString("guid", p.getGUID.ToString());
                writer.WriteElementString("ip", p.getIP());
                writer.WriteElementString("port", p.getPORT().ToString());
                writer.WriteElementString("last_seen_date", DateTime.Now.ToShortDateString());
                writer.WriteElementString("last_seen_time", DateTime.Now.ToLongTimeString());

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void savePeer(PeerInfo p)
        {

            if (peerListFileExist() == false)
            {
                // check if peer list exist
                createNewPeerXML(p);
                return;
            }

            // append or add peerInfo to list
            updatePeerList(p);

            // save xml


        }

        public void savePeerList()
        {
            xml.Save(XmlWriter.Create(peerListPath));
        }

        public List<PeerInfo> getPeerList()
        {
            // get peer list
            return null;
        }


    }
}
