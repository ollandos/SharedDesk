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
        XmlDocument root;

        public FileHelper()
        {
            this.peerListPath = "peers.xml";
            loadFromXmlFile();
        }

        private void loadFromXmlFile()
        {

            if (peerListFileExist() == false)
            {
                return;
            }

            string xmlString = "";
            try
            {
                // read from file
                xmlString = File.ReadAllText(peerListPath);
            }
            catch (Exception error)
            {
                return;
            }

            // load from file
            root = new XmlDocument();
            root.LoadXml(xmlString);
        }

        /// <summary>
        /// Check 
        /// </summary>
        /// <returns></returns>
        private bool peerListFileExist()
        {

            if (File.Exists(peerListPath))
            {
                return true;
            }

            return false;
        }

        private XmlNode peerInfoToXmlNode(PeerInfo p)
        {
            XmlNode peer = root.CreateTextNode("Peer");
            return peer;
        }

        public void addAvaliableFileInfoToPeer(string ip, int port, string fileName)
        {
            // find peer with same ip and port
            string selectedPeer = String.Format("/Peers/Peer[ip='{0}' and port='{1}']", ip, port);
            
            XmlElement el = (XmlElement)root.SelectSingleNode(selectedPeer);
            if (el != null)
            {
                // add a file info to node in xml document
                XmlElement elem = root.CreateElement("File");
                elem.InnerText = fileName;
                el.AppendChild(elem);

                // save file
                savePeerList();
            }
            else
            {
                Console.WriteLine("Could not find peer with that ip and port in XML!");
            }

        }

        private void updatePeerList(PeerInfo p)
        {

            if (root == null)
            {
                loadFromXmlFile();
            }

            string guid = p.getGUID.ToString();
            XmlNodeList nodeList = root.SelectNodes("/Peers/Peer");

            foreach (XmlNode n in nodeList)
            {
                string peerGuid = n["guid"].InnerXml;

                if (peerGuid == guid)
                {
                    // delete this element
                    string selectedPeer = String.Format("/Peers/Peer[guid={0}]", guid);

                    XmlElement el = (XmlElement)root.SelectSingleNode(selectedPeer);
                    if (el != null)
                    {
                        el.ParentNode.RemoveChild(el);
                    }

                }


            }

            // peer does not exist in list
            if (nodeList.Count == 0)
            {
                return;
            }

            // create new node 
            XmlNode peer = nodeList.Item(0).Clone();
            peer["guid"].InnerXml = guid;
            peer["ip"].InnerXml = p.getIP();
            peer["port"].InnerXml = p.getPORT().ToString();
            peer["last_seen_date"].InnerXml = DateTime.Now.ToShortDateString();
            peer["last_seen_time"].InnerXml = DateTime.Now.ToLongTimeString();

            // add new peer to list
            root.ChildNodes[1].AppendChild(peer);

            // save file
            savePeerList();

        }

        private void createNewPeerXML(PeerInfo p)
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

                writer.Close();
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
            try
            {
                root.Save(peerListPath);
            }
            catch (Exception)
            {

            }
        }

    }
}
