using System.Collections.Generic;
using System.Windows.Forms;
using ZDMMO;

namespace GameSample
{
    public class UIManager
    {
        public static void AddSubForm<T>(Form parent, Panel parentContainer) where T :Form, new()
        {
            T frm = new T();
            frm.TopLevel = false;
            parentContainer.Controls.Add(frm);
            frm.Size = parentContainer.Size; 
            frm.Show();
        }

        private List<Form> mOpeningForms = new List<Form>();

        public void AddUI<T>() where T :Form , new()
        {
            T frm = SingletonFactory<T>.Instance;
            if (mOpeningForms.Contains(frm))
            {
                frm.BringToFront();
                return;
            }

            mOpeningForms.Add(frm);
            frm.TopLevel = false;
            SingletonFactory<frmMain>.Instance.NodePanel.Controls.Add(frm);
            frm.Size = SingletonFactory<frmMain>.Instance.NodePanel.Size;
            frm.Show();
            frm.BringToFront();
        }

        public void CloseUI<T>() where T : Form, new()
        {
            T frm = SingletonFactory<T>.Instance;
            if (mOpeningForms.Contains(frm))
            {
                mOpeningForms.Remove(frm);
                frm.Close();
                SingletonFactory<T>.Destroy();
            }
            else
            {
                
            }
            
        }
    }
}
