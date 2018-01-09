namespace GameSample
{
    partial class frmHome
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
            this.btnRaid = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRaid
            // 
            this.btnRaid.Location = new System.Drawing.Point(48, 117);
            this.btnRaid.Name = "btnRaid";
            this.btnRaid.Size = new System.Drawing.Size(75, 41);
            this.btnRaid.TabIndex = 0;
            this.btnRaid.Text = "Explore";
            this.btnRaid.UseVisualStyleBackColor = true;
            this.btnRaid.Click += new System.EventHandler(this.btnRaid_Click);
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 352);
            this.ControlBox = false;
            this.Controls.Add(this.btnRaid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHome";
            this.Text = "frmHome";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRaid;
    }
}