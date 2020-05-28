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
        int IdPart = 1;
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
                    IdPart = int.Parse(dt.Rows[0]["IdPart"].ToString());
                    txtGroupTypeQuestionName.Text = dt.Rows[0]["Name"].ToString();
                    btnLocation.Text = Conifg.AudioFolder + "\\" + dt.Rows[0]["AudioName"].ToString();
                    mmDescription.Text = dt.Rows[0]["Descriptions"].ToString();
                    btnPicture.Text = Conifg.PictureFolder + "\\" + dt.Rows[0]["Images"].ToString();
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
        void clear ()
        {
            txtGroupTypeQuestionName.Text = "";
            mmDescription.Text = "";
            btnLocation.Text = "";
            btnPicture.Text = "";
        }
        void checkBtnSave ()
        {
            if (txtGroupTypeQuestionName.Text == "" || mmDescription.Text == "")
                btnAdd.Enabled = false;
            else
                btnAdd.Enabled = true;
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
        void loadPart ()
        {
            DataTable dtPart = new DataTable();
            string res = mdPart.GetAll(ref dtPart);
            if (res == "OK")
            {
                lookUpPart.Properties.DataSource = dtPart;
                lookUpPart.Properties.DisplayMember = "Name";
                lookUpPart.Properties.ValueMember = "ID";
                lookUpPart.EditValue = this.IdPart;
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        private void frmAddGroupTypeQuestion_Load(object sender, EventArgs e)
        {
            loadDataGrid();
            loadPart();
            checkBtnSave();
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
                }
            }
        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
                List<string> groupTypeQuestions = new List<string>();
                groupTypeQuestions.Add(txtGroupTypeQuestionName.Text);
                groupTypeQuestions.Add(mmDescription.Text);
                groupTypeQuestions.Add(Path.GetFileName(btnPicture.Text));
                groupTypeQuestions.Add(Path.GetFileName(btnLocation.Text));
                groupTypeQuestions.Add("1");
                groupTypeQuestions.Add(TypeID.ToString());
                groupTypeQuestions.Add(lookUpPart.EditValue.ToString());
            int IDGroupTypeQuestion = -1;
            if (this.ID == -1)
            {
                if (mdGroupTypeQuestion.CheckAudioFile(Path.GetFileName(btnLocation.Text)))
                {
                    string res = mdGroupTypeQuestion.Insert(ref IDGroupTypeQuestion, groupTypeQuestions);
                    if (res == "OK")
                    {
                        if(btnLocation.Text != "")
                            File.Copy(btnLocation.Text, Conifg.AudioFolder + "\\" + Path.GetFileName(btnLocation.Text));
                        if(btnPicture.Text != "")
                            File.Copy(btnPicture.Text, Conifg.PictureFolder + "\\" + Path.GetFileName(btnPicture.Text));
                        XtraMessageBox.Show("Compelete");
                        loadDataGrid();
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

        private void btnPicture_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            op.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            op.FileName = "";
            if (op.ShowDialog() == DialogResult.OK)
            {
                btnPicture.Text = op.FileName;

                if (File.Exists(Conifg.PictureFolder + "\\" + Path.GetFileName(btnPicture.Text)) && ID == -1)
                {
                    btnPicture.Text = "";
                    XtraMessageBox.Show("File is exits");
                }
            }
        }


        private void txtGroupTypeQuestionName_EditValueChanged(object sender, EventArgs e)
        {
            checkBtnSave();
        }

        private void mmDescription_EditValueChanged(object sender, EventArgs e)
        {
            checkBtnSave();
        }

        private void lookUpPart_EditValueChanged(object sender, EventArgs e)
        {
            string check = mdPart.getGroup(lookUpPart.EditValue.ToString());
            if (check == "1")
                btnLocation.Enabled = true;
            else if (check == "2")
                // btnLocation.ReadOnly = true;
                btnLocation.Enabled = false;
            else
                XtraMessageBox.Show(check);
        }
    }
}