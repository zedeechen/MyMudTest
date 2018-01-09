using GameSample.Common.UserControls;
using GameSample.Dungeon;
using System;
using System.Drawing;
using System.Windows.Forms;
using ZDMMO;

namespace GameSample
{
    public partial class frmDungeon : Form
    {
        private DungeonInfo mDungeon;
        private int mIndex = 0;

        public frmDungeon()
        {
            InitializeComponent();
            InitFixedUI();

            Activated += FrmDungeon_Activated;

            DoRefreshUI();
        }

        private void InitFixedUI()
        {
            UIHeroSummary control = new UIHeroSummary(SingletonFactory<UserInfo>.Instance.GetHero(0));
            control.Location = new Point(30, 30);
            this.Controls.Add(control);

            Button btnNext = new Button();
            btnNext.Text = "Go Ahead";
            btnNext.Click += BtnNext_Click;
            btnNext.Size = new Size(50, 20);
            btnNext.Location = new Point(100, 140);
            Controls.Add(btnNext);

            Button btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Click += BtnExit_Click;
            btnExit.Size = new Size(50, 20);
            btnExit.Location = new Point(155, 140);
            Controls.Add(btnExit);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            SingletonFactory<GameController>.Instance.LeaveDungeon();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            EventInfo eventInfo = mDungeon.GetEventAtIndex(mIndex);
            if (eventInfo == null) return;

            txtEventLog.AppendText(eventInfo.EventType);
            txtEventLog.AppendText("\n");

            txtEventLog.AppendText(eventInfo.DoEvent());

            mIndex++;
        }

        private void FrmDungeon_Activated(object sender, EventArgs e)
        {
            DoRefreshUI();
        }

        private void DoRefreshUI()
        {
            mDungeon = SingletonFactory<GameController>.Instance.ExploringDungeon;
            
        }
    }
}
