using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

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
        public static bool checkStatus(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Status"].ToString() == true.ToString())
                    return true;
            }
            return false;
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

        public static string Detect(string pathIn, string pathOut, ref List<string> ImageName)
        {

            Document document = new Document();
            document.LoadFromFile(pathIn);
            int index = 1;
            foreach (Section section in document.Sections)
            {
                foreach (Paragraph paragraph in section.Paragraphs)
                {
                    foreach (DocumentObject docObject in paragraph.ChildObjects)
                    {

                        if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                        {
                            DocPicture picture = docObject as DocPicture;
                            String imageName = String.Format(pathOut + "\\{0}.png", index);
                            if (picture.Image.Width > 100 && picture.Image.Height > 100)
                            {
                                ImageName.Add(imageName);
                                picture.Image.Save(imageName, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            index++;
                        }
                    }

                }
            }
            return document.GetText();
        }
        public static string TextToHtml(string text)
        {
            text = HttpUtility.HtmlEncode(text);
            text = text.Replace("\r\n", "\r");
            text = text.Replace("\n", "\r");
            text = text.Replace("\r", "<br>\r\n");
            text = text.Replace("  ", " &nbsp;");
            return text;
        }
        public static string GetTextFromWord(string paths)
        {
            StringBuilder text = new StringBuilder();
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = paths;
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);

            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                text.Append(" \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString());
            }
            word.Quit();
            docs.Close();
            return text.ToString();
        }
    }
}
