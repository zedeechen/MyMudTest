using System;
using System.Windows.Forms;

namespace GameSample.Common.UserControls
{
    public class UIAddMinusControl : UserControl
    {
        private Label lbName;
        private Button  btnAdd;
        private Button  btnMinus;
        private Label lbValue;
        private Label lbValueMod;

        public UIAddMinusControl(string name, EventHandler minusAction, EventHandler addAction, int tag)
        {
            InitializeComponent();

            lbName.Text = name;

            btnMinus.Tag = tag;
            btnAdd.Tag = tag;
            btnMinus.Click += minusAction;
            btnAdd.Click += addAction;
        }
        
        public void SetLabelValue(int value)
        {
            lbValue.Text = value.ToString();
        }
        public void SetLabelModValue(int value)
        {
            if (value > 0)
            {
                lbValueMod.Text = string.Format("(+{0})", value);
            }
            else
            {
                lbValueMod.Text = string.Format("({0})", value);
            }
            
        }

        private void InitializeComponent()
        {
            lbName = new Label();
            lbName.Location = new System.Drawing.Point(0, 0);
            lbName.Size = new System.Drawing.Size(50, 20);

            btnMinus = new Button();
            btnMinus.Location = new System.Drawing.Point(50, 0);
            btnMinus.Size = new System.Drawing.Size(20, 20);
            btnMinus.Text = "-";

            lbValue = new Label();
            lbValue.Location = new System.Drawing.Point(100, 0);
            lbValue.Size = new System.Drawing.Size(50, 20);

            btnAdd = new Button();
            btnAdd.Location = new System.Drawing.Point(150, 0);
            btnAdd.Size = new System.Drawing.Size(20, 20);
            btnAdd.Text = "+";

            lbValueMod = new Label();
            lbValueMod.Location = new System.Drawing.Point(200, 0);
            lbValueMod.Size = new System.Drawing.Size(50, 20);

            this.Size = new System.Drawing.Size(250, 20);

            this.Controls.Add(lbName);
            this.Controls.Add(btnMinus);
            this.Controls.Add(lbValue);
            this.Controls.Add(btnAdd);
            this.Controls.Add(lbValueMod);
        }
    }
}
