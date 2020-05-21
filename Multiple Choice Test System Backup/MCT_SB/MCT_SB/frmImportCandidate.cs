using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using BUS;
using System.Collections.Generic;
using System.Linq;
using Module;

namespace MCT_SB
{
    public partial class frmImportCandidate : DevExpress.XtraEditors.XtraForm
    {
        public frmImportCandidate()
        {
            InitializeComponent();
        }
        public frmImportCandidate(int ID)
        {
            InitializeComponent();
        }
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();  //create openfileDialog Object
                openFileDialog1.Filter = "XML Files (*.xml; *.xls; *.xlsx; *.xlsm; *.xlsb) |*.xml; *.xls; *.xlsx; *.xlsm; *.xlsb";//open file format define Excel Files(.xls)|*.xls| Excel Files(.xlsx)|*.xlsx| 
                openFileDialog1.FilterIndex = 3;

                openFileDialog1.Multiselect = false;        //not allow multiline selection at the file selection level
                openFileDialog1.Title = "Open Text File-R13";   //define the name of openfileDialog
                openFileDialog1.InitialDirectory = @"Desktop"; //define the initial directory

                if (openFileDialog1.ShowDialog() == DialogResult.OK)        //executing when file open
                {
                    txtPath.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtPath.Text))
            {
                try
                {
                    DataTable tbContainer = new DataTable();
                    string strConn = string.Empty;
                    string pathName = txtPath.Text;
                    string range = txtFrom.Text + ":" + txtTo.Text; //Chỉ ra vùng chứa bảng dữ liệu (tránh lấy phần header + footer)
                    FileInfo file = new FileInfo(pathName);
                    string extension = file.Extension;
                    switch (extension) //Để chọn phiên bản excel phù hợp
                    {
                        case ".xls":
                            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                            break;
                        case ".xlsx":
                            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                            break;
                        default:
                            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                            break;
                    }
                    OleDbConnection cnnxls = new OleDbConnection(strConn); //khởi tạo connect đến file excel
                    cnnxls.Open();
                    DataTable dtSheet = cnnxls.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow drSheet in dtSheet.Rows) //duyệt từng sheet trong file
                    {
                        if (drSheet["TABLE_NAME"].ToString().Contains("$"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
                        {
                            OleDbDataAdapter oda = new OleDbDataAdapter(string.Format("select * from [{0}$" + range + "]", drSheet["TABLE_NAME"].ToString().Split('$')[0]), cnnxls);//lấy data từ sheet
                            oda.Fill(tbContainer);
                        }
                    }
                    cnnxls.Close();
                    grcImport.DataSource = tbContainer; //đổ data lên gridview
                    btnSave.Enabled = true;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                XtraMessageBox.Show("Could not find file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resetFrom(false);
            }
        }
        private void resetFrom (bool status)
        {
            btnRun.Enabled = status;
            btnSave.Enabled = status;
            txtPath.Text = "";
            txtFrom.Text = "";
            txtTo.Text = "";
            grcImport.DataSource = "";
        }
        private void checkInput ()
        {
            if (txtFrom.Text != "" && txtTo.Text != "" && txtPath.Text != "")
                btnRun.Enabled = true;
            else
                btnRun.Enabled = false;
        }

        private void txtFrom_EditValueChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void txtTo_EditValueChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void txtPath_EditValueChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            DataTable dt = grcImport.DataSource as DataTable;
            foreach (DataRow dr in dt.Rows)
            {
                List<string> list = new List<string>();
                list.Add(dr[1].ToString());
                list.Add(dr[0].ToString());
                list.Add(dr[2].ToString());
                list.Add(dr[3].ToString());
                list.Add(dr[4].ToString());
                list.Add(dr[5].ToString());
                list.Add(dr[6].ToString());
                list.Add(dr[7].ToString());
                byte[] Img = null;
                int ID = -1;
                string res = mdCandidate.Insert(ref ID, list, Img);
                if (res != "OK")
                    XtraMessageBox.Show(res);
                else
                {
                    XtraMessageBox.Show("List candidate has been saved !","Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    resetFrom(false);
                }
            }
        }
    }
}