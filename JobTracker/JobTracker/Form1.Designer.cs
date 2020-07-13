namespace JobTracker
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
            this.panelFileBrowser = new System.Windows.Forms.Panel();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.DriveListBox1 = new System.Windows.Forms.ComboBox();
            this.btnPermitsFileDownload = new System.Windows.Forms.Button();
            this.panelFileBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFileBrowser
            // 
            this.panelFileBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFileBrowser.Controls.Add(this.listBox2);
            this.panelFileBrowser.Controls.Add(this.listBox1);
            this.panelFileBrowser.Controls.Add(this.DriveListBox1);
            this.panelFileBrowser.Controls.Add(this.btnPermitsFileDownload);
            this.panelFileBrowser.Location = new System.Drawing.Point(295, 73);
            this.panelFileBrowser.Name = "panelFileBrowser";
            this.panelFileBrowser.Size = new System.Drawing.Size(211, 305);
            this.panelFileBrowser.TabIndex = 2;
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(12, 205);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(191, 82);
            this.listBox2.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 90);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(191, 108);
            this.listBox1.TabIndex = 2;
            // 
            // DriveListBox1
            // 
            this.DriveListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DriveListBox1.FormattingEnabled = true;
            this.DriveListBox1.Location = new System.Drawing.Point(12, 59);
            this.DriveListBox1.Name = "DriveListBox1";
            this.DriveListBox1.Size = new System.Drawing.Size(191, 21);
            this.DriveListBox1.TabIndex = 1;
            // 
            // btnPermitsFileDownload
            // 
            this.btnPermitsFileDownload.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnPermitsFileDownload.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnPermitsFileDownload.Location = new System.Drawing.Point(12, 10);
            this.btnPermitsFileDownload.Name = "btnPermitsFileDownload";
            this.btnPermitsFileDownload.Size = new System.Drawing.Size(190, 42);
            this.btnPermitsFileDownload.TabIndex = 0;
            this.btnPermitsFileDownload.Text = "Permits/File Download";
            this.btnPermitsFileDownload.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelFileBrowser);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelFileBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFileBrowser;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox DriveListBox1;
        private System.Windows.Forms.Button btnPermitsFileDownload;
    }
}

