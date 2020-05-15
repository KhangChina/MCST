using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public class mdGroupTypeQuestion
    {
        public static string Insert(ref int IDQuestion, List<string> ltGroupTypeQuestion,  byte[] Image )
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"insert into GROUP_TYPE_QUESTIONS values ('"+ ltGroupTypeQuestion[0]+ "','"+ltGroupTypeQuestion[1]+"','@Image','"+ltGroupTypeQuestion[2]+"','"+ltGroupTypeQuestion[3]+"','"+ltGroupTypeQuestion[4]+"')";
                SqlCommand cmdInsert = new SqlCommand(query, con);
                cmdInsert.CommandType = CommandType.Text;
                cmdInsert.Parameters.Add("@Image", SqlDbType.Image).Value = Image;
                cmdInsert.Transaction = sqlTrans;
                int result = cmdInsert.ExecuteNonQuery();
                if (result == 1)
                {
                    query = "SELECT IDENT_CURRENT('GROUP_TYPE_QUESTIONS')";
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
                        return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", "Insert GROUP_TYPE_QUESTIONS Erro");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", "Insert GROUP_TYPE_QUESTIONS Erro");
                }

                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", e.Message);
            }
        }
        public static string Insert(ref int IDQuestion, List<string> ltGroupTypeQuestion)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"insert into GROUP_TYPE_QUESTIONS values ('" + ltGroupTypeQuestion[0] + "','" + ltGroupTypeQuestion[1] + "','','" + ltGroupTypeQuestion[2] + "','" + ltGroupTypeQuestion[3] + "','" + ltGroupTypeQuestion[4] + "')";
                SqlCommand cmdInsert = new SqlCommand(query, con);
                cmdInsert.CommandType = CommandType.Text;               
                cmdInsert.Transaction = sqlTrans;
                int result = cmdInsert.ExecuteNonQuery();
                if (result == 1)
                {
                    query = "SELECT IDENT_CURRENT('GROUP_TYPE_QUESTIONS')";
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
                        return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", "Insert GROUP_TYPE_QUESTIONS Erro");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", "Insert GROUP_TYPE_QUESTIONS Erro");
                }

                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", e.Message);
            }
        }
    }
}
