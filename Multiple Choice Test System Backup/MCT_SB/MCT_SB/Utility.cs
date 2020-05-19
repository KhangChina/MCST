using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCT_SB
{
    public static class Utility
    {
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static bool IsFocusForm(Type type, Form frmParent)
        {
            int i = 0;
            if (frmParent == null) return false;
            foreach (Form frm in frmParent.MdiChildren)
            {
                if (frm.GetType() == type)
                {
                    if (frm.MinimizeBox)
                    {
                        frm.Focus();
                        frm.WindowState = FormWindowState.Normal;
                    }
                    frm.Focus();
                    return true;
                }
                else
                {
                    i++;
                }
            }
            if (i != 0)
                return false;
            return false;
        }
    }
}
