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
            this.tbGUID = new System.Windows.Forms.TextBox();
            this.labelResponse = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbPORT = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnGetRoutingTable = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();
            this.tbTarget = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbMessages = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
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
            this.lblGUID.Location = new System.Drawing.Point(44, 171);
            this.lblGUID.Name = "lblGUID";
            this.lblGUID.Size = new System.Drawing.Size(34, 13);
            this.lblGUID.TabIndex = 4;
            this.lblGUID.Text = "GUID";
            // 
            // tbGUID
            // 
            this.tbGUID.Location = new System.Drawing.Point(47, 187);
            this.tbGUID.Name = "tbGUID";
            this.tbGUID.Size = new System.Drawing.Size(66, 20);
            this.tbGUID.TabIndex = 7;
            this.tbGUID.Text = "0";
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
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(128, 187);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(104, 20);
            this.tbIP.TabIndex = 10;
            this.tbIP.Text = "127.0.0.1";
            // 
            // tbPORT
            // 
            this.tbPORT.Location = new System.Drawing.Point(247, 187);
            this.tbPORT.Name = "tbPORT";
            this.tbPORT.Size = new System.Drawing.Size(75, 20);
            this.tbPORT.TabIndex = 11;
            this.tbPORT.Text = "8080";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(130, 171);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(17, 13);
            this.lblIP.TabIndex = 12;
            this.lblIP.Text = "IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "port";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 366);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(582, 22);
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
            this.btnGetRoutingTable.Location = new System.Drawing.Point(47, 213);
            this.btnGetRoutingTable.Name = "btnGetRoutingTable";
            this.btnGetRoutingTable.Size = new System.Drawing.Size(156, 23);
            this.btnGetRoutingTable.TabIndex = 18;
            this.btnGetRoutingTable.Text = "Get routing table";
            this.btnGetRoutingTable.UseVisualStyleBackColor = true;
            this.btnGetRoutingTable.Click += new System.EventHandler(this.btnGetRoutingTable_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(47, 242);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(156, 23);
            this.btnSendFile.TabIndex = 19;
            this.btnSendFile.Text = "Send File";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.Location = new System.Drawing.Point(247, 213);
            this.btnLeave.Margin = new System.Windows.Forms.Padding(2);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(75, 23);
            this.btnLeave.TabIndex = 20;
            this.btnLeave.Text = "Leave";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbTarget
            // 
            this.tbTarget.Location = new System.Drawing.Point(47, 271);
            this.tbTarget.Name = "tbTarget";
            this.tbTarget.Size = new System.Drawing.Size(100, 20);
            this.tbTarget.TabIndex = 21;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(47, 297);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 22;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbMessages
            // 
            this.tbMessages.Location = new System.Drawing.Point(340, 41);
            this.tbMessages.Multiline = true;
            this.tbMessages.Name = "tbMessages";
            this.tbMessages.Size = new System.Drawing.Size(230, 279);
            this.tbMessages.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 388);
            this.Controls.Add(this.tbMessages);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.tbTarget);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnGetRoutingTable);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbPORT);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.labelResponse);
            this.Controls.Add(this.tbGUID);
            this.Controls.Add(this.lblGUID);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelIP);
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
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label lblGUID;
        private System.Windows.Forms.TextBox tbGUID;
        private System.Windows.Forms.Label labelResponse;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPORT;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStatus;
        private System.Windows.Forms.Button btnGetRoutingTable;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.TextBox tbTarget;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox tbMessages;
    }
}

