using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SharedDesk
{
    public partial class loginForm : Form
    {
        private APIService service;

        public loginForm()
        {
            InitializeComponent();

            service = new APIService();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("http://proep.maximize-it.eu");
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            picBoxLogo.Cursor = Cursors.Hand;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                errorProviderEmail.Icon = Properties.Resources.error;
                errorProviderEmail.SetError(txtEmail, "Fill in an email!");
                labelErrorEmail.Text = "Fill in an email!";
                labelErrorEmail.Visible = true;
                txtEmail.Focus();
            }
            else if (!IsValidEmail(txtEmail.Text))
            {
                errorProviderEmail.Icon = Properties.Resources.error;
                errorProviderEmail.SetError(txtEmail, "Invalid email format!");
                labelErrorEmail.Text = "Invalid email format!";
                labelErrorEmail.Visible = true;
                txtEmail.Focus();
            }
            else if (String.IsNullOrEmpty(txtPassword.Text))
            {
                errorProviderPw.Icon = Properties.Resources.error;
                errorProviderPw.SetError(txtPassword, "Fill in a password!");
                labelErrorPw.Text = "Fill a password!";
                labelErrorPw.Visible = true;
                txtPassword.Focus();
            }
            else
            {
                string api_key = service.login(txtEmail.Text.ToString(), txtPassword.Text.ToString());

                if (api_key != null)
                {
                    Form mainForm = new Form1(txtEmail.Text, api_key);

                    mainForm.Show();

                    this.Hide();
                }
                else
                {
                    errorProviderEmail.Icon = Properties.Resources.error;
                    errorProviderEmail.SetError(txtEmail, "Unknwon email...");
                    errorProviderPw.Icon = Properties.Resources.error;
                    errorProviderPw.SetError(txtPassword, "...password!");
                    labelErrorPw.Text = "Unknown email or password";
                    labelErrorPw.Visible = true;
                    txtPassword.Text = "";
                    txtEmail.Focus();
                }
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                errorProviderEmail.Icon = Properties.Resources.error;
                errorProviderEmail.SetError(txtEmail, "Fill in an email!");
                labelErrorEmail.Text = "Fill in an email!";
                labelErrorEmail.Visible = true;
                txtEmail.Focus();
            }
            else if(!IsValidEmail(txtEmail.Text))
            {
                errorProviderEmail.Icon = Properties.Resources.error;
                errorProviderEmail.SetError(txtEmail, "Invalid email format!");
                labelErrorEmail.Text = "Invalid email format!";
                labelErrorEmail.Visible = true;
                txtEmail.Focus();
            }
            else
            {
                errorProviderEmail.Icon = Properties.Resources.check;
                errorProviderEmail.SetError(txtEmail, "Email address filled.");
                labelErrorEmail.Visible = false;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                errorProviderPw.Icon = Properties.Resources.error;
                errorProviderPw.SetError(txtPassword, "Fill in a password!");
                labelErrorPw.Text = "Fill a password!";
                labelErrorPw.Visible = true;
                txtPassword.Focus();
            }
            else
            {
                errorProviderPw.Icon = Properties.Resources.check;
                errorProviderPw.SetError(txtPassword, "Password filled");
                labelErrorPw.Visible = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
