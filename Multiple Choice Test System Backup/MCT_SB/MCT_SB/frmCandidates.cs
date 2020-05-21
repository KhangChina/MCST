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
using Module;

namespace MCT_SB
{
    public partial class frmCandidates : DevExpress.XtraEditors.XtraForm
    {
        public frmCandidates()
        {
            InitializeComponent();     
        }
        DataTable dtCandidate;
        void LoadCandidate()
        {
            dtCandidate = new DataTable();
            string res = mdCandidate.GetAll(ref dtCandidate);
            if (res == "OK")
                grcCadidate.DataSource = dtCandidate;
            else XtraMessageBox.Show(res);
        }
        private void frmCandidates_Load(object sender, EventArgs e)
        {
            LoadCandidate();
        }
        private void grcCadidate_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {           
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Append)
            {
                e.Handled = true;
                frmAddCadidate frm = new frmAddCadidate();
                frm.ShowDialog();
                LoadCandidate();
            }
            if(e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                if (XtraMessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    e.Handled = true;
                    int IDCandidates = int.Parse(grvCandidate.GetFocusedRowCellValue(colID).ToString());
                    string res = mdCandidate.Delete(IDCandidates);
                    if(res != "OK")
                    {
                        XtraMessageBox.Show(res);
                    }
                    else
                    {
                        XtraMessageBox.Show("Deleted ID = " + IDCandidates.ToString());
                        LoadCandidate();
                    }
                }
                LoadCandidate();
            }
        }     
        private void grcCadidate_DoubleClick(object sender, EventArgs e)
        {
            if (grvCandidate.FocusedRowHandle < 0)
                return;
            int IDCandidates = int.Parse(grvCandidate.GetFocusedRowCellValue(colID).ToString());
            frmAddCadidate frm = new frmAddCadidate(IDCandidates);
            frm.ShowDialog();
            LoadCandidate();
        }
    }
}