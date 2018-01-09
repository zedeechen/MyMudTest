using GameSample.Common.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZDMMO;

namespace GameSample
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            InitFixedUI();
            Console.WriteLine(SingletonFactory<UserInfo>.Instance.GetHero(0).GetRace());
        }
        private void InitFixedUI()
        {
            UIHeroSummary control = new UIHeroSummary(SingletonFactory<UserInfo>.Instance.GetHero(0));
            control.Location = new Point(30, 30);
            this.Controls.Add(control);
        }
        private void btnRaid_Click(object sender, EventArgs e)
        {
            SingletonFactory<GameController>.Instance.EnterDungeon();
        }
    }
}
