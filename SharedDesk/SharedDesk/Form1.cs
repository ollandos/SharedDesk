using SharedDesk.UDP_protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using SharedDesk.TCP_file_transfer;

namespace SharedDesk
{
    public partial class Form1 : Form
    {
        // Peer Object
        Peer peer;

        // GUID 
        //Guid guid;
        int guid;
        int port;

        // variable for storing local IP
        private IPAddress ip;
        int remotePort;

        // used for login
        public string email;
        protected string apiKey;

        private FileHelper filehelper;

        /// <summary>
        /// APIService online
        /// </summary>
        public APIService service;

        public Form1(string email, string api_key)
        {
            InitializeComponent();

            this.email = email;
            this.apiKey = api_key;
            this.filehelper = new FileHelper();

            // generate guid (int)
            Random rnd = new Random();
            guid = rnd.Next(12);
            tbGUID.Text = guid.ToString();

            port = GetOpenPort();
            tbPORT.Text = port.ToString();

            // get local IP address
            //ip = IPAddress.Parse(LocalIPAddress());
            //tbIP.Text = ip.ToString();
        }

        /// <summary>
        /// Method for getting the local IP address in order to use it for hosting the service.
        /// </summary>
        /// <returns>Your IP address.</returns>
        public static string LocalIPAddress()
        {
            try
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        if (localIP.Contains("192.168"))
                            return localIP;
                    }
                }
                return localIP;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private int GetOpenPort()
        {
            int PortStartIndex = 1000;
            int PortEndIndex = 9000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Creating Peer
            peer = new Peer();

            // Subscribing to events
            subscribeToListener();

            // Generate GUID 
            //guid = Guid.NewGuid();
        }



        // Check all forms to and make sure IP and PORT are filled in and valid
        private bool validateForm()
        {

            try
            {
                ip = IPAddress.Parse(tbIP.Text);
            }
            catch (Exception)
            {
                toolStatus.Text = "ERROR: wrong ip";
                return false;
            }

            try
            {
                remotePort = Convert.ToInt32(tbPORT.Text);
            }
            catch (Exception)
            {
                toolStatus.Text = "ERROR: wrong port";
                return false;
            }

            // check that port nr is between 0 and 65535
            if (remotePort < 0 || remotePort > 65535)
            {
                toolStatus.Text = "ERROR: wrong port";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Button Events
        /// </summary>

        //Ping Button
        private void buttonPing_Click(object sender, EventArgs e)
        {
            validateForm();

            // create end point
            IPEndPoint remotePoint = new IPEndPoint(ip, remotePort);
            UDPResponder udpResponse = new UDPResponder(remotePoint, port);
            udpResponse.sendPing();

            toolStatus.Text = "Status: Sent ping";

        }
        /// <summary>
        /// HANDLERS
        /// </summary>

        public void subscribeToListener()
        {
            peer.updateTable += new Peer.handlerUpdatedTable(handleUpdatedTable);
        }

        private void handleUpdatedTable()
        {
            Dictionary<int, string> table = new Dictionary<int, string>();
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DataSource = null));
            foreach (KeyValuePair<int, PeerInfo> entry in peer.getRoutingTable.getPeers())
            {
                table.Add(entry.Key, entry.Key + "\t\t" + entry.Value.getGUID + "\t\t" + entry.Value.getIP() + ":" + entry.Value.getPORT());
            }
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DataSource = new BindingSource(table, null)));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DisplayMember = "Value"));
            //listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.ValueMember = "Key"));
        }

        // Gets routing table from boot peer and starts the process of finding closest peers
        private void btnGetRoutingTable_Click(object sender, EventArgs e)
        {
            validateForm();
            peer.init(Convert.ToInt32(tbGUID.Text), tbIP.Text, Convert.ToInt32(tbPORT.Text));
            toolStatus.Text = "Status: Sent routing table request";
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();

            if (dr != DialogResult.OK)
            {
                // no file was selected
                return;
            }

            string fileFullPath = ofd.FileName;
            string fileName = Path.GetFileName(fileFullPath);

            // get ip and port from selected peer in listbox
            int index = listRoutingTable.SelectedIndex;

            // find the guid of the peer selected
            //string selectedPeer = listRoutingTable.Items[index].ToString();

            if (index == -1)
            {
                toolStatus.Text = "Error: No peer selected!";
                return;
            }

            Dictionary<int, PeerInfo> peerDictionary = peer.getRoutingTable.getPeers();


            // FIX FOR MONDAY
            // Will loop through peerDictonary and send to the first one it finds.. 

            for (int i = 0; i < 12; i++)
            {
                if (peerDictionary.ContainsKey(i))
                {

                    PeerInfo receivingPeer = peerDictionary[i];

                    if (receivingPeer.getGUID != guid)
                    {
                        toolStatus.Text = String.Format("Sending file \"{0}\" to peer guid {1}, ip {2}:{3}", fileName, receivingPeer.getGUID, receivingPeer.getIP(), receivingPeer.getPORT());
 
                        IPAddress RecevingIp = IPAddress.Parse(receivingPeer.getIP());

                        //// Send file info 
                        //// create end point
                        IPEndPoint remotePoint = new IPEndPoint(RecevingIp, receivingPeer.getPORT());
                        UDPResponder udpResponse = new UDPResponder(remotePoint, port);
                        udpResponse.sendFileInfo(fileFullPath);
                    }

                    return;

                }
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            peer.sendLeaveRequests(); 
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<string> files = filehelper.getAvaliableFiles();

            listAvaliableFiles.Items.Clear();
            foreach (string s in files)
            {
                listAvaliableFiles.Items.Add(s);
            }

        }

        private void btnRequestFile_Click(object sender, EventArgs e)
        {
            int index = listAvaliableFiles.SelectedIndex;
            if (index == -1)
            {
                toolStatus.Text = "Error: No file selected!";
            }

            // get selected file
            string file = listAvaliableFiles.Items[index].ToString();
            
            // get peers with the file
            List<PeerInfo> peers = filehelper.getPeersWithFile(file);


        }
    }
}
