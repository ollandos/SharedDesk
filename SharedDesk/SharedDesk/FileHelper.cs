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

        public bool peerExistInFile(string guid)
        {

            string xmlString = File.ReadAllText(peerListPath);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            XmlNodeList list = xml.SelectNodes("/Peers/Peer");
            foreach (XmlNode n in list)
            {
                string peerGuid = n["guid"].InnerText;

                if (peerGuid == guid)
                {
                    // delete this element
                    return true;
                }

            }

            return false;
        }

        public void createNewPeerXML(PeerInfo p)
        {
            // store peer list to file
            using (XmlWriter writer = XmlWriter.Create(peerListPath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Peers");

                writer.WriteElementString("guid", p.getGUID.ToString());
                writer.WriteElementString("ip", p.getIP());
                writer.WriteElementString("port", p.getPORT().ToString());
                writer.WriteElementString("last_seen_date", DateTime.Now.ToShortDateString());
                writer.WriteElementString("last_seen_time", DateTime.Now.ToLongTimeString());

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

            // check if peer exist in list, in that case 
            if (peerExistInFile(p.getGUID.ToString()))
            {
                // append element 

            }


            // update "last_seen"


        }

        public void savePeerList(List<PeerInfo> list)
        {
            // store peer list to file
            using (XmlWriter writer = XmlWriter.Create("peers.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Peers");

                foreach (PeerInfo p in list)  // <-- This is new
                {
                    writer.WriteStartElement("Peer"); // <-- Write employee element

                    //writer.WriteElementString("ID", employee.Id.ToString());   // <-- These are new
                    //writer.WriteElementString("FirstName", employee.FirstName);
                    //writer.WriteElementString("LastName", employee.LastName);
                    //writer.WriteElementString("Salary", employee.Salary.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument(); ;
            }
        }

        public List<PeerInfo> getPeerList()
        {
            // get peer list
            return null;
        }


    }
}
