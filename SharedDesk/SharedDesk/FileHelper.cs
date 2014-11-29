using SharedDesk.TCP_file_transfer;
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

        /// <summary>
        /// check if list of peers exist 
        /// </summary>
        /// <returns>false if file cannot be found</returns>
        private bool loadFromXmlFile()
        {

            if (peerListFileExist() == false)
            {
                return false;
            }

            string xmlString = "";
            try
            {
                // read from file
                xmlString = File.ReadAllText(peerListPath);
            }
            catch (Exception error)
            {
                return false;
            }

            // load from file
            root = new XmlDocument();
            root.LoadXml(xmlString);
            return true;
        }


        public List<PeerInfo> getPeersWithFile(string name)
        {

            if (loadFromXmlFile() == false)
            {
                // failed to load file
                return null;
            }
                
            List<PeerInfo> peers = new List<PeerInfo>();

            // find all peers with the file
            string selectedPeer = String.Format("/Peers/Peer/File[name='{0}']", name);
            XmlNodeList nodeList = root.SelectNodes(selectedPeer);

            foreach (XmlNode n in nodeList)
            {
                string elGuid = n.ParentNode["guid"].InnerXml;
                string elIp = n.ParentNode["ip"].InnerXml;
                string elPort = n.ParentNode["port"].InnerXml;

                int elGuidInt = Convert.ToInt32(elGuid);
                int elPortInt = Convert.ToInt32(elPort);

                Console.WriteLine("guid {0}\t\tip {1}:{2} has the file", elGuid, elIp, elPort);

                PeerInfo newPeer = new PeerInfo(elGuidInt, elIp, elPortInt);
                peers.Add(newPeer);

            }

            // POTENTIAL BUG
            // might run into problems if someone store different files
            // with same name. (since I don't use md5 to lookup the file)

            return peers;

        }

        public List<string> getAvaliableFiles()
        {

            if (loadFromXmlFile() == false)
            {
                // failed to load file
                return null;
            }

            XmlNodeList nodeList = root.SelectNodes("/Peers/Peer/File");
            List<string> files = new List<string>();

            foreach (XmlNode n in nodeList)
            {
                string name = n["name"].InnerXml;
                if (files.Contains(name) == false)
                {
                    files.Add(name);
                }
            }

            return files;
        }

        /// <summary>
        /// get the md5 hash of the file 
        /// need this to request the file from a peer
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string getMd5OfFile(string name)
        {

            if (loadFromXmlFile() == false)
            {
                // failed to load file
                return null;
            }

            // find peer with same ip and port
            string selectedFile = String.Format("/Peers/Peer/File[name='{0}']", name);

            XmlNode el = (XmlNode)root.SelectSingleNode(selectedFile);
            if (el != null)
            {
                return el["md5"].InnerXml;
            }

            // POSSIBLE BUG: 
            // if a two files have the same name, but different md5 hash
            // then we can't know what file the user wants

            return "";
        }

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

        /// <summary>
        /// create a share.xml file with info of what files and
        /// folders are shared with peers (or publicly avaliable)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="peer"></param>
        public void authorizeFileDownload(string filePath, PeerInfo peer)
        {
            // create another xml file where you store
            // what peers are allowed to access files and folders
            
            // if PeerInfo is null then the share is for everyone
            if (peer == null)
            {
                // public share!
            }





        }


        public void addAvaliableFileInfoToPeer(string ip, int port, FileInfoP2P file)
        {
            if (loadFromXmlFile() == false)
            {
                // failed to load file
                return;
            };

            // find peer with same ip and port
            string selectedPeer = String.Format("/Peers/Peer[ip='{0}' and port='{1}']", ip, port);

            XmlNode el = (XmlNode)root.SelectSingleNode(selectedPeer);
            if (el != null)
            {
                // create a new file node
                XmlElement fileNode = root.CreateElement("File");

                // create nodes attached to this file node
                XmlElement nameNode = root.CreateElement("name");
                nameNode.InnerText = file.name;

                XmlElement sizeNode = root.CreateElement("size");
                sizeNode.InnerText = file.size.ToString();

                XmlElement md5Node = root.CreateElement("md5");
                md5Node.InnerText = file.getMd5AsString();

                fileNode.AppendChild(nameNode);
                fileNode.AppendChild(sizeNode);
                fileNode.AppendChild(md5Node);
                el.AppendChild(fileNode);

                // add a file info to node in xml 
                //XmlNode elem = root.CreateElement("File");
                //elem.InnerText = file.name; 

                //el.AppendChild(elem);

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
                if (loadFromXmlFile() == false)
                {
                    // failed to load file
                    return;
                }

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
