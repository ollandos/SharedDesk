using SharedDesk.UDP_protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharedDesk
{
    public partial class Form1 : Form
    {
        // Peer Object
        Peer peer;

        // GUID 
        //Guid guid;
 
        IPAddress ip;
        int remotePort;
        int listenPort;

        public Form1()
        {
            InitializeComponent();
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
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.Items.Clear()));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DataSource = null));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DataSource = peer.getRoutingTable.getPeers()));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.DisplayMember = "toString"));
            listRoutingTable.Invoke(new MethodInvoker(() => listRoutingTable.ValueMember = "getGUID"));
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
            peer.init(Convert.ToInt32(tbGUID.Text),tbIP.Text,Convert.ToInt32(tbPORT.Text));
            toolStatus.Text = "Status: Sent routing table request";
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            int guid = -1;
            guid = (int)listRoutingTable.SelectedValue;
            Console.WriteLine(guid);
        }
    }
}
