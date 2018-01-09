namespace GameSample
{
    partial class frmCreateRole
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
            this.panelStep3 = new System.Windows.Forms.Panel();
            this.btnDone = new System.Windows.Forms.Button();
            this.lbRemainPoints = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbRace = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbClass = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStep1 = new System.Windows.Forms.Panel();
            this.panelStep2 = new System.Windows.Forms.Panel();
            this.panelStep3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStep3
            // 
            this.panelStep3.Controls.Add(this.btnDone);
            this.panelStep3.Controls.Add(this.lbRemainPoints);
            this.panelStep3.Controls.Add(this.label3);
            this.panelStep3.Controls.Add(this.lbRace);
            this.panelStep3.Controls.Add(this.label4);
            this.panelStep3.Controls.Add(this.lbClass);
            this.panelStep3.Controls.Add(this.label1);
            this.panelStep3.Location = new System.Drawing.Point(35, 78);
            this.panelStep3.Name = "panelStep3";
            this.panelStep3.Size = new System.Drawing.Size(276, 280);
            this.panelStep3.TabIndex = 0;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(245, 257);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(71, 23);
            this.btnDone.TabIndex = 30;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lbRemainPoints
            // 
            this.lbRemainPoints.AutoSize = true;
            this.lbRemainPoints.Location = new System.Drawing.Point(116, 48);
            this.lbRemainPoints.Name = "lbRemainPoints";
            this.lbRemainPoints.Size = new System.Drawing.Size(17, 12);
            this.lbRemainPoints.TabIndex = 9;
            this.lbRemainPoints.Text = "28";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Points Remain:";
            // 
            // lbRace
            // 
            this.lbRace.AutoSize = true;
            this.lbRace.Location = new System.Drawing.Point(205, 12);
            this.lbRace.Name = "lbRace";
            this.lbRace.Size = new System.Drawing.Size(0, 12);
            this.lbRace.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(158, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Race:";
            // 
            // lbClass
            // 
            this.lbClass.AutoSize = true;
            this.lbClass.Location = new System.Drawing.Point(68, 12);
            this.lbClass.Name = "lbClass";
            this.lbClass.Size = new System.Drawing.Size(0, 12);
            this.lbClass.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Class:";
            // 
            // panelStep1
            // 
            this.panelStep1.Location = new System.Drawing.Point(12, 203);
            this.panelStep1.Name = "panelStep1";
            this.panelStep1.Size = new System.Drawing.Size(350, 305);
            this.panelStep1.TabIndex = 1;
            this.panelStep1.Visible = false;
            // 
            // panelStep2
            // 
            this.panelStep2.Location = new System.Drawing.Point(56, -28);
            this.panelStep2.Name = "panelStep2";
            this.panelStep2.Size = new System.Drawing.Size(200, 100);
            this.panelStep2.TabIndex = 2;
            // 
            // frmCreateRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 304);
            this.ControlBox = false;
            this.Controls.Add(this.panelStep2);
            this.Controls.Add(this.panelStep3);
            this.Controls.Add(this.panelStep1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateRole";
            this.Text = "Create Role";
            this.Load += new System.EventHandler(this.frmCreateRole_Load);
            this.panelStep3.ResumeLayout(false);
            this.panelStep3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelStep3;
        private System.Windows.Forms.Label lbRace;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbRemainPoints;
        private System.Windows.Forms.Panel panelStep1;
        private System.Windows.Forms.Panel panelStep2;
        private System.Windows.Forms.Button btnDone;
    }
}