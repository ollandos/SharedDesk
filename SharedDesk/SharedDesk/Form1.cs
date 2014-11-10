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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void buttonPing_Click(object sender, EventArgs e)
        {

            // get ip address and port 
            IPAddress ip = IPAddress.Parse(tbIp.Text);
            int port = Convert.ToInt32(tbPort.Text);

            // check that port nr is between 0 and 65535
            if (port < 0 || port > 65535)
            {
                //lblStatus.Text = "Error: Wrong port";
                return;
            }

            // create end point
            IPEndPoint endPoint = new IPEndPoint(ip, port);

	    // connect to endpoint
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string text = "Hello";
            byte[] send_buffer = Encoding.ASCII.GetBytes(text);

            socket.SendTo(send_buffer, endPoint);

        }

        private void btnListen_Click(object sender, EventArgs e)
        {

            int port = Convert.ToInt32(tbListenPort.Text);
            Protocol p = new Protocol(port);
        }
    }
}
