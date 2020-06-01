namespace JobTracker.Login
{
    partial class FrmJTLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmJTLogin));
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtJTUserName = new System.Windows.Forms.TextBox();
            this.txtJTPassword = new System.Windows.Forms.TextBox();
            this.cbIsTestDb = new System.Windows.Forms.CheckBox();
            this.btnLoginJT = new System.Windows.Forms.Button();
            this.btnLoginCancelJT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblUserName.Location = new System.Drawing.Point(41, 67);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(3);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(98, 20);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "User Name";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblPassword.Location = new System.Drawing.Point(41, 123);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(86, 20);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password";
            // 
            // txtJTUserName
            // 
            this.txtJTUserName.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJTUserName.Location = new System.Drawing.Point(178, 67);
            this.txtJTUserName.Name = "txtJTUserName";
            this.txtJTUserName.Size = new System.Drawing.Size(233, 29);
            this.txtJTUserName.TabIndex = 3;
            // 
            // txtJTPassword
            // 
            this.txtJTPassword.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJTPassword.Location = new System.Drawing.Point(178, 123);
            this.txtJTPassword.Name = "txtJTPassword";
            this.txtJTPassword.Size = new System.Drawing.Size(233, 29);
            this.txtJTPassword.TabIndex = 3;
            // 
            // cbIsTestDb
            // 
            this.cbIsTestDb.AutoSize = true;
            this.cbIsTestDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIsTestDb.Location = new System.Drawing.Point(178, 168);
            this.cbIsTestDb.Name = "cbIsTestDb";
            this.cbIsTestDb.Size = new System.Drawing.Size(169, 21);
            this.cbIsTestDb.TabIndex = 4;
            this.cbIsTestDb.Text = "Use Test Database";
            this.cbIsTestDb.UseVisualStyleBackColor = true;
            // 
            // btnLoginJT
            // 
            this.btnLoginJT.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnLoginJT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoginJT.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoginJT.Location = new System.Drawing.Point(178, 218);
            this.btnLoginJT.Name = "btnLoginJT";
            this.btnLoginJT.Size = new System.Drawing.Size(95, 32);
            this.btnLoginJT.TabIndex = 5;
            this.btnLoginJT.Text = "Login";
            this.btnLoginJT.UseVisualStyleBackColor = true;
            this.btnLoginJT.Click += new System.EventHandler(this.BtnLoginJT_Click);
            // 
            // btnLoginCancelJT
            // 
            this.btnLoginCancelJT.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnLoginCancelJT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoginCancelJT.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoginCancelJT.Location = new System.Drawing.Point(307, 218);
            this.btnLoginCancelJT.Name = "btnLoginCancelJT";
            this.btnLoginCancelJT.Size = new System.Drawing.Size(104, 32);
            this.btnLoginCancelJT.TabIndex = 6;
            this.btnLoginCancelJT.Text = "Cancel";
            this.btnLoginCancelJT.UseVisualStyleBackColor = true;
            this.btnLoginCancelJT.Click += new System.EventHandler(this.BtnLoginCancelJT_Click);
            // 
            // FrmJTLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(484, 293);
            this.Controls.Add(this.btnLoginCancelJT);
            this.Controls.Add(this.btnLoginJT);
            this.Controls.Add(this.cbIsTestDb);
            this.Controls.Add(this.txtJTPassword);
            this.Controls.Add(this.txtJTUserName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmJTLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Job Tracker Login";
            this.Load += new System.EventHandler(this.FrmJTLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtJTUserName;
        private System.Windows.Forms.TextBox txtJTPassword;
        private System.Windows.Forms.CheckBox cbIsTestDb;
        private System.Windows.Forms.Button btnLoginJT;
        private System.Windows.Forms.Button btnLoginCancelJT;
    }
}