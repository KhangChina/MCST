using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Module;

namespace MCT_SB
{
    public partial class frmImportExams : DevExpress.XtraEditors.XtraForm
    {
        public frmImportExams()
        {
            InitializeComponent();
        }
        DataTable dtAnswerFull;
        DataTable dtAnswerGroup;
        private void btnOpen_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            op.Filter = "File word | *.docx";
            if (op.ShowDialog() == DialogResult.OK)
            {
                btnOpen.Text = op.FileName;
            }
        }
        void ReadAnswer(string path)
        {
            dtAnswerFull = new DataTable();
            dtAnswerFull.Columns.Add("Number");
            dtAnswerFull.Columns.Add("Answer");

            string line = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] arrListStr = line.Split('.');
                    dtAnswerFull.Rows.Add(arrListStr[0], arrListStr[1]);
                }
            }
        }
        private void btnDetact_Click(object sender, EventArgs e)
        {
            string pathEx = Application.StartupPath + "\\Data\\Ex1\\Image";
            string text = "";
            DetactImage(pathEx, ref text);

            string html = Utility.TextToHtml(text);

            memoEdit1.Text = text;
            DetactFullListen(html);
            DetactListenGroup(html);

        }
        void DetactImage(string Outpath, ref string text)
        {
            List<string> ImageName = new List<string>();
            text = Utility.Detect(btnOpen.Text, Outpath, ref ImageName);
            //text = Utility.DetectPDF(btnOpen.Text, Outpath, ref ImageName);
            DataTable dtImage = new DataTable();
            dtImage.Columns.Add("ImageName");
            dtImage.Columns.Add("Answer");
            foreach (string item in ImageName)
            {
                foreach (DataRow rows in dtAnswerFull.Rows)
                {
                    if (rows["Number"].ToString() + ".png" == Path.GetFileName(item))
                    {
                        dtImage.Rows.Add(item, rows["Answer"].ToString());
                    }
                }
            }
            grcImage.DataSource = dtImage;
        }
        private void btnAnswer_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            op.Filter = "File Answer | *.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                btnAnswer.Text = op.FileName;
                ReadAnswer(btnAnswer.Text);
            }

        }
        void DetactFullListen(string s)
        {
            string pattern = @"(?<number>\d+).\s&nbsp;(?<values>\S.*)<br>";
            MatchCollection matchCollection = Regex.Matches(s, pattern);
            string mark = "Mark your answer on your answer sheet.";
            DataTable dt = new DataTable();
            dt.Columns.Add("AudioName");
            dt.Columns.Add("Answer");
            dt.Columns.Add("Discription");
            foreach (Match match in matchCollection)
            {
                //MessageBox.Show(match.Groups["number"].Value+" => "+ match.Groups["values"].Value);
                string number = match.Groups["number"].Value;
                string values = match.Groups["values"].Value;

                if (values == mark)
                {
                    foreach (DataRow item in dtAnswerFull.Rows)
                    {
                        if (item["Number"].ToString() == number)
                        {
                            dt.Rows.Add("", number + " . " + values, item["Answer"].ToString());
                        }
                    }

                }
            }
            grcListenFull.DataSource = dt;
        }
        void DetactListenGroup(string s)
        {
            //string pattern = @"(?<number>\d+).\s&nbsp;(?<caption>\S.*)<br>
            //                    \((?<a>\w)\).&nbsp;(?<va>\S.*)<br>
            //                    \((?<b>\w)\).&nbsp;(?<vb>\S.*)<br>
            //                    \((?<c>\w)\).&nbsp;(?<vc>\S.*)<br>
            //                    \((?<d>\w)\).&nbsp;(?<vd>\S.*)<br>";
            string pattern = @"(?<number>\d+).\s&nbsp;(?<caption>\S.*)<br>\s*\((?<a>\w)\).&nbsp;(?<va>\S.*)<br>\s*\((?<b>\w)\).&nbsp;(?<vb>\S.*)<br>\s*\((?<c>\w)\).&nbsp;(?<vc>\S.*)<br>\s*\((?<d>\w)\).&nbsp;(?<vd>\S.*)<br>\s*";
            MatchCollection matchCollection = Regex.Matches(s, pattern);
            DataTable dtQuestion = new DataTable();
            dtQuestion.Columns.Add("Serial");
            dtQuestion.Columns.Add("Question");
            dtQuestion.Columns.Add("Image");
            dtAnswerGroup = new DataTable();
            dtAnswerGroup.Columns.Add("IDParent");
            dtAnswerGroup.Columns.Add("Answer");
            dtAnswerGroup.Columns.Add("Decription");
            dtAnswerGroup.Columns.Add("Status", typeof(bool));
            foreach (Match match in matchCollection)
            {
                //MessageBox.Show(match.Groups["number"].Value+" => "+ match.Groups["values"].Value);
                string number = match.Groups["number"].Value;
                string values = match.Groups["caption"].Value;
                if (int.Parse(number) > 31 && int.Parse(number) < 131)
                {
                    dtQuestion.Rows.Add(number, values, "");
                    dtAnswerGroup.Rows.Add(number, match.Groups["a"].Value, match.Groups["va"].Value, false);
                    dtAnswerGroup.Rows.Add(number, match.Groups["b"].Value, match.Groups["vb"].Value, false);
                    dtAnswerGroup.Rows.Add(number, match.Groups["c"].Value, match.Groups["vc"].Value, false);
                    dtAnswerGroup.Rows.Add(number, match.Groups["d"].Value, match.Groups["vd"].Value, false);
                }

            }
            UpdateAnswer();
            grcListenGroup.DataSource = dtQuestion;

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmViewExam frm = new frmViewExam(btnOpen.Text);
            frm.ShowDialog();
        }
        void UpdateAnswer()
        {
            foreach (DataRow rowsAnswer in dtAnswerFull.Rows)
            {
                foreach (DataRow rows in dtAnswerGroup.Rows)
                {
                    if (rowsAnswer["Number"].ToString() == rows["IDParent"].ToString())
                    {
                        if (rowsAnswer["Answer"].ToString() == rows["Answer"].ToString())
                        {
                            rows.SetField("Status", true);
                        }
                    }
                }
            }
        }

        private void grvListenGroup_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string IDParent = grvListenGroup.GetFocusedRowCellValue(colSerial).ToString();
            var query = from row in dtAnswerGroup.AsEnumerable() where row.Field<string>("IDParent") == IDParent select row;
            DataTable dt = query.CopyToDataTable();
            grcAnswerGroup.DataSource = dt;

        }


        private void frmImportExams_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string res = mdGroupTypeQuestion.GetAll(ref dt);
            if (res == "OK")
            {
                lookAudio.Properties.DisplayMember = "AudioName";
                lookAudio.Properties.ValueMember = "ID";
                lookAudio.Properties.DataSource = dt;
            }

        }
        void Insert()
        {
            string IdGroupTypeQuestions = lookAudio.EditValue.ToString();
        }
    }
}