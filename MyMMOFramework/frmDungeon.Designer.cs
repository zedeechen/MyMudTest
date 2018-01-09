namespace GameSample
{
    partial class frmDungeon
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
            this.txtEventLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtEventLog
            // 
            this.txtEventLog.Location = new System.Drawing.Point(12, 181);
            this.txtEventLog.Name = "txtEventLog";
            this.txtEventLog.Size = new System.Drawing.Size(346, 134);
            this.txtEventLog.TabIndex = 0;
            this.txtEventLog.Text = "";
            // 
            // frmDungeon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 358);
            this.ControlBox = false;
            this.Controls.Add(this.txtEventLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDungeon";
            this.Text = "Dungeon";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtEventLog;
    }
}