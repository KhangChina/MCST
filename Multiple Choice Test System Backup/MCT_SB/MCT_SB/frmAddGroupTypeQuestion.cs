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
using Module;

namespace MCT_SB
{
    public partial class frmAddGroupTypeQuestion : DevExpress.XtraEditors.XtraForm
    {
        public frmAddGroupTypeQuestion()
        {
            InitializeComponent();
           
        }
        int ID = -1;
        int TypeID = -1;
        public frmAddGroupTypeQuestion(int ID,int TypeID)
        {
            InitializeComponent();
            this.ID = ID;
            this.TypeID = TypeID;
            if (ID != -1)
            {
                DataTable dt = new DataTable();
                string res = mdGroupTypeQuestion.GetByID(ref dt, ID.ToString());
                if (res == "OK")
                {
                    txtAudioName.Text = dt.Rows[0]["Name"].ToString();
                    btnLocation.Text = Conifg.AudioFolder + "\\" + dt.Rows[0]["AudioName"].ToString();
                    mmDescription.Text = dt.Rows[0]["Descriptions"].ToString();
                }
                else
                {
                    XtraMessageBox.Show(res);
                }    
            }
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
        void clear ()
        {
            txtAudioName.Text = "";
            mmDescription.Text = "";
            btnLocation.Text = "";
        }
        DataTable ProcessDirectory(string path) //lấy danh sách file trong thư mục
        {
            DataTable table = new DataTable();

            DataColumn col = new DataColumn("No.", typeof(int)); //Tạo cột stt
            col.AllowDBNull = false;
            col.AutoIncrement = true;
            col.AutoIncrementSeed = 1;
            col.Unique = true;

            table.Columns.Add(col);
            table.Columns.Add(new DataColumn("File Name", typeof(string)));

            string[] fileList = Directory.GetFiles(path);//lay danh sách file cho vao mảng
            //duyet mang file trong thư mục
            foreach (string fileName in fileList)
            {
                table.Rows.Add(null,Path.GetFileName(fileName).Trim());
            }
            return table;
        }

        void loadDataGrid()
        {
            grcGroupTypeQuestion.DataSource = ProcessDirectory(Conifg.AudioFolder);
        }

        private void frmAddGroupTypeQuestion_Load(object sender, EventArgs e)
        {
            if (this.ID != -1)
                ResetForm(true);
            else
                ResetForm(false);
            loadDataGrid();
        }

        private void btnLocation_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            op.Filter = "mp3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            op.FileName = "";
            if (op.ShowDialog() == DialogResult.OK)
            {
                btnLocation.Text = op.FileName;

                if (File.Exists(Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text)) && ID == -1)
                {
                    btnLocation.Text = "";
                    XtraMessageBox.Show("File is exits");
                    ResetForm(false);
                }
                else
                {
                    ResetForm(true);
                }
            }
        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
                List<string> groupTypeQuestions = new List<string>();
                groupTypeQuestions.Add(txtAudioName.Text);
                groupTypeQuestions.Add(mmDescription.Text);
                groupTypeQuestions.Add(Path.GetFileName(btnLocation.Text));
                groupTypeQuestions.Add("1");
                groupTypeQuestions.Add(TypeID.ToString());
                int IDGroupTypeQuestion = -1;
            if (this.ID == -1)
            {
                if (mdGroupTypeQuestion.CheckAudioFile(Path.GetFileName(btnLocation.Text)))
                {
                    string res = mdGroupTypeQuestion.Insert(ref IDGroupTypeQuestion, groupTypeQuestions);
                    if (res == "OK")
                    {
                        File.Copy(btnLocation.Text, Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text));
                        XtraMessageBox.Show("Compelete");
                        loadDataGrid();
                        ResetForm(false);
                        clear();
                    }
                    else
                    {
                        XtraMessageBox.Show(res);
                    }
                }
                else
                {
                    XtraMessageBox.Show("File is exist");
                }
            }
            else
            {
                string res = mdGroupTypeQuestion.Update(ID, groupTypeQuestions);
                if (res == "OK")
                {
                    if (Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text) != btnLocation.Text)//kiểm tra file có thay đổi ko
                        File.Copy(btnLocation.Text, Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text));
                    XtraMessageBox.Show("Compelete");
                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show(res);
                }
            } 
        }
    }
}