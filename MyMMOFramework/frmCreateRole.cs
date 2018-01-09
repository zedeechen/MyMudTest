using GameSample.Common.UserControls;
using GameSample.Heroes;
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
    public partial class frmCreateRole : Form
    {
        public frmCreateRole()
        {
            InitializeComponent();
        }

        private int mStep = 0;
        
        private bool mChangedChoice = true;

        private int mRemainPoints = 28;
        private Dictionary<enmPropType, int> mInitProps = new Dictionary<enmPropType, int>();

        private List<UIAddMinusControl> mControls;

        private HeroInfo mCreatingHeroInfo = null;

        private void frmCreateRole_Load(object sender, EventArgs e)
        {
            this.FormClosing += FrmCreateRole_FormClosing;  

            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
            {
                mInitProps[type] = 8;
            }
            mCreatingHeroInfo = new HeroInfo();

            {
                panelStep1.SetBounds(0, 0, this.Width, this.Height);
                Button btn;
                ClassConfig config;
                byte key;
                
                for (int i = 0, count = SingletonFactory<ClassConfig>.Instance.GetMaxId(); i < count; i++)
                {
                    key = (byte)(i + 1);
                    config = SingletonFactory<ClassConfig>.Instance.GetDataById(key);
                    btn = new Button();
                    btn.Text = config.name;
                    btn.Click += ChosenClass;
                    btn.Tag = key;
                    btn.SetBounds(20 + 150 * (i % 3), 20 + 100 * (i / 3), 100, 50);
                    panelStep1.Controls.Add(btn);
                }

                
            }

            {
                panelStep2.SetBounds(0, 0, this.Width, this.Height);
                Button btn;
                RaceConfig config;
                byte key;
                for (int i = 0, count = SingletonFactory<RaceConfig>.Instance.GetMaxId(); i < count; i++)
                {
                    key = (byte)(i + 1);
                    config = SingletonFactory<RaceConfig>.Instance.GetDataById(key);
                    btn = new Button();
                    btn.Text = config.name;
                    btn.Click += ChosenRace;
                    btn.Tag = key;
                    btn.SetBounds(20 + 150 * (i % 3), 20 + 100 * (i / 3), 100, 50);
                    panelStep2.Controls.Add(btn);
                }
            }

            {
                panelStep3.SetBounds(0, 0, this.Width, this.Height);

                UIAddMinusControl control;
                mControls = new List<UIAddMinusControl>();
                for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
                {
                    control = new UIAddMinusControl(type.ToString(), btnReduce_Click, btnIncrease_Click, (int)type);
                    control.Location = new Point(30, 40 + (type - enmPropType.BP_MIN) * 30);
                    panelStep3.Controls.Add(control);

                    mControls.Add(control);
                }
            }

            SwitchToNextStep();
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            enmPropType type = (enmPropType)((Button)sender).Tag;
            BuyProp(type);

            mControls[type - enmPropType.BP_MIN - 1].SetLabelValue(mCreatingHeroInfo.GetBaseProp(type));
            mControls[type - enmPropType.BP_MIN - 1].SetLabelModValue(mCreatingHeroInfo.GetBasePropMod(type));
        }
        private void btnReduce_Click(object sender, EventArgs e)
        {
            enmPropType type = (enmPropType)((Button)sender).Tag;
            RefundProp(type);

            mControls[type - enmPropType.BP_MIN - 1].SetLabelValue(mCreatingHeroInfo.GetBaseProp(type));
            mControls[type - enmPropType.BP_MIN - 1].SetLabelModValue(mCreatingHeroInfo.GetBasePropMod(type));
        }

        private void FrmCreateRole_FormClosing(object sender, FormClosingEventArgs e)
        {
            mInitProps.Clear();
            mCreatingHeroInfo = null;
        }

        private void InitUI()
        {
            switch (mStep)
            {
                case 1:
                    {
                        panelStep1.Show();
                        panelStep2.Hide();
                        panelStep3.Hide();
                        
                        break;
                    }
                case 2:
                    {
                        panelStep1.Hide();
                        panelStep2.Show();
                        panelStep3.Hide();
                        
                        break;
                    }
                case 3:
                    {
                        ResetProp();

                        panelStep1.Hide();
                        panelStep2.Hide();
                        panelStep3.Show();

                        lbClass.Text = mCreatingHeroInfo.GetClasses();
                        lbRace.Text = mCreatingHeroInfo.GetRace();

                        lbRemainPoints.Text = mRemainPoints.ToString();
                        
                        UIAddMinusControl control;
                        for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
                        {
                            control = mControls[type - enmPropType.BP_MIN - 1];
                            control.SetLabelValue(mCreatingHeroInfo.GetBaseProp(type));
                            control.SetLabelModValue(mCreatingHeroInfo.GetBasePropMod(type));
                        }

                        
                        break;
                    }
            }            
        }

        private void ResetProp()
        {
            if (mChangedChoice)
            {
                mChangedChoice = false;

                mRemainPoints = 28;
                
                mCreatingHeroInfo.InitBasePoints(mInitProps);
            }
        }

        private void SwitchToNextStep()
        {
            mStep++;
            InitUI();
        }

        private void ChosenClass(object sender, EventArgs e)
        {
            byte key = (byte)((Button)sender).Tag;

            mCreatingHeroInfo.ResetAllClassLevel();
            mCreatingHeroInfo.SetClassLevel(key, 1);

            SwitchToNextStep();
        }
        private void ChosenRace(object sender, EventArgs e)
        {
            byte key = (byte)((Button)sender).Tag;

            mCreatingHeroInfo.SetRace(key);
            SwitchToNextStep();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (mRemainPoints == 0)
            {
                SingletonFactory<GameController>.Instance.AddHero(mCreatingHeroInfo);
                SingletonFactory<GameController>.Instance.FinishedCreatingRole();
                SingletonFactory<GameController>.Instance.EnterHome();
            }
        }

        private void RefundProp(enmPropType type)
        {
            int point;
            if ((point = mCreatingHeroInfo.GetInitBP(type)) <= 8)
                return;

            byte returnBP = GameLogic.GetReqBuyPoints(point - 1);
            if (returnBP < byte.MaxValue)
            {
                mCreatingHeroInfo.ReduceInitBp(type);
                mRemainPoints += returnBP;

                lbRemainPoints.Text = mRemainPoints.ToString();
            }
        }

        private void BuyProp(enmPropType type)
        {
            int point;
            if ((point = mCreatingHeroInfo.GetInitBP(type)) >= 18)
                return;

            byte reqBP = GameLogic.GetReqBuyPoints(point);
            if (mRemainPoints >= reqBP)
            {
                mCreatingHeroInfo.AddInitBp(type);
                mRemainPoints -= reqBP;

                lbRemainPoints.Text = mRemainPoints.ToString();
            }
        }
    }
}
