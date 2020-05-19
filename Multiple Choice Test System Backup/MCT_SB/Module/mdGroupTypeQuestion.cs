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

                string query = @"insert into GROUP_TYPE_QUESTIONS values ('" + ltGroupTypeQuestion[0] + "','" + ltGroupTypeQuestion[1] + "','image','" + ltGroupTypeQuestion[2] + "','" + ltGroupTypeQuestion[3] + "','" + ltGroupTypeQuestion[4] + "')";
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
                        return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", "Insert GROUP_TYPE_QUESTIONS Error");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdGroupTypeQuestion", "Insert", "Insert GROUP_TYPE_QUESTIONS Error");
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
        public static string Update(int IDGroupType, List<string> ltGroupType)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"Update GROUP_TYPE_QUESTIONS set Name =N'" + ltGroupType[0] + "',Descriptions = '" + ltGroupType[1] + "',Images = 'image',AudioName =N'" + ltGroupType[2] + "',Status='" + ltGroupType[3] + "' where ID = '" + IDGroupType + "';";
                SqlCommand cmdUpdate = new SqlCommand(query, con);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.Transaction = sqlTrans;
                int result = cmdUpdate.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();

                    return Provider.ErroString("Module", "mdGroupTypeQuestion", "Update", "Update Group Type Question Error");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGrouptypeQuestion", "Update", e.Message);
            }
        }
        public static string GetAll(ref DataTable dtGroupTypeQuestion)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT * FROM GROUP_TYPE_QUESTIONS";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtGroupTypeQuestion);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdPart", "GetAll", e.Message);
            }
        }
        public static bool CheckAudioFile(string AudioName)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT COUNT(*) FROM GROUP_TYPE_QUESTIONS WHERE AudioName = '"+ AudioName +"'";
                SqlCommand cmdCheckAudio = new SqlCommand(query, con);
                int count = (int)cmdCheckAudio.ExecuteScalar();
                con.Close();
                return count > 0 ? false : true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static string Delete(int IDGroupType)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                string query = @"DELETE GROUP_TYPE_QUESTIONS WHERE ID = " + IDGroupType;
                SqlCommand cmdDelete = new SqlCommand(query, con);
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.Transaction = sqlTrans;
                int result = cmdDelete.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdGroupTypeQuestion", "Delete", "Datete Group Type Question Error");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGroupTypeQuestion", "Delete", e.Message);
            }
        }
        public static string GetByID(ref DataTable dtGroupType, string IDGroupType)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "SELECT * FROM GROUP_TYPE_QUESTIONS Where ID = " + IDGroupType + "";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtGroupType);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGroupTypeQuestion", "GetByID", e.Message);
            }
        }
    }
}
