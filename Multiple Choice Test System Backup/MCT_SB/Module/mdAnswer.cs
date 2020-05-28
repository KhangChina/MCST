using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Module
{
    public static class mdAnswer
    {
        public static string Insert(ref int IDAnswer, List<string> ltAnswer)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"insert into ANSWERS values (N'" + ltAnswer[0] + "','" + ltAnswer[1] + "','','" + ltAnswer[2] + "')";
                SqlCommand cmdInsert = new SqlCommand(query, con);
                cmdInsert.CommandType = CommandType.Text;
                cmdInsert.Transaction = sqlTrans;
                int result = cmdInsert.ExecuteNonQuery();
                if (result == 1)
                {
                    query = "SELECT IDENT_CURRENT('ANSWERS')";
                    SqlCommand cmdGetID = new SqlCommand(query, con);
                    cmdGetID.CommandType = CommandType.Text;
                    cmdGetID.Transaction = sqlTrans;

                    object id = cmdGetID.ExecuteScalar();

                    IDAnswer = Convert.ToInt32(id);

                    if (result > 0)
                        sqlTrans.Commit();
                    else
                    {
                        sqlTrans.Rollback();
                        sqlTrans.Dispose();
                        con.Close();
                        return Provider.ErroString("Module", "mdAnswer", "insert", "Insert Answer Erro");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdAnswer", "insert", "Insert Answer Erro");
                }

                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdAnswer", "insert", e.Message);
            }
        }
        public static string GetAnswerByQuestion(ref DataTable dtAnswer, string IDQuestion)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "select ID,Descriptions,Status  from ANSWERS where IdQuestion = " + IDQuestion;
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtAnswer);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdAnswer", "GetAnswerByQuestion", e.Message);
            }
        }
        public static bool CheckExitAnser(string ID)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT COUNT(*) FROM ANSWERS WHERE ID = '" + ID + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int count = (int)cmd.ExecuteScalar();
                con.Close();
                return count > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        public static string Update(string IDAnswer, List<string> ltanswer)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"Update ANSWERS set Descriptions =N'" + ltanswer[0] + "', Status = '"+ ltanswer[1] +"' where ID = '" + IDAnswer + "';";
                SqlCommand cmdUpdate = new SqlCommand(query, con);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.Transaction = sqlTrans;
                int result = cmdUpdate.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();

                    return Provider.ErroString("Module", "mdAnswer", "Update", "Update Answer Erro");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdAnswer", "Update", e.Message);
            }
        }
        public static string Delete(string IDAnswer)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"DELETE ANSWERS WHERE ID = " + IDAnswer;
                SqlCommand cmdDelete = new SqlCommand(query, con);
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.Transaction = sqlTrans;
                int result = cmdDelete.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdAnswer", "Delete", "Datete Answer Erro");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdAnswer", "Delete", e.Message);
            }
        }
    }
}
