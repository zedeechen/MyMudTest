using GameSample.Heroes;
using System.Windows.Forms;

namespace GameSample.Common.UserControls
{
    public class UIHeroSummary : UserControl
    {
        private Label lbHp;
        private Label lbRace;
        private Label lbLevel;
        private Label lbAB;
        private Label lbAC;

        private HeroInfo mInfo;

        public UIHeroSummary(HeroInfo info)
        {
            InitializeComponent();

            mInfo = info;
            RefreshUI();
        }
        private void InitializeComponent()
        {
            lbHp = new Label();
            lbHp.Location = new System.Drawing.Point(0, 0);
            lbHp.Size = new System.Drawing.Size(50, 20);

            lbRace = new Label();
            lbRace.Location = new System.Drawing.Point(0, 25);
            lbRace.Size = new System.Drawing.Size(50, 20);

            lbLevel = new Label();
            lbLevel.Location = new System.Drawing.Point(55, 25);
            lbLevel.Size = new System.Drawing.Size(150, 20);

            lbAB = new Label();
            lbAB.Location = new System.Drawing.Point(0, 50);
            lbAB.Size = new System.Drawing.Size(50, 20);

            lbAC = new Label();
            lbAC.Location = new System.Drawing.Point(55, 50);
            lbAC.Size = new System.Drawing.Size(50, 20);

            this.Size = new System.Drawing.Size(150, 100);

            this.Controls.Add(lbHp);
            this.Controls.Add(lbRace);
            this.Controls.Add(lbLevel);
            this.Controls.Add(lbAB);
            this.Controls.Add(lbAC);
        }

        private void InitBindings()
        {

        }

        public void RefreshUI()
        {
            lbHp.Text = string.Format("HP:{0}",mInfo.GetAdvProp(enmPropType.HP));
            lbRace.Text = mInfo.GetRace();
            lbLevel.Text = mInfo.GetClasses();
            lbAB.Text = string.Format("AB:{0}",mInfo.GetAdvProp(enmPropType.ATTACK_BONUS));
            lbAC.Text = string.Format("AC:{0}",mInfo.GetAdvProp(enmPropType.ARMOR_CLASS));
        }
    }
}
