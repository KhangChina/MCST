using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace MCT_SB
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCreateQuestion_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!Utility.IsFocusForm(typeof(frmAddQuestion), this))
            {
                frmAddQuestion frm = new frmAddQuestion();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnListCandidate_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!Utility.IsFocusForm(typeof(frmCandidates), this))
            {
                frmCandidates frm = new frmCandidates();
                frm.MdiParent = this;
                frm.Show();
            }

        }

        private void btnPart_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!Utility.IsFocusForm(typeof(frmPart), this))
            {
                frmPart frm = new frmPart();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnType_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!Utility.IsFocusForm(typeof(frmQuestion), this))
            {
                frmQuestion frm = new frmQuestion();
                frm.MdiParent = this;
                frm.Show();
            }
        }
    }
}