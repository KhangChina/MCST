using DevExpress.XtraEditors;
using Module;
using System;
using System.Collections.Generic;
using System.Data;

namespace MCT_SB
{
    public partial class frmAddQuestion2 : DevExpress.XtraEditors.XtraForm
    {
        public frmAddQuestion2()
        {
            InitializeComponent();
        }
        public frmAddQuestion2(int ID)
        {
            InitializeComponent();
        }
        int ID = -1;
        private void frmAddQuestion2_Load(object sender, EventArgs e)
        {
            DataTable dtStatus = new DataTable();
            dtStatus.Columns.Add("ID");
            dtStatus.Columns.Add("Name");
            dtStatus.Rows.Add("1", "True");
            dtStatus.Rows.Add("0", "False");

            lookStatusAnswer.Properties.DataSource = dtStatus;
            lookStatusAnswer.Properties.DisplayMember = "Name";
            lookStatusAnswer.Properties.ValueMember = "ID";
            lookStatusAnswer.ItemIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveAnswer_Click(object sender, EventArgs e)
        {
            /*DataRow dr;
            dr["MaSV"] = "SV0001";
            dr["TenSV"] = "Nguyen Van A";
            dr["SDT"] = "123123";
            grcAnswer.Rows.Add(dr);*/
        }
    }
}