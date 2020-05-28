using DevExpress.XtraEditors;
using Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MCT_SB
{
    public partial class frmAddQuestion2 : DevExpress.XtraEditors.XtraForm
    {
        public frmAddQuestion2()
        {
            InitializeComponent();
            dtAnwer = new DataTable();
            dtAnwer.Columns.Add("Descriptions", typeof(string));
            dtAnwer.Columns.Add("Status", typeof(bool));
        }
        DataTable dtAnwer;
        string IDGroupTypeQuestion = "-1";
        string IDPart = "-1";
        string IDQuestion = "-1";
        List<string> lstDelAnswer = new List<string>(); //lưu tạm những answer chuẩn bị xoá
        public frmAddQuestion2(string IDGroupTypeQuestion, string IDPart) //form add
        {
            InitializeComponent();
            this.IDGroupTypeQuestion = IDGroupTypeQuestion;
            this.IDPart = IDPart;
            dtAnwer = new DataTable();
            dtAnwer.Columns.Add("Descriptions",typeof(string));
            dtAnwer.Columns.Add("Status",typeof(bool));
        }
        public frmAddQuestion2(string IDQuestion) //form update
        {
            InitializeComponent();
            this.IDQuestion = IDQuestion;
            DataTable dt = new DataTable(); //load data question
            string res = mdQuestion.GetQuestionByID(ref dt, IDQuestion);
            if (res == "OK")
            {
                IDPart = dt.Rows[0]["IdPart"].ToString();
                IDGroupTypeQuestion = dt.Rows[0]["IdGroupTypeQuestions"].ToString();
                mmDescription.Text = dt.Rows[0]["Descriptions"].ToString();
                var img = (Byte[])(dt.Rows[0]["Images"]);
                try
                {
                    picImage.Image = Utility.byteArrayToImage(img);
                }
                catch
                { }
            }
            else
            {
                XtraMessageBox.Show(res);
            }
            dtAnwer = new DataTable();
            dtAnwer.Columns.Add("ID", typeof(int));
            dtAnwer.Columns.Add("Descriptions", typeof(string));
            dtAnwer.Columns.Add("Status", typeof(bool)); //Khởi tạo datatable answer
            string ress = mdAnswer.GetAnswerByQuestion(ref dtAnwer, IDQuestion); //load data answer
            if (res == "OK")
            {
                gridControl1.DataSource = dtAnwer;
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        private void btnSaveAnswer_Click_1(object sender, EventArgs e)
        {
            if (cbx.EditValue.ToString() == true.ToString())
            {
                foreach (DataRow item in dtAnwer.Rows)
                {
                    if (item["Status"].ToString() == true.ToString())
                    {
                        XtraMessageBox.Show("This question has a true answer!");
                        return;
                    }
                }
            }
            if(IDQuestion != "-1") // khác -1 là đang update, sử dụng datatable có trường ID
                dtAnwer.Rows.Add(-1,mmDescriptionAnswer.Text,cbx.EditValue.ToString());
            else
                dtAnwer.Rows.Add(mmDescriptionAnswer.Text, cbx.EditValue.ToString());
            gridControl1.DataSource = dtAnwer;
        }
        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (Utility.checkStatus(dtAnwer))
            {
                if (IDQuestion == "-1")
                    insert_question();
                else
                    update_question();
            }
            else
            {
                XtraMessageBox.Show("This question hasn't TRUE answer");
            }
        }
        private void insert_question ()
        {
            if (IDGroupTypeQuestion != "-1")//thêm mới câu hỏi
            {
                List<string> ltQuestion = new List<string>();
                ltQuestion.Add(mmDescription.Text);
                ltQuestion.Add(IDPart);
                ltQuestion.Add(IDGroupTypeQuestion);
                ltQuestion.Add("1");
                int IDQuestion = -1;
                string res = "";
                if (picImage.Image == null)
                    res = mdQuestion.Insert(ref IDQuestion, ltQuestion);
                else
                    res = mdQuestion.Insert(ref IDQuestion, ltQuestion, Utility.imageToByteArray(picImage.Image));
                //Insert Anwer

                if (res == "OK")
                {
                    foreach (DataRow row in dtAnwer.Rows)
                    {
                        List<string> ltAnswer = new List<string>();
                        ltAnswer.Add(row["Descriptions"].ToString());
                        ltAnswer.Add(row["Status"].ToString());
                        ltAnswer.Add(IDQuestion.ToString());
                        int IDAnswer = -1;
                        string ress = mdAnswer.Insert(ref IDAnswer, ltAnswer);
                        if (ress != "OK")
                        {
                            XtraMessageBox.Show(ress);
                            return;
                        }
                    }
                    XtraMessageBox.Show("Success");
                }
                else
                {
                    XtraMessageBox.Show(res);
                }

            }
            else
            {
                XtraMessageBox.Show("Fail");
            }
        }
        private void update_question ()
        {
            string res = "";
            if (picImage.Image == null)
                res = mdQuestion.Update(int.Parse(IDQuestion), mmDescription.Text);
            else
                res = mdQuestion.Update(int.Parse(IDQuestion), mmDescription.Text, Utility.imageToByteArray(picImage.Image));
                //Update Anwer
            if (res == "OK")
                {
                    update_answer();
                    delete_answer();
                }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        private void update_answer()
        {
            foreach (DataRow row in dtAnwer.Rows)
            {
                List<string> ltAnswer = new List<string>();
                ltAnswer.Add(row["Descriptions"].ToString());
                ltAnswer.Add(row["Status"].ToString());
                ltAnswer.Add(IDQuestion);
                string ress = "";
                int ID = -1;
                if (mdAnswer.CheckExitAnser(row["ID"].ToString())) //Kiểm tra nếu đã có thì update
                    ress = mdAnswer.Update(row["ID"].ToString(), ltAnswer);
                else
                    ress = mdAnswer.Insert(ref ID, ltAnswer); //insert nếu là đáp án mới
                if (ress != "OK")
                {
                    XtraMessageBox.Show(ress);
                    return;
                }
            }
            XtraMessageBox.Show("Success");
        }
        private void delete_answer()
        {
            string res = "";
            foreach(string item in lstDelAnswer)
            {
                res = mdAnswer.Delete(item);
                if(res != "OK")
                {
                    XtraMessageBox.Show(res);
                    return;
                }
            }    
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnAction_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string ID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ID"]).ToString();
            if (IDQuestion != "-1")
                lstDelAnswer.Add(ID); //thêm id answer cần xoá vào list delete
            foreach (DataRow dr in dtAnwer.Rows) //xoá trên UI
            {
                if (dr["ID"].ToString() == ID)
                {
                    dtAnwer.Rows.Remove(dr);
                    break;
                }
            }
            gridControl1.DataSource = dtAnwer;
        }
        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit ckc = sender as CheckEdit;
            string ID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ID"]).ToString();
            if (ckc.Checked == true)
            {
                foreach(DataRow dt in dtAnwer.Rows)
                {
                    if (dt["ID"].ToString() != ID)
                        dt["Status"] = false;
                }
                gridControl1.DataSource = dtAnwer;
            }
        }
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string ID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ID"]).ToString();
            string desc = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Descriptions"]).ToString();
            foreach (DataRow dr in dtAnwer.Rows) //xoá trên UI
            {
                if (dr["ID"].ToString() == ID)
                {
                    dr["Descriptions"] = desc;
                    break;
                }
            }
        }
    }
}