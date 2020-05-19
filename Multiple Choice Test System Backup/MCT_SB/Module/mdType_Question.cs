using System;
using System.Data;
using System.Data.SqlClient;

namespace Module
{
    public static class mdType_Question
    {
        public static string GetAll(ref DataTable dtType_Question)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "select * from TYPE_QUESTIONS";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtType_Question);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdType_Question", "GetAll", e.Message);
            }
        }
        public static string GetTreeType(ref DataTable dtType)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "select TYPE_QUESTIONS.ID + 0.1 as ID,0 as PARENTID, Name from TYPE_QUESTIONS UNION All select GROUP_TYPE_QUESTIONS.ID + 0.2 as ID, IdTypeQuestion + 0.1 as PARENTID, Name from GROUP_TYPE_QUESTIONS";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtType);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdType", "GetTreeType", e.Message);
            }
        }
        public static string Delete(int IDType)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"DELETE TYPE_QUESTIONS WHERE ID = " + IDType;
                SqlCommand cmdDelete = new SqlCommand(query, con);
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.Transaction = sqlTrans;
                int result = cmdDelete.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdType_Question", "Delete", "Datete Type Question Error");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdType_Question", "Delete", e.Message);
            }
        }
    }
}
