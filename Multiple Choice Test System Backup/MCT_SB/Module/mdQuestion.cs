using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
   public static class mdQuestion
    {
        public static string Insert(ref int IDQuestion, List<string> ltQuestion, byte[] Image)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"insert into QUESTIONS values ('"+ ltQuestion[0] + "','"+ ltQuestion[1] + "','"+ ltQuestion[2] + "',@Image,'"+ltQuestion[3]+"','"+ltQuestion[4]+"','"+ltQuestion[5]+"')";
                SqlCommand cmdInsert = new SqlCommand(query, con);
                cmdInsert.CommandType = CommandType.Text;
                cmdInsert.Parameters.Add("@Image", SqlDbType.Image).Value = Image;
                cmdInsert.Transaction = sqlTrans;
                int result = cmdInsert.ExecuteNonQuery();
                if (result == 1)
                {
                    query = "SELECT IDENT_CURRENT('QUESTIONS')";
                    SqlCommand cmdGetID = new SqlCommand(query, con);
                    cmdGetID.CommandType = CommandType.Text;
                    cmdGetID.Transaction = sqlTrans;

                    object id = cmdGetID.ExecuteScalar();

                    IDQuestion = Convert.ToInt32(id);

                    if (result > 0)
                        sqlTrans.Commit();
                    else
                    {
                        sqlTrans.Rollback();
                        sqlTrans.Dispose();
                        con.Close();
                        return Provider.ErroString("Module", "mdQuestion", "insert", "Insert Question Erro");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdQuestion", "insert", "Insert Question Erro");
                }

                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdQuestion", "insert", e.Message);
            }
        }
        public static string GetTreeQuestion(ref DataTable dtQuestion)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtQuestion);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdQuestion", "GetTreeQuestion", e.Message);
            }
        }
        public static string GetByGroupType(ref DataTable dtQuestion, int GroupTypeID)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "select QUESTIONS.ID,QUESTIONS.Descriptions, QUESTIONS.Status, Images,ANSWERS.ID as IDAnswer, ANSWERS.Descriptions as AnswerDescription  from QUESTIONS,ANSWERS where QUESTIONS.ID = ANSWERS.IdQuestion and ANSWERS.Status = 1 and IdGroupTypeQuestions = " + GroupTypeID;
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtQuestion);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdQuestion", "GetByGroupType", e.Message);
            }
        }
    }
}
