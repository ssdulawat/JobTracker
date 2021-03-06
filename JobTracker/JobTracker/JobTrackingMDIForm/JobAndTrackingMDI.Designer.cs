﻿namespace JobTracker.JobTrackingMDIForm
{
    partial class JobAndTrackingMDI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobAndTrackingMDI));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.calendarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTImeExpenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dbaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jTListItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoiceToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbkApprovedVersion = new System.Windows.Forms.LinkLabel();
            this.btnCloasAll = new System.Windows.Forms.Button();
            this.lblLogin = new System.Windows.Forms.Button();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.linklbl = new System.Windows.Forms.LinkLabel();
            this.lnlLblNewVersion = new System.Windows.Forms.LinkLabel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.NtyicnJT = new System.Windows.Forms.NotifyIcon(this.components);
            this.CMSNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TimerDateTime = new System.Windows.Forms.Timer(this.components);
            this.timerGet = new System.Windows.Forms.Timer(this.components);
            this.BackWorkerEmail = new System.ComponentModel.BackgroundWorker();
            this.tabctrlFrm = new DevComponents.DotNetBar.TabControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlFrm)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calendarToolStripMenuItem,
            this.managerToolStripMenuItem,
            this.addTImeExpenseToolStripMenuItem,
            this.contactToolStripMenuItem,
            this.dbaseToolStripMenuItem,
            this.jTListItemToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.invoiceToolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1010, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // calendarToolStripMenuItem
            // 
            this.calendarToolStripMenuItem.Name = "calendarToolStripMenuItem";
            this.calendarToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.calendarToolStripMenuItem.Text = "Calendar";
            this.calendarToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // managerToolStripMenuItem
            // 
            this.managerToolStripMenuItem.Name = "managerToolStripMenuItem";
            this.managerToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.managerToolStripMenuItem.Text = "Manager";
            // 
            // addTImeExpenseToolStripMenuItem
            // 
            this.addTImeExpenseToolStripMenuItem.Name = "addTImeExpenseToolStripMenuItem";
            this.addTImeExpenseToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.addTImeExpenseToolStripMenuItem.Text = "Add Time/Expense";
            // 
            // contactToolStripMenuItem
            // 
            this.contactToolStripMenuItem.Name = "contactToolStripMenuItem";
            this.contactToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            this.contactToolStripMenuItem.Text = "Contacts";
            // 
            // dbaseToolStripMenuItem
            // 
            this.dbaseToolStripMenuItem.Name = "dbaseToolStripMenuItem";
            this.dbaseToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.dbaseToolStripMenuItem.Text = "Dbase";
            // 
            // jTListItemToolStripMenuItem
            // 
            this.jTListItemToolStripMenuItem.Name = "jTListItemToolStripMenuItem";
            this.jTListItemToolStripMenuItem.Size = new System.Drawing.Size(96, 24);
            this.jTListItemToolStripMenuItem.Text = "JT List Item";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // invoiceToolsToolStripMenuItem
            // 
            this.invoiceToolsToolStripMenuItem.Name = "invoiceToolsToolStripMenuItem";
            this.invoiceToolsToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.invoiceToolsToolStripMenuItem.Text = "Invoice Tools";
            // 
            // lbkApprovedVersion
            // 
            this.lbkApprovedVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbkApprovedVersion.AutoSize = true;
            this.lbkApprovedVersion.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lbkApprovedVersion.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbkApprovedVersion.ForeColor = System.Drawing.Color.Maroon;
            this.lbkApprovedVersion.LinkColor = System.Drawing.Color.Maroon;
            this.lbkApprovedVersion.Location = new System.Drawing.Point(723, 9);
            this.lbkApprovedVersion.Name = "lbkApprovedVersion";
            this.lbkApprovedVersion.Size = new System.Drawing.Size(119, 18);
            this.lbkApprovedVersion.TabIndex = 3;
            this.lbkApprovedVersion.TabStop = true;
            this.lbkApprovedVersion.Text = "Approved Version";
            // 
            // btnCloasAll
            // 
            this.btnCloasAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloasAll.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnCloasAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloasAll.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloasAll.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCloasAll.Location = new System.Drawing.Point(854, 4);
            this.btnCloasAll.Name = "btnCloasAll";
            this.btnCloasAll.Size = new System.Drawing.Size(75, 29);
            this.btnCloasAll.TabIndex = 4;
            this.btnCloasAll.Text = "Close All";
            this.btnCloasAll.UseVisualStyleBackColor = false;
            // 
            // lblLogin
            // 
            this.lblLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogin.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblLogin.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.lblLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLogin.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.Location = new System.Drawing.Point(929, 4);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(76, 29);
            this.lblLogin.TabIndex = 9;
            this.lblLogin.Text = "Admin Login";
            this.lblLogin.UseMnemonic = false;
            this.lblLogin.UseVisualStyleBackColor = false;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonPanel1.Controls.Add(this.linklbl);
            this.kryptonPanel1.Controls.Add(this.lnlLblNewVersion);
            this.kryptonPanel1.Controls.Add(this.lblVersion);
            this.kryptonPanel1.Controls.Add(this.lblTime);
            this.kryptonPanel1.Controls.Add(this.lblDate);
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 484);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Blue;
            this.kryptonPanel1.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.HeaderCustom2;
            this.kryptonPanel1.Size = new System.Drawing.Size(1012, 31);
            this.kryptonPanel1.TabIndex = 11;
            // 
            // linklbl
            // 
            this.linklbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linklbl.AutoSize = true;
            this.linklbl.BackColor = System.Drawing.Color.Transparent;
            this.linklbl.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linklbl.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linklbl.Location = new System.Drawing.Point(883, 7);
            this.linklbl.Name = "linklbl";
            this.linklbl.Size = new System.Drawing.Size(115, 18);
            this.linklbl.TabIndex = 4;
            this.linklbl.TabStop = true;
            this.linklbl.Text = "www.valjato.com";
            // 
            // lnlLblNewVersion
            // 
            this.lnlLblNewVersion.AutoSize = true;
            this.lnlLblNewVersion.BackColor = System.Drawing.Color.Transparent;
            this.lnlLblNewVersion.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(74)))), ((int)(((byte)(116)))));
            this.lnlLblNewVersion.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnlLblNewVersion.ForeColor = System.Drawing.Color.Maroon;
            this.lnlLblNewVersion.LinkColor = System.Drawing.Color.Maroon;
            this.lnlLblNewVersion.Location = new System.Drawing.Point(592, 7);
            this.lnlLblNewVersion.Name = "lnlLblNewVersion";
            this.lnlLblNewVersion.Size = new System.Drawing.Size(124, 18);
            this.lnlLblNewVersion.TabIndex = 3;
            this.lnlLblNewVersion.TabStop = true;
            this.lnlLblNewVersion.Text = "Check new version";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Enabled = false;
            this.lblVersion.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblVersion.Location = new System.Drawing.Point(430, 7);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(52, 17);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblTime.Location = new System.Drawing.Point(235, 7);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(36, 17);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "Time";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblDate.Location = new System.Drawing.Point(36, 7);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(36, 17);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            // 
            // NtyicnJT
            // 
            this.NtyicnJT.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NtyicnJT.BalloonTipTitle = "Job Tracking";
            this.NtyicnJT.ContextMenuStrip = this.CMSNotify;
            this.NtyicnJT.Icon = ((System.Drawing.Icon)(resources.GetObject("NtyicnJT.Icon")));
            this.NtyicnJT.Text = "Job Tracking";
            this.NtyicnJT.Visible = true;
            this.NtyicnJT.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NtyicnJT_MouseClick);
            // 
            // CMSNotify
            // 
            this.CMSNotify.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CMSNotify.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CMSNotify.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CMSNotify.Name = "CMSNotify";
            this.CMSNotify.Size = new System.Drawing.Size(61, 4);
            // 
            // TimerDateTime
            // 
            this.TimerDateTime.Enabled = true;
            this.TimerDateTime.Interval = 1000;
            this.TimerDateTime.Tick += new System.EventHandler(this.TimerDateTime_Tick);
            // 
            // timerGet
            // 
            this.timerGet.Enabled = true;
            this.timerGet.Interval = 120000;
            this.timerGet.Tick += new System.EventHandler(this.TimerGet_Tick);
            // 
            // BackWorkerEmail
            // 
            this.BackWorkerEmail.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackWorkerEmail_DoWork);
            // 
            // tabctrlFrm
            // 
            this.tabctrlFrm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(228)))), ((int)(((byte)(250)))));
            this.tabctrlFrm.CanReorderTabs = true;
            this.tabctrlFrm.CloseButtonOnTabsVisible = true;
            this.tabctrlFrm.CloseButtonPosition = DevComponents.DotNetBar.eTabCloseButtonPosition.Right;
            this.tabctrlFrm.CloseButtonVisible = true;
            this.tabctrlFrm.ColorScheme.TabItemHotBackground2 = System.Drawing.Color.Gold;
            this.tabctrlFrm.ColorScheme.TabItemSelectedBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(214)))), ((int)(((byte)(246)))));
            this.tabctrlFrm.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabctrlFrm.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabctrlFrm.Location = new System.Drawing.Point(0, 28);
            this.tabctrlFrm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabctrlFrm.Name = "tabctrlFrm";
            this.tabctrlFrm.SelectedTabFont = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabctrlFrm.SelectedTabIndex = -1;
            this.tabctrlFrm.Size = new System.Drawing.Size(1010, 25);
            this.tabctrlFrm.TabIndex = 16;
            this.tabctrlFrm.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabctrlFrm.Text = "Tab Open Form";
            this.tabctrlFrm.SelectedTabChanged += new DevComponents.DotNetBar.TabStrip.SelectedTabChangedEventHandler(this.TabctrlFrm_SelectedTabChanged);
            this.tabctrlFrm.TabItemClose += new DevComponents.DotNetBar.TabStrip.UserActionEventHandler(this.TabctrlFrm_TabItemClose);
            // 
            // JobAndTrackingMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1010, 512);
            this.Controls.Add(this.tabctrlFrm);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.btnCloasAll);
            this.Controls.Add(this.lbkApprovedVersion);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "JobAndTrackingMDI";
            this.Text = "Job Tracking";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JobAndTrackingMDI_FormClosing);
            this.Load += new System.EventHandler(this.JobAndTrackingMDI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlFrm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem calendarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTImeExpenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contactToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dbaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jTListItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invoiceToolsToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lbkApprovedVersion;
        private System.Windows.Forms.Button btnCloasAll;
        public System.Windows.Forms.Button lblLogin;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.LinkLabel linklbl;
        private System.Windows.Forms.LinkLabel lnlLblNewVersion;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.NotifyIcon NtyicnJT;
        private System.Windows.Forms.ContextMenuStrip CMSNotify;
        private System.Windows.Forms.Timer TimerDateTime;
        private System.Windows.Forms.Timer timerGet;
        private System.ComponentModel.BackgroundWorker BackWorkerEmail;
        internal DevComponents.DotNetBar.TabControl tabctrlFrm;
    }
}