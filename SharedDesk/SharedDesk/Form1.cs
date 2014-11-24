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
        int listenPort;

        // used for login
        public string email;
        protected string apiKey;

        /// <summary>
        /// APIService online
        /// </summary>
        public APIService service;

        public Form1(string email, string api_key)
        {
            InitializeComponent();

            this.email = email;
            this.apiKey = api_key;

            // generate guid (int)
            Random rnd = new Random();
            guid = rnd.Next(100);
            tbGUID.Text = guid.ToString();

            port = GetOpenPort();
            tbPORT.Text = port.ToString();

            // get local IP address
            ip = IPAddress.Parse(LocalIPAddress());
            tbIP.Text = ip.ToString();
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
                listenPort = Convert.ToInt32(tbListenPort.Text);
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

            // check that port nr is between 0 and 65535
            if (listenPort < 0 || listenPort > 65535)
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
            UDPResponder udpResponse = new UDPResponder(remotePoint, listenPort);
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
                table.Add(entry.Key, entry.Key + "        " +entry.Value.getGUID + "        " + entry.Value.getIP() + "        " + entry.Value.getPORT());
            }
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DataSource = new BindingSource(table, null)));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DisplayMember = "Value"));
            //listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.ValueMember = "Key"));
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (tbListenPort.Text == "")
            {
                toolStatus.Text = "ERROR: wrong port";
                return;
            }

            int port;
            try
            {
                port = Convert.ToInt32(tbListenPort.Text);
            }
            catch (Exception)
            {
                toolStatus.Text = "ERROR: wrong port";
                return;
            }

            // check that port nr is between 0 and 65535
            if (port < 0 || port > 65535)
            {
                toolStatus.Text = "ERROR: wrong port";
                return;
            }

            UDPListener p = new UDPListener(port);
            toolStatus.Text = "Status: Listening on port " + port.ToString();
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
            string guid;
            guid = listRoutingTable.SelectedValue.ToString();
            Console.WriteLine(guid);
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
            if (index == -1)
            {
                toolStatus.Text = "Error: No peer selected!";
                return;
            }

            PeerInfo peer = (PeerInfo) listRoutingTable.Items[index];
            toolStatus.Text = String.Format("Sending file \"{0}\" to peer guid {1}, ip {2}:{3}", fileName, peer.getGUID, peer.getIP(), peer.getPORT());

            // Send file info 
            // create end point
            IPEndPoint remotePoint = new IPEndPoint(ip, remotePort);
            UDPResponder udpResponse = new UDPResponder(remotePoint, listenPort);
            udpResponse.sendFileInfo(fileFullPath);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            peer.sendLeaveRequests(); 
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
