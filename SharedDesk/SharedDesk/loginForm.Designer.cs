namespace SharedDesk
{
    partial class loginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginForm));
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.picBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.errorProviderEmail = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.errorProviderPw = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelErrorEmail = new System.Windows.Forms.Label();
            this.labelErrorPw = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderPw)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnLogin.Location = new System.Drawing.Point(308, 215);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(94, 43);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.txtEmail.Location = new System.Drawing.Point(308, 75);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(190, 26);
            this.txtEmail.TabIndex = 0;
            this.txtEmail.Text = "max@email.com";
            this.txtEmail.Leave += new System.EventHandler(this.txtEmail_Leave);
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(250, 78);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(52, 20);
            this.labelEmail.TabIndex = 3;
            this.labelEmail.Text = "Email:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password:";
            // 
            // picBoxLogo
            // 
            this.picBoxLogo.Image = global::SharedDesk.Properties.Resources.logo;
            this.picBoxLogo.ImageLocation = "";
            this.picBoxLogo.Location = new System.Drawing.Point(12, 61);
            this.picBoxLogo.Name = "picBoxLogo";
            this.picBoxLogo.Size = new System.Drawing.Size(197, 197);
            this.picBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxLogo.TabIndex = 5;
            this.picBoxLogo.TabStop = false;
            this.picBoxLogo.Click += new System.EventHandler(this.pictureBox1_Click);
            this.picBoxLogo.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(6, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(206, 35);
            this.labelTitle.TabIndex = 6;
            this.labelTitle.Text = "SharedDesk";
            // 
            // errorProviderEmail
            // 
            this.errorProviderEmail.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProviderEmail.ContainerControl = this;
            this.errorProviderEmail.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProviderEmail.Icon")));
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.txtPassword.Location = new System.Drawing.Point(308, 150);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(190, 26);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "max";
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // errorProviderPw
            // 
            this.errorProviderPw.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProviderPw.ContainerControl = this;
            this.errorProviderPw.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProviderPw.Icon")));
            // 
            // labelErrorEmail
            // 
            this.labelErrorEmail.AutoSize = true;
            this.labelErrorEmail.Location = new System.Drawing.Point(304, 104);
            this.labelErrorEmail.Name = "labelErrorEmail";
            this.labelErrorEmail.Size = new System.Drawing.Size(51, 20);
            this.labelErrorEmail.TabIndex = 7;
            this.labelErrorEmail.Text = "label1";
            this.labelErrorEmail.Visible = false;
            // 
            // labelErrorPw
            // 
            this.labelErrorPw.AutoSize = true;
            this.labelErrorPw.Location = new System.Drawing.Point(304, 179);
            this.labelErrorPw.Name = "labelErrorPw";
            this.labelErrorPw.Size = new System.Drawing.Size(51, 20);
            this.labelErrorPw.TabIndex = 8;
            this.labelErrorPw.Text = "label3";
            this.labelErrorPw.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(344, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 35);
            this.label1.TabIndex = 9;
            this.label1.Text = "Login";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(404, 215);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(94, 43);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 261);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Register? Click image.";
            // 
            // loginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(538, 281);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelErrorPw);
            this.Controls.Add(this.labelErrorEmail);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.picBoxLogo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnLogin);
            this.MaximumSize = new System.Drawing.Size(560, 303);
            this.MinimumSize = new System.Drawing.Size(560, 303);
            this.Name = "loginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderPw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picBoxLogo;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ErrorProvider errorProviderEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ErrorProvider errorProviderPw;
        private System.Windows.Forms.Label labelErrorPw;
        private System.Windows.Forms.Label labelErrorEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
    }
}