namespace SharedDesk
{
    partial class Form1
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
            this.listRoutingTable = new System.Windows.Forms.ListBox();
            this.labelGUID = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.lblGUID = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonPing = new System.Windows.Forms.Button();
            this.textTarget = new System.Windows.Forms.TextBox();
            this.listResponses = new System.Windows.Forms.ListBox();
            this.labelResponse = new System.Windows.Forms.Label();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbListenPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listRoutingTable
            // 
            this.listRoutingTable.FormattingEnabled = true;
            this.listRoutingTable.Location = new System.Drawing.Point(47, 41);
            this.listRoutingTable.Name = "listRoutingTable";
            this.listRoutingTable.Size = new System.Drawing.Size(212, 95);
            this.listRoutingTable.TabIndex = 0;
            // 
            // labelGUID
            // 
            this.labelGUID.AutoSize = true;
            this.labelGUID.Location = new System.Drawing.Point(44, 25);
            this.labelGUID.Name = "labelGUID";
            this.labelGUID.Size = new System.Drawing.Size(34, 13);
            this.labelGUID.TabIndex = 1;
            this.labelGUID.Text = "GUID";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(143, 25);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(17, 13);
            this.labelIP.TabIndex = 2;
            this.labelIP.Text = "IP";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(233, 25);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(26, 13);
            this.labelPort.TabIndex = 3;
            this.labelPort.Text = "Port";
            // 
            // lblGUID
            // 
            this.lblGUID.AutoSize = true;
            this.lblGUID.Location = new System.Drawing.Point(44, 158);
            this.lblGUID.Name = "lblGUID";
            this.lblGUID.Size = new System.Drawing.Size(34, 13);
            this.lblGUID.TabIndex = 4;
            this.lblGUID.Text = "GUID";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(238, 209);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 5;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            // 
            // buttonPing
            // 
            this.buttonPing.Location = new System.Drawing.Point(157, 209);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(75, 23);
            this.buttonPing.TabIndex = 6;
            this.buttonPing.Text = "Ping";
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // textTarget
            // 
            this.textTarget.Location = new System.Drawing.Point(47, 174);
            this.textTarget.Name = "textTarget";
            this.textTarget.Size = new System.Drawing.Size(66, 20);
            this.textTarget.TabIndex = 7;
            this.textTarget.Text = "0";
            // 
            // listResponses
            // 
            this.listResponses.FormattingEnabled = true;
            this.listResponses.Location = new System.Drawing.Point(340, 41);
            this.listResponses.Name = "listResponses";
            this.listResponses.Size = new System.Drawing.Size(212, 225);
            this.listResponses.TabIndex = 8;
            // 
            // labelResponse
            // 
            this.labelResponse.AutoSize = true;
            this.labelResponse.Location = new System.Drawing.Point(337, 9);
            this.labelResponse.Name = "labelResponse";
            this.labelResponse.Size = new System.Drawing.Size(60, 13);
            this.labelResponse.TabIndex = 9;
            this.labelResponse.Text = "Responses";
            // 
            // tbIp
            // 
            this.tbIp.Location = new System.Drawing.Point(128, 174);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(104, 20);
            this.tbIp.TabIndex = 10;
            this.tbIp.Text = "127.0.0.1";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(247, 174);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(75, 20);
            this.tbPort.TabIndex = 11;
            this.tbPort.Text = "8080";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(130, 158);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(17, 13);
            this.lblIP.TabIndex = 12;
            this.lblIP.Text = "IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "port";
            // 
            // tbListenPort
            // 
            this.tbListenPort.Location = new System.Drawing.Point(47, 255);
            this.tbListenPort.Name = "tbListenPort";
            this.tbListenPort.Size = new System.Drawing.Size(100, 20);
            this.tbListenPort.TabIndex = 14;
            this.tbListenPort.Text = "8080";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Listen port";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(157, 254);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 16;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 299);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbListenPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbIp);
            this.Controls.Add(this.labelResponse);
            this.Controls.Add(this.listResponses);
            this.Controls.Add(this.textTarget);
            this.Controls.Add(this.buttonPing);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.lblGUID);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.labelGUID);
            this.Controls.Add(this.listRoutingTable);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listRoutingTable;
        private System.Windows.Forms.Label labelGUID;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label lblGUID;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.TextBox textTarget;
        private System.Windows.Forms.ListBox listResponses;
        private System.Windows.Forms.Label labelResponse;
        private System.Windows.Forms.TextBox tbIp;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbListenPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnListen;
    }
}

