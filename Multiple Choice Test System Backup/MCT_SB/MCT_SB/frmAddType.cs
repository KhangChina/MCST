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
    public partial class frmAddType : DevExpress.XtraEditors.XtraForm
    {
        public frmAddType()
        {
            InitializeComponent();     
        }
        int IDType = -1;
        public frmAddType(int ID)
        {
            InitializeComponent();
            this.IDType = ID;
            loadData(ID);
        }
        private void loadData(int ID)
        {
            DataTable dtType = new DataTable();
            string res = mdType_Question.GetByID(ref dtType,ID);
            if (res == "OK")
            {
                txtTypeName.Text = dtType.Rows[0]["Name"].ToString();
                txtTypeCode.Text = dtType.Rows[0]["Code"].ToString();
                mmTypeDes.Text = dtType.Rows[0]["Descriptions"].ToString();
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                List<string> ltType = new List<string>();
                ltType.Add(txtTypeName.Text);
                ltType.Add(mmTypeDes.Text);
                ltType.Add(txtTypeCode.Text);
                string res = "";
                if (this.IDType == -1)
                    res = mdType_Question.Insert(ref IDType, ltType);
                else
                    res = mdType_Question.Update(IDType, ltType);
                if (res == "OK")
                {
                    if (this.IDType == -1)
                    {
                        XtraMessageBox.Show("Create type is successfully");
                        Clear();
                    }
                    else
                        XtraMessageBox.Show("Update type is successfully");
                }
                else
                {
                    XtraMessageBox.Show(res);
                }
            }
            else
                MessageBox.Show("Please Enter Type Name !");
        }
        void Clear()
        {
            mmTypeDes.Text = "";
            txtTypeName.Text = "";
            txtTypeCode.Text = "";
        }
        private bool checkInput ()
        {
            if (txtTypeName.Text == "")
                return false;
            else
                return true;
        }
    }
}