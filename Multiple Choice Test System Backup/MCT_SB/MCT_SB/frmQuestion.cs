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
    public partial class frmQuestion : DevExpress.XtraEditors.XtraForm
    {
        public frmQuestion()
        {
            InitializeComponent();
        }
        void LoadTypeQuestion ()
        {
            DataTable dtLoadTypeQuestion = new DataTable();
            string res = mdType_Question.GetAll(ref dtLoadTypeQuestion);
            if(res == "OK")
            {
                grcTypeQuestion.DataSource = dtLoadTypeQuestion;
            }
        }

        private void frmQuestion_Load(object sender, EventArgs e)
        {
            LoadTypeQuestion();
        }
    }
}