namespace StrategyPatternExample
{
    partial class FileTransfer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioTCPv3 = new System.Windows.Forms.RadioButton();
            this.radioTCPv2 = new System.Windows.Forms.RadioButton();
            this.radioTCP = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.tbSendIP = new System.Windows.Forms.TextBox();
            this.tbSendPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbReceivePort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.btnSelectSaveFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSaveFolder = new System.Windows.Forms.TextBox();
            this.tbReceiveIP = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbFileTransfers = new System.Windows.Forms.ListBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioTCPv3);
            this.groupBox1.Controls.Add(this.radioTCPv2);
            this.groupBox1.Controls.Add(this.radioTCP);
            this.groupBox1.Location = new System.Drawing.Point(12, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method";
            // 
            // radioTCPv3
            // 
            this.radioTCPv3.AutoSize = true;
            this.radioTCPv3.Checked = true;
            this.radioTCPv3.Location = new System.Drawing.Point(12, 29);
            this.radioTCPv3.Name = "radioTCPv3";
            this.radioTCPv3.Size = new System.Drawing.Size(58, 17);
            this.radioTCPv3.TabIndex = 4;
            this.radioTCPv3.TabStop = true;
            this.radioTCPv3.Text = "TCPv3";
            this.radioTCPv3.UseVisualStyleBackColor = true;
            this.radioTCPv3.CheckedChanged += new System.EventHandler(this.radioTCPv3_CheckedChanged);
            // 
            // radioTCPv2
            // 
            this.radioTCPv2.AutoSize = true;
            this.radioTCPv2.Location = new System.Drawing.Point(97, 29);
            this.radioTCPv2.Name = "radioTCPv2";
            this.radioTCPv2.Size = new System.Drawing.Size(58, 17);
            this.radioTCPv2.TabIndex = 3;
            this.radioTCPv2.Text = "TCPv2";
            this.radioTCPv2.UseVisualStyleBackColor = true;
            this.radioTCPv2.CheckedChanged += new System.EventHandler(this.radioTCPv2_CheckedChanged);
            // 
            // radioTCP
            // 
            this.radioTCP.AutoSize = true;
            this.radioTCP.Location = new System.Drawing.Point(174, 29);
            this.radioTCP.Name = "radioTCP";
            this.radioTCP.Size = new System.Drawing.Size(46, 17);
            this.radioTCP.TabIndex = 1;
            this.radioTCP.Text = "TCP";
            this.radioTCP.UseVisualStyleBackColor = true;
            this.radioTCP.CheckedChanged += new System.EventHandler(this.radioTCP_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectFile);
            this.groupBox2.Controls.Add(this.tbSendIP);
            this.groupBox2.Controls.Add(this.tbSendPort);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 98);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(23, 58);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(120, 23);
            this.btnSelectFile.TabIndex = 11;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // tbSendIP
            // 
            this.tbSendIP.Location = new System.Drawing.Point(73, 15);
            this.tbSendIP.Name = "tbSendIP";
            this.tbSendIP.Size = new System.Drawing.Size(155, 20);
            this.tbSendIP.TabIndex = 10;
            // 
            // tbSendPort
            // 
            this.tbSendPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSendPort.Location = new System.Drawing.Point(234, 15);
            this.tbSendPort.Name = "tbSendPort";
            this.tbSendPort.Size = new System.Drawing.Size(65, 22);
            this.tbSendPort.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "IP: ";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(173, 58);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(120, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send File";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbReceivePort
            // 
            this.tbReceivePort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReceivePort.Location = new System.Drawing.Point(234, 19);
            this.tbReceivePort.Name = "tbReceivePort";
            this.tbReceivePort.Size = new System.Drawing.Size(65, 22);
            this.tbReceivePort.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP: ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.tbFileName);
            this.groupBox3.Controls.Add(this.btnSelectSaveFolder);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.tbSaveFolder);
            this.groupBox3.Controls.Add(this.tbReceiveIP);
            this.groupBox3.Controls.Add(this.btnListen);
            this.groupBox3.Controls.Add(this.tbReceivePort);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(313, 193);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Listen for connection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "FileName:";
            // 
            // tbFileName
            // 
            this.tbFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFileName.Location = new System.Drawing.Point(25, 119);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(273, 20);
            this.tbFileName.TabIndex = 10;
            // 
            // btnSelectSaveFolder
            // 
            this.btnSelectSaveFolder.Location = new System.Drawing.Point(273, 66);
            this.btnSelectSaveFolder.Name = "btnSelectSaveFolder";
            this.btnSelectSaveFolder.Size = new System.Drawing.Size(26, 23);
            this.btnSelectSaveFolder.TabIndex = 9;
            this.btnSelectSaveFolder.Text = "...";
            this.btnSelectSaveFolder.UseVisualStyleBackColor = true;
            this.btnSelectSaveFolder.Click += new System.EventHandler(this.btnSelectSaveFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Save folder:";
            // 
            // tbSaveFolder
            // 
            this.tbSaveFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSaveFolder.Location = new System.Drawing.Point(23, 69);
            this.tbSaveFolder.Name = "tbSaveFolder";
            this.tbSaveFolder.Size = new System.Drawing.Size(244, 20);
            this.tbSaveFolder.TabIndex = 7;
            // 
            // tbReceiveIP
            // 
            this.tbReceiveIP.Enabled = false;
            this.tbReceiveIP.Location = new System.Drawing.Point(73, 19);
            this.tbReceiveIP.Name = "tbReceiveIP";
            this.tbReceiveIP.Size = new System.Drawing.Size(155, 20);
            this.tbReceiveIP.TabIndex = 6;
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(177, 154);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(121, 23);
            this.btnListen.TabIndex = 5;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 512);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(335, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(45, 17);
            this.lblStatus.Text = "Status: ";
            // 
            // lbFileTransfers
            // 
            this.lbFileTransfers.FormattingEnabled = true;
            this.lbFileTransfers.Location = new System.Drawing.Point(12, 391);
            this.lbFileTransfers.Name = "lbFileTransfers";
            this.lbFileTransfers.Size = new System.Drawing.Size(313, 95);
            this.lbFileTransfers.TabIndex = 6;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // FileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 534);
            this.Controls.Add(this.lbFileTransfers);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileTransfer";
            this.Text = "Send and Receive files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileTransfer_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioTCP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbReceivePort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.TextBox tbReceiveIP;
        private System.Windows.Forms.Button btnSelectSaveFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSaveFolder;
        private System.Windows.Forms.TextBox tbSendIP;
        private System.Windows.Forms.TextBox tbSendPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.RadioButton radioTCPv2;
        private System.Windows.Forms.ListBox lbFileTransfers;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.RadioButton radioTCPv3;
    }
}

