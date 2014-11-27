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

                    //// Send file info 
                    //// create end point
                    IPEndPoint remotePoint = new IPEndPoint(ip, remotePort);
                    UDPResponder udpResponse = new UDPResponder(remotePoint, listenPort);
                    udpResponse.sendFileInfo(fileFullPath);

                    return;

                }
            }
        }
        FileWatcher.FileWatcher fw;
        bool myWatchingBool = false;
        bool m_bIsChecked = false;

        private void btnBrowseFiles_Click(object sender, EventArgs e)
        {
            DialogResult resDialog = dlgOpenDir.ShowDialog();
            if (resDialog.ToString() == "OK")
            {
                tbFilePath.Text = dlgOpenDir.SelectedPath;

                if(cbIncludeSub.Checked)
                {
                    m_bIsChecked = true;
                }
                fw = new FileWatcher.FileWatcher(m_bIsChecked);
            }
        }

        private void tmrEditNotifier_Tick(object sender, EventArgs e)
        {
            if (fw._m_bMyBool == true)
            {
                lbFileChanges.BeginUpdate();
                lbFileChanges.Items.Add(fw.sbToString());
                lbFileChanges.EndUpdate();
                fw._m_bMyBool = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (myWatchingBool == false)
            {
                fw.WatchFile(tbFilePath.Text.ToString());
                tmrEditNotifier.Enabled = true;
                btnWatch.Text = "watching";
                myWatchingBool = true;
            }
            else
            {
                tmrEditNotifier.Enabled = false;
                btnWatch.Text = "watch";
                myWatchingBool = false;
            }
        }
    }
}
