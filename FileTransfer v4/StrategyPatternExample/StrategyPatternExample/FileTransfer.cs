using StrategyPatternExample.Observers;
using StrategyPatternExample.Transfer_Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrategyPatternExample
{

    public partial class FileTransfer : Form
    {

        TransferFile tf = null;
        string fileFullPath = "";
        string fileName = "";
        PnrpManager mPnrpManager;
        Thread mPnrpThread;
        // TODO: 
        // add progress bar for file transfer


        public FileTransfer()
        {
            LoginForm loginForm = new LoginForm();
            var result = loginForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                string name = loginForm.Username;
                mPnrpManager = new PnrpManager();

                mPnrpThread = new Thread(delegate()
                {
                    mPnrpManager.registerPeerName(name);
                });
                mPnrpThread.Start();

                //--------------------------
                InitializeComponent();

                string currentDir = Directory.GetCurrentDirectory();
                tbSaveFolder.Text = currentDir;

                tbReceiveIP.Text = getLocalIpAddress();
                tbReceivePort.Text = "8080";

                tbSendIP.Text = getLocalIpAddress();
                tbSendPort.Text = "8080";

                // Set Transfer file protocol
                tf = new TransferFile(new TransferTCPv4());

                // attach IObservable interface to monitor 
                tf.attach(new ConsoleObserver());
                lblStatus.Text = "Status: Selected TCPv4";
                tbFileName.Enabled = false;

                // a notify icon in the corner
                FieldInfo iconField = typeof(Form).GetField("defaultIcon", BindingFlags.NonPublic | BindingFlags.Static);
                Icon myIcon = (Icon)iconField.GetValue(iconField);
                //notifyicon = notifyIcon1;
                notifyIcon1.Text = "File Transfer App";
                notifyIcon1.Visible = true;
                //notifyicon.BalloonTipClicked += notifyicon_BalloonTipClicked;
                notifyIcon1.Icon = myIcon;

                // use events
                //FileTransferEvents.downloaded += FileTransferEvents_downloaded;
                FileTransferEvents.downloadComplete += FileTransferEvents_downloadComplete;
                FileTransferEvents.progress += FileTransferEvents_progress;
                FileTransferEvents.speedChange += FileTransferEvents_speedChange;
            }

        }

        void FileTransferEvents_speedChange(ChangedEvent e)
        {
            updateListBoxOfTransfers(FileTransferEvents.filename, FileTransferEvents.percent, FileTransferEvents.speed);
        }

        void FileTransferEvents_progress(ChangedEvent e)
        {
            byte percentage = (byte)e.Message;
            updateListBoxOfTransfers(FileTransferEvents.filename, percentage, FileTransferEvents.speed);
        }

        private void updateListBoxOfTransfers(string file, byte progress, string speed)
        {

            // since different threads are dealing with file transfers this updates
            // is not executed by the thread that holds the form, so a "invoke" is requried
            // this solves it... 
            if (lbFileTransfers.InvokeRequired)
            {
                this.Invoke(new Action<string, byte, string>(updateListBoxOfTransfers), new object[] { file, progress, speed });
                return;
            }
            else
            {
                if (lbFileTransfers != null)
                {
                    lbFileTransfers.Items.Clear();
                    lbFileTransfers.Items.Add("File:\t\t" + file);
                    lbFileTransfers.Items.Add("completed:\t" + progress.ToString() + "%");
                    lbFileTransfers.Items.Add("Speed:\t\t" + speed);
                    lbFileTransfers.Items.Add("");
                }
            }


        }

        void FileTransferEvents_downloadComplete(ChangedEvent e)
        {
            // notify icon
            notifyIcon1.BalloonTipTitle = "Received File";
            notifyIcon1.BalloonTipText = (string)e.Message;
            notifyIcon1.ShowBalloonTip(2000);

            updateListBoxOfTransfers((string)e.Message, 100, "0 kb/s");

        }


        /// <summary>
        /// Get default ip
        /// </summary>
        /// <returns></returns>

        public string getLocalIpAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string localIP = "";

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            try
            {
                // get ip address and port 
                IPAddress ip = IPAddress.Parse(tbSendIP.Text);
                int port = Convert.ToInt32(tbSendPort.Text);

                // check that port nr is between 0 and 65535
                if (port < 0 || port > 65535)
                {
                    lblStatus.Text = "Error: Wrong port";
                    return;
                }

                // create end point
                IPEndPoint endPoint = new IPEndPoint(ip, port);

                if (this.fileFullPath != "")
                {
                    // send file
                    tf.sendFile(this.fileFullPath, endPoint);

                    // status text
                    lblStatus.Text = String.Format("Sending file to {0}:{1}", ip.ToString(), port);

                    // list box
                    lbFileTransfers.Items.Add("Sending: " + this.fileName);
                    //lbFileTransfers.Items.Add("Progress:\t\t0%");
                    //lbFileTransfers.Items.Add("");
                }

            }
            catch (Exception error)
            {
                // TODO: 
                // Handle different errors and exceptions
                MessageBox.Show(error.Message);
            }


        }



        private void radioTCP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTCP.Checked == true)
            {
                this.tf = new TransferFile(new TransferTCP());
                this.tf.attach(new ConsoleObserver());
                lblStatus.Text = "Status: Selected TCP";
                tbFileName.Enabled = true;
            }

        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            // get ip address and port 
            //IPAddress ip = IPAddress.Parse(tbReceiveIP.Text);
            int port;
            try
            {
                port = Convert.ToInt32(tbReceivePort.Text);
            }
            catch (Exception)
            {
                lblStatus.Text = "Error: Wrong port";
                return;
            }

            // check that port nr is between 0 and 65535
            if (port < 0 || port > 65535)
            {
                lblStatus.Text = "Error: Wrong port";
                return;
            }

            // create end point
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            // notify
            lblStatus.Text = "Status: Listening on port: " + port.ToString();

            // listen for file
            if (tbFileName.Text != "")
            {
                tf.listenForFile(tbSaveFolder.Text + "\\" + tbFileName.Text, endPoint);
            }
            else
            {
                tf.listenForFile(tbSaveFolder.Text, endPoint);
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.fileFullPath = ofd.FileName;
                this.fileName = Path.GetFileName(this.fileFullPath);
                tbFileName.Text = this.fileName;

                lblStatus.Text = "Status: Selected file " + fileName;
            }


        }

        private void radioTCPv2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTCPv2.Checked == true)
            {
                this.tf = new TransferFile(new TransferTCPv2());
                this.tf.attach(new ConsoleObserver());

                lblStatus.Text = "Status: Selected TCPv2";
                tbFileName.Enabled = false;
            }
        }

        private void btnSelectSaveFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                tbSaveFolder.Text = fbd.SelectedPath;

                lblStatus.Text = "Status: Selected save folder";
            }
        }

        private void radioTCPv3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTCPv3.Checked == true)
            {
                this.tf = new TransferFile(new TransferTCPv3());
                this.tf.attach(new ConsoleObserver());

                lblStatus.Text = "Status: Selected TCPv3";
                tbFileName.Enabled = false;
            }
        }


        private void FileTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        private void FileTransfer_FormClosed(object sender, FormClosedEventArgs e)
        {
            mPnrpManager.removeRegistration();
            mPnrpThread.Join();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string classifier = tbPeerName.Text;
            string[] IpAndPort = mPnrpManager.getIPv4AndPort(classifier);
            tbSendIP.Text = IpAndPort[0];
            tbSendPort.Text = IpAndPort[1];
        }

        private void radioTCPv4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTCPv4.Checked == true)
            {
                this.tf = new TransferFile(new TransferTCPv4());
                this.tf.attach(new ConsoleObserver());
                lblStatus.Text = "Status: Selected TCPv4";
                tbFileName.Enabled = false;
            }
        }


    }
}
