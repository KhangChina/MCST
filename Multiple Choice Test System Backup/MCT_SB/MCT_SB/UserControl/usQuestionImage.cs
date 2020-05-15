using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MCT_SB.UserControl
{
    public partial class usQuestionImage : DevExpress.XtraEditors.XtraUserControl
    {
        public usQuestionImage()
        {
            InitializeComponent();
        }
        public Image GetImage()
        {
            return pic.Image; 
        }
    }
}
