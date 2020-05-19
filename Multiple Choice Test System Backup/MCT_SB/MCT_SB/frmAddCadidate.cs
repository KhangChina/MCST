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
    public partial class frmAddCadidate : DevExpress.XtraEditors.XtraForm
    {
        public frmAddCadidate()
        {
            InitializeComponent();
            btnSave.Enabled = true;
        }
        public frmAddCadidate(int IDCandidates)
        {
            InitializeComponent();
            btnUpdate.Enabled = true;
            dt = new DataTable();
            LoadUpdate(IDCandidates);
            
        }
        DataTable dt;
        void LoadUpdate(int IDCandidates)
        {
            // mdCandidate.
            if(IDCandidates > 0)
            {
                string res = mdCandidate.GetByID(ref dt, IDCandidates);
                if(res == "OK")
                {
                    txtIDCard.Text = dt.Rows[0]["IDCard"].ToString();
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    cbxGender.Text = dt.Rows[0]["Gender"].ToString();
                    dateBirthDay.Text = dt.Rows[0]["DateOfBirth"].ToString();               
                    txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                    mmAddress.Text = dt.Rows[0]["Address"].ToString();
                    txtMail.Text = dt.Rows[0]["Email"].ToString();
                    cbxStatus.Checked = bool.Parse(dt.Rows[0]["Status"].ToString());

                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dt.Rows[0]["Image"]);
                        MemoryStream mem = new MemoryStream(data);
                        picImageCandidate.Image = Image.FromStream(mem);
                    }
                    catch
                    {
                        picImageCandidate.Image = null;
                    }
                       
                    
                }
                else
                {
                    XtraMessageBox.Show(res);
                }
            }
            
           
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> ltCandidate = new List<string>();
            ltCandidate.Add(txtIDCard.Text);
            ltCandidate.Add(txtName.Text);
            ltCandidate.Add(cbxGender.Text);
            ltCandidate.Add(dateBirthDay.EditValue.ToString());
            ltCandidate.Add(txtPhone.Text);
            ltCandidate.Add(mmAddress.Text);
            ltCandidate.Add(txtMail.Text);
            ltCandidate.Add(cbxStatus.EditValue.ToString());
            byte[] Img = null;
            if (picImageCandidate.Image != null)
            {
                Img = imageToByteArray(picImageCandidate.Image);
            }
            int ID = -1;
            string res = mdCandidate.Insert(ref ID, ltCandidate, Img);
            if (res != "OK")
                XtraMessageBox.Show(res);
            else
                this.Close();
        }
       
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int IDCandidates = int.Parse(dt.Rows[0]["ID"].ToString());
            List<string> ltCandidate = new List<string>();
            ltCandidate.Add(txtIDCard.Text);
            ltCandidate.Add(txtName.Text);
            ltCandidate.Add(cbxGender.Text);
            ltCandidate.Add(dateBirthDay.EditValue.ToString());
            ltCandidate.Add(txtPhone.Text);
            ltCandidate.Add(mmAddress.Text);
            ltCandidate.Add(txtMail.Text);
            ltCandidate.Add(cbxStatus.EditValue.ToString());
            byte[] Img = null;
            if (picImageCandidate.Image != null)
            {
                Img = imageToByteArray(picImageCandidate.Image);
            }
            //int ID = -1;
            string res = mdCandidate.UpdateByID(ref IDCandidates, ltCandidate, Img);
            if (res != "OK")
                XtraMessageBox.Show(res);
            else
                this.Close();

        }
    }
}