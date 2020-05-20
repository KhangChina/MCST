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
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Columns;

namespace MCT_SB
{
    public partial class frmQuestion : DevExpress.XtraEditors.XtraForm
    {
        public frmQuestion()
        {
            InitializeComponent();
        }
        
        private void frmQuestion_Load(object sender, EventArgs e)
        {
            LoadTreeList();
        }
        void TreeLoad()
        {
            //TreeListNode node = tree.FocusedNode;
            foreach (TreeListNode node in tree.Nodes)
            {
                node.ImageIndex = 2;
                TreeListColumn column = tree.Columns[0];
                foreach (TreeListNode item in node.Nodes)
                {
                    item.ImageIndex = 1;
                }
            }
        }
        void LoadTreeList()
        {
            tree.SelectImageList = imageCollection1;
            tree.ImageIndexFieldName = "ImageIndex";
            tree.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;
            DataTable dtType = new DataTable();
            string res = mdType_Question.GetTreeType(ref dtType);
            if (res == "OK")
            {
                tree.DataSource = dtType;
            }
            else
            {
                XtraMessageBox.Show(res);
            }
        }
        private void tree_Load(object sender, EventArgs e)
        {
            TreeLoad();
        }

        private void tree_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = tree.FocusedNode;
            string reportName = node.GetValue("ID").ToString();
            string[] arrListStr = reportName.Split('.');
            if (arrListStr[1] == "1")
            {
                btnDeleteGroupType.Enabled = false;
                btnUpdateGroupType.Enabled = false;
                btnRemoveType.Enabled = true;
                btnAddType.Enabled = true;
                btnUpdateType.Enabled = true;
            }
            else
            {
                btnDeleteGroupType.Enabled = true;
                btnUpdateGroupType.Enabled = true;
                btnRemoveType.Enabled = false;
                btnAddType.Enabled = false;
                btnUpdateType.Enabled = false;
                loadGridQuestion();
            }
        }
        private  void loadGridQuestion ()
        {
            TreeListNode node = tree.FocusedNode;
            object reportName = node.GetValue("ID");
            int IdGroupType = int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
            DataTable dt = new DataTable();
            string res = mdQuestion.GetByGroupType(ref dt, IdGroupType);
            if (res == "OK")
                grcQuestion.DataSource = dt;
            else XtraMessageBox.Show(res);
        }

        private void btnAddType_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddType frm = new frmAddType();
            frm.ShowDialog();
            LoadTreeList();
            TreeLoad();//Set Icon
        }

        private void btnUpdateType_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = tree.FocusedNode;
            object reportName = node.GetValue("ID");
            int ID = int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
            frmAddType frm = new frmAddType(ID);
            frm.ShowDialog();
            LoadTreeList();
            TreeLoad();//Set Icon
        }

        private void btnRemoveType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                TreeListNode node = tree.FocusedNode;
                object reportName = node.GetValue("ID");
                int ID = int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
                string res = mdType_Question.Delete(ID);
                if (res == "OK")
                {
                    XtraMessageBox.Show("Complete !");
                    LoadTreeList();
                    TreeLoad();
                }
                else
                {
                    XtraMessageBox.Show(res);
                }
            }
        }

        private void btnAddGroupType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = tree.FocusedNode;
            string reportName = node.GetValue("PARENTID").ToString();
            string[] arrListStr = reportName.Split('.');
            int parentID = int.Parse(arrListStr[0]);
            if (parentID == 0)
            {
                parentID = int.Parse(node.GetValue("ID").ToString().Split('.')[0]);
            }
            frmAddGroupTypeQuestion frm = new frmAddGroupTypeQuestion(-1, parentID);
            frm.ShowDialog();
            LoadTreeList();
            TreeLoad();//Set Icon
        }

        private void btnUpdateGroupType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = tree.FocusedNode;
            object reportName = node.GetValue("ID");
            int ID = int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
            frmAddGroupTypeQuestion frm = new frmAddGroupTypeQuestion(ID, -1);
            frm.ShowDialog();
            LoadTreeList();
            TreeLoad();//Set Icon
        }

        private void btnDeleteGroupType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                TreeListNode node = tree.FocusedNode;
                object reportName = node.GetValue("ID");
                int ID = int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
                string res = mdGroupTypeQuestion.Delete(ID);
                if (res == "OK")
                {
                    XtraMessageBox.Show("Complete !");
                    LoadTreeList();
                    TreeLoad();
                }
                else
                {
                    XtraMessageBox.Show(res);
                }
            }
        }
    }
}