using DevExpress.XtraEditors;
using Module;
using System;
using System.Collections.Generic;
using System.Data;

namespace MCT_SB
{
    public partial class frmAddPart : DevExpress.XtraEditors.XtraForm
    {
        public frmAddPart()
        {
            InitializeComponent();
            LoadGroup();
            btnSave.Enabled = true;
        }
        public frmAddPart(int ID)
        {
            InitializeComponent();
            LoadGroup();
            btnUpdate.Enabled = true;
            this.ID = ID;
            DataTable dt = new DataTable();
            string res = mdPart.GetByID(ref dt, ID.ToString());
            if (res == "OK")
            {
                txtPart.Text = dt.Rows[0]["Name"].ToString();
                lookGroup.EditValue = dt.Rows[0]["IdGroup"].ToString();
                mm.Text = dt.Rows[0]["Descriptions"].ToString();
                ckStatus.EditValue = bool.Parse(dt.Rows[0]["Status"].ToString());
            }
        }
        int ID = -1;
        private void frmAddPart_Load(object sender, EventArgs e)
        {
           
        }
        void LoadGroup()
        {
            DataTable dtGroup = new DataTable();
            dtGroup.Columns.Add("ID");
            dtGroup.Columns.Add("Name");
            string res = mdGroup.GetAll(ref dtGroup);
            if (res == "OK")
            {
                lookGroup.Properties.DataSource = dtGroup;
                lookGroup.Properties.DisplayMember = "Name";
                lookGroup.Properties.ValueMember = "ID";
                lookGroup.ItemIndex = 0;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> ltPart = new List<string>();
            ltPart.Add(txtPart.Text);
            ltPart.Add(lookGroup.EditValue.ToString());
            ltPart.Add(mm.Text);
            ltPart.Add(ckStatus.EditValue.ToString());
            int IDPart = -1;
            string res = mdPart.Insert(ref IDPart, ltPart);
            if (res == "OK")
            {
                XtraMessageBox.Show("Compelete");
                Clear();
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        void Clear()
        {
            mm.Text = "";
            txtPart.Text = "";
        }
        private void btnUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (ID != -1)
            {
                //Get in control
                List<string> ltPart = new List<string>();
                ltPart.Add(txtPart.Text);
                ltPart.Add(lookGroup.EditValue.ToString());
                ltPart.Add(mm.Text);
                ltPart.Add(ckStatus.EditValue.ToString());
                string res = mdPart.Update(ID, ltPart);
                if (res == "OK")
                {
                    this.Close();
                }




            }
        }
    }
}