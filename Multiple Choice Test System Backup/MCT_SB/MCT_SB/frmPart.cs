using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using Module;
using System;
using System.Data;
using System.Windows.Forms;

namespace MCT_SB
{
    public partial class frmPart : DevExpress.XtraEditors.XtraForm
    {
        public frmPart()
        {
            InitializeComponent();
        }

        private void frmPart_Load(object sender, EventArgs e)
        {
            LoadTreeList();
        }
        void LoadTreeList()
        {
            tree.SelectImageList = imageCollection1;
            tree.ImageIndexFieldName = "ImageIndex";
            tree.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;
            DataTable dtPart = new DataTable();
            string res = mdGroup.GetTreePart(ref dtPart);
            if (res == "OK")
            {
                tree.DataSource = dtPart;
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
        void TreeLoad()
        {
            //TreeListNode node = tree.FocusedNode;
            foreach (TreeListNode node in tree.Nodes)
            {
                node.ImageIndex = 2;
                TreeListColumn column = tree.Columns[0];
                foreach (TreeListNode item in node.Nodes)
                {
                    // object reportName = item.GetValue(column);
                    //XtraMessageBox.Show(reportName.ToString());
                    item.ImageIndex = 1;
                }
            }
        }
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddPart frm = new frmAddPart();
            frm.ShowDialog();
            LoadTreeList();
            TreeLoad();//Set Icon
        }
        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = tree.FocusedNode;
            object reportName = node.GetValue("ID");
            int IDPart =int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
            frmAddPart frm = new frmAddPart (IDPart);
            frm.ShowDialog();
            LoadTreeList();
            TreeLoad();//Set Icon
        }
        private void btnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(XtraMessageBox.Show("Are you sure you want to delete ?", "Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                TreeListNode node = tree.FocusedNode;
                object reportName = node.GetValue("ID");
                int IDPart = int.Parse(Math.Truncate(double.Parse(reportName.ToString())).ToString());
                string res = mdPart.Delete(IDPart);
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
        private void tree_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            //XtraMessageBox.Show("Hello");
            TreeListNode node = tree.FocusedNode;
            string reportName = node.GetValue("ID").ToString();
            string[] arrListStr = reportName.Split('.');
            if(arrListStr[1]=="1")
            {
                btnRemove.Enabled = false;
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
            }
            else
            {
                btnRemove.Enabled = true;
                btnAdd.Enabled = true;
                btnUpdate.Enabled = true;
            }
        }
    }
}