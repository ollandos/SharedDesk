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
            this.labelPort = new System.Windows.Forms.Label();
            this.lblGUID = new System.Windows.Forms.Label();
            this.buttonPing = new System.Windows.Forms.Button();
            this.tbGUID = new System.Windows.Forms.TextBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbPORT = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnGetRoutingTable = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();
            this.listAvaliableFiles = new System.Windows.Forms.ListBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnRequestFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listTransfers = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listRoutingTable
            // 
            this.listRoutingTable.FormattingEnabled = true;
            this.listRoutingTable.Location = new System.Drawing.Point(47, 41);
            this.listRoutingTable.Name = "listRoutingTable";
            this.listRoutingTable.Size = new System.Drawing.Size(275, 95);
            this.listRoutingTable.TabIndex = 0;
            // 
            // labelGUID
            // 
            this.labelGUID.AutoSize = true;
            this.labelGUID.Location = new System.Drawing.Point(44, 25);
            this.labelGUID.Name = "labelGUID";
            this.labelGUID.Size = new System.Drawing.Size(29, 13);
            this.labelGUID.TabIndex = 1;
            this.labelGUID.Text = "Spot";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(240, 25);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(39, 13);
            this.labelPort.TabIndex = 3;
            this.labelPort.Text = "IP:Port";
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
            // buttonPing
            // 
            this.buttonPing.Location = new System.Drawing.Point(209, 228);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(114, 23);
            this.buttonPing.TabIndex = 6;
            this.buttonPing.Text = "Ping";
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // tbGUID
            // 
            this.tbGUID.Location = new System.Drawing.Point(47, 174);
            this.tbGUID.Name = "tbGUID";
            this.tbGUID.Size = new System.Drawing.Size(66, 20);
            this.tbGUID.TabIndex = 7;
            this.tbGUID.Text = "0";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(128, 174);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(104, 20);
            this.tbIP.TabIndex = 10;
            this.tbIP.Text = "127.0.0.1";
            // 
            // tbPORT
            // 
            this.tbPORT.Location = new System.Drawing.Point(247, 174);
            this.tbPORT.Name = "tbPORT";
            this.tbPORT.Size = new System.Drawing.Size(75, 20);
            this.tbPORT.TabIndex = 11;
            this.tbPORT.Text = "8080";
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 348);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(690, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStatus
            // 
            this.toolStatus.Name = "toolStatus";
            this.toolStatus.Size = new System.Drawing.Size(45, 17);
            this.toolStatus.Text = "Status: ";
            // 
            // btnGetRoutingTable
            // 
            this.btnGetRoutingTable.Location = new System.Drawing.Point(47, 200);
            this.btnGetRoutingTable.Name = "btnGetRoutingTable";
            this.btnGetRoutingTable.Size = new System.Drawing.Size(156, 23);
            this.btnGetRoutingTable.TabIndex = 18;
            this.btnGetRoutingTable.Text = "Get routing table";
            this.btnGetRoutingTable.UseVisualStyleBackColor = true;
            this.btnGetRoutingTable.Click += new System.EventHandler(this.btnGetRoutingTable_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(47, 229);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(156, 23);
            this.btnSendFile.TabIndex = 19;
            this.btnSendFile.Text = "Share File";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.Location = new System.Drawing.Point(208, 200);
            this.btnLeave.Margin = new System.Windows.Forms.Padding(2);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(114, 23);
            this.btnLeave.TabIndex = 20;
            this.btnLeave.Text = "Leave";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.button1_Click);
            // 
            // listAvaliableFiles
            // 
            this.listAvaliableFiles.FormattingEnabled = true;
            this.listAvaliableFiles.Location = new System.Drawing.Point(340, 171);
            this.listAvaliableFiles.Name = "listAvaliableFiles";
            this.listAvaliableFiles.Size = new System.Drawing.Size(325, 121);
            this.listAvaliableFiles.TabIndex = 21;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(509, 311);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnRequestFile
            // 
            this.btnRequestFile.Location = new System.Drawing.Point(590, 311);
            this.btnRequestFile.Name = "btnRequestFile";
            this.btnRequestFile.Size = new System.Drawing.Size(75, 23);
            this.btnRequestFile.TabIndex = 23;
            this.btnRequestFile.Text = "download";
            this.btnRequestFile.UseVisualStyleBackColor = true;
            this.btnRequestFile.Click += new System.EventHandler(this.btnRequestFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "GUID";
            // 
            // listTransfers
            // 
            this.listTransfers.FormattingEnabled = true;
            this.listTransfers.Location = new System.Drawing.Point(340, 41);
            this.listTransfers.Name = "listTransfers";
            this.listTransfers.Size = new System.Drawing.Size(325, 95);
            this.listTransfers.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Avaliable files";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(338, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Transfers";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 258);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Share folder";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 370);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listTransfers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRequestFile);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.listAvaliableFiles);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnGetRoutingTable);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbPORT);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.tbGUID);
            this.Controls.Add(this.buttonPing);
            this.Controls.Add(this.lblGUID);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelGUID);
            this.Controls.Add(this.listRoutingTable);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listRoutingTable;
        private System.Windows.Forms.Label labelGUID;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label lblGUID;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.TextBox tbGUID;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPORT;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStatus;
        private System.Windows.Forms.Button btnGetRoutingTable;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.ListBox listAvaliableFiles;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnRequestFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listTransfers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}

