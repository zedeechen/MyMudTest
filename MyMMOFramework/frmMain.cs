using GameSample;
using System;
using System.Windows.Forms;
using ZDMMO;

namespace GameSample
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public Panel NodePanel { get { return panel1; } }


        private void Form1_Load(object sender, EventArgs e)
        {
            //SingletonFactory<UserInfo>.Instance.AddResources(enmBaseResourceType.GOLD, 100);
        }

        private void DestroySingletons()
        {
            //SingletonFactory<UserInfo>.Destroy();
        }

        private void menuQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            SingletonFactory<GameController>.Instance.CreateNewGame();

            SingletonFactory<UIManager>.Instance.AddUI<frmCreateRole>();
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            SingletonFactory<GameController>.Instance.TryLoadGame();
        }
    }
}
