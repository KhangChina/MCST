using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Module
{
    public static class mdPart
    {
        public static string GetAll(ref DataTable dtPart, string IDGroup)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT * FROM PART Where IDGroup = " + IDGroup + " and Status = 1";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtPart);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "GetAll", e.Message);
            }
        }
        public static string GetAll(ref DataTable dtPart)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT ID,Name FROM PART Where Status = 1";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtPart);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "GetAll", e.Message);
            }
        }
        public static string Insert(ref int IDPart,List <string> ltPart)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"insert into PART values (N'"+ ltPart [0]+ "','"+ ltPart [1]+ "',N'"+ ltPart [2]+ "','"+ ltPart [3]+ "')";
                SqlCommand cmdInsert = new SqlCommand(query, con);
                cmdInsert.CommandType = CommandType.Text;                
                cmdInsert.Transaction = sqlTrans;
                int result = cmdInsert.ExecuteNonQuery();
                if (result == 1)
                {
                    query = "SELECT IDENT_CURRENT('PART')";
                    SqlCommand cmdGetID = new SqlCommand(query, con);
                    cmdGetID.CommandType = CommandType.Text;
                    cmdGetID.Transaction = sqlTrans;

                    object id = cmdGetID.ExecuteScalar();

                    IDPart = Convert.ToInt32(id);

                    if (result > 0)
                        sqlTrans.Commit();
                    else
                    {
                        sqlTrans.Rollback();
                        sqlTrans.Dispose();
                        con.Close();
                        return Provider.ErroString("Module", "mdPart", "Insert", "Insert Part Erro");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdPart", "Insert", "Insert Part Erro");
                }

                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "Insert", e.Message);
            }
        }
        public static string Update(int IDPart, List<string> ltPart)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"Update PART set Name =N'"+ ltPart [0]+ "',IdGroup = '"+ ltPart [1]+ "',Descriptions =N'"+ ltPart [2]+ "',Status='"+ltPart[3]+"' where ID = '"+ IDPart + "';";
                SqlCommand cmdUpdate = new SqlCommand(query, con);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.Transaction = sqlTrans;
                int result = cmdUpdate.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();

                    return Provider.ErroString("Module", "mdPart", "Update", "Update Part Erro");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "Update", e.Message);
            }
        }
        public static string GetByID(ref DataTable dtPart,string IDPart)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT * FROM PART Where ID = "+ IDPart + "";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtPart);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "GetByID", e.Message);
            }
        }
        public static string Delete(int IDPart)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"DELETE PART WHERE ID = " + IDPart;
                SqlCommand cmdDelete = new SqlCommand(query, con);
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.Transaction = sqlTrans;
                int result = cmdDelete.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdPart", "Delete", "Datete Part Erro");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "Delete", e.Message);
            }
        }
        public static string getGroup(string IDPart)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT IdGroup FROM PART WHERE ID = '" + IDPart + "'";
                SqlCommand cmdCheckAudio = new SqlCommand(query, con);
                string group = cmdCheckAudio.ExecuteScalar().ToString();
                con.Close();
                return group;
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "GetGroup", e.Message);
            }
        }
    }
}
