using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
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
            loadGroupType();
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
                lookTypeQuestion.ItemIndex = 0;
            }
        }
        void loadGroupType() //load danh sách group type question dựa vào Part và Type question
        {
            DataTable dt = new DataTable();
            string res = mdGroupTypeQuestion.GetByTypeAndPart(ref dt, lookTypeQuestion.EditValue.ToString(), lookPart.EditValue.ToString());
            if (res == "OK")
            {
                lookUpGroupType.Properties.DataSource = dt;
                lookUpGroupType.Properties.DisplayMember = "Name";
                lookUpGroupType.Properties.ValueMember = "ID";
                lookUpGroupType.ItemIndex = 0;
            }
            else XtraMessageBox.Show(res);
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
            if (lookTypeQuestion.EditValue != null)
                loadGroupType();
            mmDescription.Text = lookPart.Properties.GetDataSourceValue("Descriptions", lookPart.ItemIndex).ToString();
        }
        private void lookTypeQuestion_EditValueChanged(object sender, EventArgs e)
        {
            mmTypeQuestion.Text = lookTypeQuestion.Properties.GetDataSourceValue("Descriptions", lookTypeQuestion.ItemIndex).ToString();
            string CodeQuestionGroup = lookTypeQuestion.Properties.GetDataSourceValue("Code", lookTypeQuestion.ItemIndex).ToString();
            if (CodeQuestionGroup.Length > 0)
            {
                loadGroupType();
            }
        }
        private void lookUpGroupType_EditValueChanged(object sender, EventArgs e)
        {
            loadQuestion();
        }
        private void loadQuestion () //load danh sách question lên grid
        {
            DataTable dt = new DataTable();
            string res = mdQuestion.GetQuestionByGroupType(ref dt, int.Parse(lookUpGroupType.EditValue.ToString()));
            if (res == "OK")
                grcQuestion.DataSource = dt;
            else XtraMessageBox.Show(res);
        }
        private void btnAddQuestion_Click(object sender, EventArgs e)
        {
            frmAddQuestion2 frm = new frmAddQuestion2(lookUpGroupType.EditValue.ToString(), lookPart.EditValue.ToString());
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(); //hiển thị form add question
            loadQuestion();
        }
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == No)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1); //tuỳ chỉnh cột số thứ tự
            }
        }
        private void btnQuestion_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            EditorButton btn = e.Button as EditorButton;
            if(e.Button.Index == 0)
            {
                if (grcQuestions.FocusedRowHandle < 0)
                    return;
                string IDQuestion = grcQuestions.GetFocusedRowCellValue(ID).ToString();
                frmAddQuestion2 frm = new frmAddQuestion2(IDQuestion);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
                loadQuestion();
            }
            else
            {
                if (XtraMessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string IDQuestion = grcQuestions.GetFocusedRowCellValue(ID).ToString();
                    string res = mdQuestion.Delete(IDQuestion);
                    if (res != "OK")
                        XtraMessageBox.Show(res);
                    else
                    {
                        XtraMessageBox.Show("Deleted ID = " + IDQuestion.ToString());
                        loadQuestion();
                    }
                }
            }
        }
    }
}