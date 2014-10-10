using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrategyPatternExample
{
    public partial class LoginForm : Form
    {
        public string Username { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Username = textBox1.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
