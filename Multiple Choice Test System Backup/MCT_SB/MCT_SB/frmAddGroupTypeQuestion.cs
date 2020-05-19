using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace MCT_SB
{
    public partial class frmAddGroupTypeQuestion : DevExpress.XtraEditors.XtraForm
    {
        public frmAddGroupTypeQuestion()
        {
            InitializeComponent();
           
        }
        
        private void txtLocation_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
           
        }
        void ResetForm(bool status)
        {
            txtAudioName.Enabled = status;          
            btnLocation.ReadOnly = true;
            mmDescription.Enabled = status;
            btnAdd.Enabled = status;
        }

        private void frmAddGroupTypeQuestion_Load(object sender, EventArgs e)
        {
            ResetForm(false);
        }

        private void btnLocation_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            op.Filter = "mp3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            op.FileName = "";
            if (op.ShowDialog() == DialogResult.OK)
            {
                btnLocation.Text = op.FileName;

                if (File.Exists(Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text)))
                {
                    btnLocation.Text = "";
                    XtraMessageBox.Show("File is exits");
                    ResetForm(false);
                }
                else
                {
                    File.Copy(btnLocation.Text, Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text));
                    ResetForm(true);
                }
            }
        }
    }
}