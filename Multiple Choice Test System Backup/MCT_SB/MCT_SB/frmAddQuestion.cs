﻿using DevExpress.XtraEditors;
using Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MCT_SB
{
    public partial class frmAddQuestion : DevExpress.XtraEditors.XtraForm
    {
        public frmAddQuestion()
        {
            InitializeComponent();
        }

        private void frmQuestion_Load(object sender, EventArgs e)
        {
            LoadGroup();
            LoadType_Question();
            ResetTab(false);
            LoadGrid();
        }
        void LoadGroup()
        {
            DataTable dtGroup = new DataTable();
            string res = mdGroup.GetAll(ref dtGroup);
            if (res == "OK")
            {
                lookGroup.Properties.DataSource = dtGroup;
                lookGroup.Properties.DisplayMember = "Name";
                lookGroup.Properties.ValueMember = "ID";
                lookGroup.ItemIndex = 0;
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        void LoadType_Question()
        {
            DataTable dtType_Question = new DataTable();
            string res = mdType_Question.GetAll(ref dtType_Question);
            if (res == "OK")
            {
                lookTypeQuestion.Properties.DataSource = dtType_Question;
                lookTypeQuestion.Properties.DisplayMember = "Name";
                lookTypeQuestion.Properties.ValueMember = "ID";
                //lookTypeQuestion.ItemIndex = 0;
            }
        }
        void LoadGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Image", typeof(Image));
            dt.Columns.Add("Answer");
            dt.Columns.Add("Discription");
            dt.Columns.Add("Status", typeof(bool));

            DataTable dtAnwer = new DataTable();
            dtAnwer.Columns.Add("Status");
            dtAnwer.Columns.Add("Answer");
            dtAnwer.Rows.Add("A", "A");
            dtAnwer.Rows.Add("B", "B");
            dtAnwer.Rows.Add("C", "C");
            dtAnwer.Rows.Add("D", "D");

            lookAnswer.DataSource = dtAnwer;
            lookAnswer.DisplayMember = "Answer";
            lookAnswer.ValueMember = "Status";

            colAnwer.ColumnEdit = lookAnswer;
            grc.DataSource = dt;

            txtDescriptions.Text = "";
            AudioName.Text = "";
            txtNameGroupTypeQuestion.Text = "";
        }
        private void lookGroup_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtPart = new DataTable();
            string res = mdPart.GetAll(ref dtPart, lookGroup.EditValue.ToString());
            if (res == "OK")
            {
                lookPart.Properties.DataSource = dtPart;
                lookPart.Properties.DisplayMember = "Name";
                lookPart.Properties.ValueMember = "ID";
                lookPart.ItemIndex = 0;
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }

        private void lookPart_EditValueChanged(object sender, EventArgs e)
        {
            mmDescription.Text = lookPart.Properties.GetDataSourceValue("Descriptions", lookPart.ItemIndex).ToString();
        }

        private void lookTypeQuestion_EditValueChanged(object sender, EventArgs e)
        {
            mmTypeQuestion.Text = lookTypeQuestion.Properties.GetDataSourceValue("Descriptions", lookTypeQuestion.ItemIndex).ToString();
            string CodeQuestionGroup = lookTypeQuestion.Properties.GetDataSourceValue("Code", lookTypeQuestion.ItemIndex).ToString();
            if (CodeQuestionGroup.Length > 0)
            {
                GetTabControl(CodeQuestionGroup, true);
            }
        }
        void ResetTab(bool Status)
        {

            for (int i = 0; i < TabQuestion.TabPages.Count; i++)
            {

                TabQuestion.TabPages[i].PageVisible = Status;
            }
        }
        void GetTabControl(string CodeQuestionGroup, bool Status)
        {
            for (int i = 0; i < TabQuestion.TabPages.Count; i++)
            {
                if (TabQuestion.TabPages[i].Tag != null)
                {
                    if (TabQuestion.TabPages[i].Tag.ToString() == CodeQuestionGroup)
                    {
                        ResetTab(false);
                        TabQuestion.TabPages[i].PageVisible = Status;
                        TabQuestion.TabPages[i].Show();
                    }
                }

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            InsertGroupTypeQuestion();

        }
        void InsertGroupTypeQuestion()
        {
            List<string> ltGroupTypeQuestion = new List<string>();
            ltGroupTypeQuestion.Add(txtNameGroupTypeQuestion.Text);
            ltGroupTypeQuestion.Add(txtDescriptions.Text);
            ltGroupTypeQuestion.Add(Path.GetFileName(AudioName.Text));
            ltGroupTypeQuestion.Add(cbxStatus.EditValue.ToString());
            ltGroupTypeQuestion.Add(lookTypeQuestion.EditValue.ToString());
            int IDGroupTypeQuestion = -1;
            string res = mdGroupTypeQuestion.Insert(ref IDGroupTypeQuestion, ltGroupTypeQuestion);
            if (res != "OK")
            {
                XtraMessageBox.Show(res);
            }
            else
            {
                InsertQuestion(IDGroupTypeQuestion);
            }

        }
        void InsertQuestion(int IDGroupTypeQuestion)
        {
            if (IDGroupTypeQuestion != -1)
            {
                int dem = grv.RowCount;
                if (dem > 0)
                {
                    for (int i = 0; i < dem; i++)
                    {
                        List<string> ltQuestionImage = new List<string>();
                        ltQuestionImage.Add(grv.GetRowCellValue(i, colDiscription).ToString());
                        ltQuestionImage.Add("");
                        ltQuestionImage.Add("");
                        ltQuestionImage.Add(lookPart.EditValue.ToString());
                        ltQuestionImage.Add(IDGroupTypeQuestion.ToString());
                        ltQuestionImage.Add(grv.GetRowCellValue(i, colStatus).ToString());
                        byte[] image = Utility.imageToByteArray((Image)grv.GetRowCellValue(i, colImage));
                        int IDQuestion = -1;
                        string res = mdQuestion.Insert(ref IDQuestion, ltQuestionImage, image);
                        if (res == "OK")
                        {
                             InsertAnswer(IDQuestion, grv.GetRowCellValue(i, colAnwer).ToString(),"1");
                        }
                        else
                        {
                            XtraMessageBox.Show(res);
                        }

                    }
                }
            }
        }
        void InsertAnswer(int IDQuestion,string Answer,string Status)
        {
            if (IDQuestion != -1)
            {
                List<string> ltAnswer = new List<string>();
                ltAnswer.Add(Answer);
                ltAnswer.Add(Status);
                ltAnswer.Add("");
                ltAnswer.Add(IDQuestion.ToString());
                int IDAnswer = -1;
                string res = mdAnswer.Insert(ref IDAnswer, ltAnswer);
                if(res == "OK")
                {
                    XtraMessageBox.Show("Insert Complete");
                }
                else
                {
                    XtraMessageBox.Show(res);
                }
            }
        }

        private void AudioName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            op.Filter = "Mp3 Files (*.Mp3)|*.Mp3|All Files (*.*)|*.*";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AudioName.Text = op.FileName;
                if (File.Exists(Application.StartupPath + "\\Audio\\" + Path.GetFileName(AudioName.Text)))
                {
                    XtraMessageBox.Show("File Exists");
                }
                else
                {
                    File.Copy(AudioName.Text, Application.StartupPath + "\\Audio\\" + Path.GetFileName(AudioName.Text));
                }

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            LoadGrid();
            
        }
    }
}