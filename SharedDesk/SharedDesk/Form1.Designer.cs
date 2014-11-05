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
            this.labelID = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonPing = new System.Windows.Forms.Button();
            this.textTarget = new System.Windows.Forms.TextBox();
            this.listResponses = new System.Windows.Forms.ListBox();
            this.labelResponse = new System.Windows.Forms.Label();
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
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Location = new System.Drawing.Point(44, 148);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(61, 13);
            this.labelID.TabIndex = 4;
            this.labelID.Text = "TempGUID";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(94, 171);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 5;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            // 
            // buttonPing
            // 
            this.buttonPing.Location = new System.Drawing.Point(184, 171);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(75, 23);
            this.buttonPing.TabIndex = 6;
            this.buttonPing.Text = "Ping";
            this.buttonPing.UseVisualStyleBackColor = true;
            // 
            // textTarget
            // 
            this.textTarget.Location = new System.Drawing.Point(47, 174);
            this.textTarget.Name = "textTarget";
            this.textTarget.Size = new System.Drawing.Size(31, 20);
            this.textTarget.TabIndex = 7;
            // 
            // listResponses
            // 
            this.listResponses.FormattingEnabled = true;
            this.listResponses.Location = new System.Drawing.Point(321, 41);
            this.listResponses.Name = "listResponses";
            this.listResponses.Size = new System.Drawing.Size(212, 95);
            this.listResponses.TabIndex = 8;
            // 
            // labelResponse
            // 
            this.labelResponse.AutoSize = true;
            this.labelResponse.Location = new System.Drawing.Point(318, 25);
            this.labelResponse.Name = "labelResponse";
            this.labelResponse.Size = new System.Drawing.Size(60, 13);
            this.labelResponse.TabIndex = 9;
            this.labelResponse.Text = "Responses";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 355);
            this.Controls.Add(this.labelResponse);
            this.Controls.Add(this.listResponses);
            this.Controls.Add(this.textTarget);
            this.Controls.Add(this.buttonPing);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.labelID);
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
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.TextBox textTarget;
        private System.Windows.Forms.ListBox listResponses;
        private System.Windows.Forms.Label labelResponse;
    }
}

