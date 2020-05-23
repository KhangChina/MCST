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
            dtAnwer = new DataTable();
            dtAnwer.Columns.Add("Dis", typeof(string));
            dtAnwer.Columns.Add("Status", typeof(bool));
        }
        public frmAddQuestion2(int IDGroupTypeQuestion)
        {
            InitializeComponent();
            this.IDGroupTypeQuestion = IDGroupTypeQuestion;
            dtAnwer = new DataTable();
            dtAnwer.Columns.Add("Dis",typeof(string));
            dtAnwer.Columns.Add("Status",typeof(bool));

        }
        DataTable dtAnwer;
        int IDGroupTypeQuestion = -1;    
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
            dtAnwer.Rows.Add(mmDescriptionAnswer.Text,cbx.EditValue.ToString());
            gridControl1.DataSource = dtAnwer;
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if(IDGroupTypeQuestion!=-1 && picImage.Image == null)
            {
                List<string> ltQuestion = new List<string>();
                ltQuestion.Add(mmDescription.Text);
                ltQuestion.Add("");
                ltQuestion.Add("");
                ltQuestion.Add(IDGroupTypeQuestion.ToString());
                ltQuestion.Add("1");
                int IDQuestion = -1;
                string res = mdQuestion.Insert(ref IDQuestion,ltQuestion);

                //Insert Anwer

                if(res == "OK")
                {
                    List<string> ltAnswer = new List<string>();
                    ltAnswer.Add(mmDescriptionAnswer.Text);
                    ltAnswer.Add(cbx.EditValue.ToString());
                    ltAnswer.Add("");
                    ltAnswer.Add(IDQuestion.ToString());
                   // int IDAnswer = -1;
                   /// res = mdAnswer.Insert(ref );


                }


            }
            
        }
    }
}