﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MCT_SB
{
    public partial class frmViewExam : DevExpress.XtraEditors.XtraForm
    {
        public frmViewExam()
        {
            InitializeComponent();
        }
        public frmViewExam(string path)
        {
            InitializeComponent();
            richEditControl1.LoadDocument(path);
        }
    }
}