﻿using SharedDesk.UDP_protocol;
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

        // var for storing peer guid
        int guid;
        // var for storing peer port
        int port;

        // variable for storing local IP
        private IPAddress ip;

        // user info
        public string email;
        protected string apiKey;

        // APIService online
        public APIService service;

        public Form1(string email, string api_key)
        {
            InitializeComponent();

            service = new APIService();

            // pass from login form
            this.email = email;
            this.apiKey = api_key;

            // generate guid (int)
            guid = generateGuid();
            // show in textbox
            tbGUID.Text = guid.ToString();

            // get local IP address
            ip = IPAddress.Parse(LocalIPAddress());
            // show in textbox
            tbIP.Text = ip.ToString();

            port = 0;
            while (port == 0)
            {
                // get available port
                port = getNextAvailablePort();
            }
            // show port in textbox
            tbPORT.Text = port.ToString();

            service.addPeer(this.apiKey, guid.ToString(), ip.ToString(), port.ToString());
        }

        // Method for getting the local IP address in order to use it for the peer.
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
                        if (localIP.Contains("192.168.1."))
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

        // Method to scan all used ports and return the next available port to be used for peer. 
        private int getNextAvailablePort()
        {
            // ini variables
            List<int> portArray = new List<int>();
            int minPortNr = 1000;

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            // add all connections to array
            portArray.AddRange(from n in connections where n.LocalEndPoint.Port >= minPortNr select n.LocalEndPoint.Port);
            // temp var
            IPEndPoint[] endPoints;
            // get all tcp listeners
            endPoints = properties.GetActiveTcpListeners();
            // add tcp listeners to array
            portArray.AddRange(from n in endPoints where n.Port >= minPortNr select n.Port);
            // get all upd listeners
            endPoints = properties.GetActiveUdpListeners();
            // add upd listeners to array
            portArray.AddRange(from n in endPoints where n.Port >= minPortNr select n.Port);
            // sort array 
            portArray.Sort();
            // return next available port
            for (int i = minPortNr; i < UInt16.MaxValue; i++)
            {
                if (!portArray.Contains(i))
                    return i;
            }
            return 0;
            
        }

        // Method to randomly generate a guid.
        private int generateGuid()
        {
            Random rnd = new Random();
            return rnd.Next(15);
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

        // Gets routing table from boot peer and starts the process of finding closest peers
        private void btnGetRoutingTable_Click(object sender, EventArgs e)
        {
            //UdpClient updScan = new UdpClient();// What is this???
            try
            {
                //updScan.Connect(ip, port);// And this???

                peer.init(Convert.ToInt32(tbGUID.Text), tbIP.Text, Convert.ToInt32(tbPORT.Text));
                toolStatus.Text = "Status: Sent routing table request";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                // No file was selected
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
                if (peerDictionary.ContainsKey(i) && peer.getGUID != i)
                {

                    PeerInfo receivingPeer = peerDictionary[i];
                    toolStatus.Text = String.Format("Sending file \"{0}\" to peer guid {1}, ip {2}:{3}", fileName, receivingPeer.getGUID, receivingPeer.getIP(), receivingPeer.getPORT());

                    IPAddress RecevingIp = IPAddress.Parse(receivingPeer.getIP());

                    //// Send file info 
                    //// create end point
                    IPEndPoint remotePoint = new IPEndPoint(RecevingIp, receivingPeer.getPORT());
                    UDPResponder udpResponse = new UDPResponder(remotePoint, port);
                    udpResponse.sendFileInfo(fileFullPath);

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
                table.Add(entry.Key, entry.Key + "        " + entry.Value.getGUID + "        " + entry.Value.getIP() + "        " + entry.Value.getPORT());
            }
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DataSource = new BindingSource(table, null)));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DisplayMember = "Value"));
            //listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.ValueMember = "Key"));
        }
    }
}
