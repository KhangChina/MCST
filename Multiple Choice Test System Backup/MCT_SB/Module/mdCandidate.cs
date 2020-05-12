using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Module
{
    public static class mdCandidate
    {
        public static string GetAll(ref DataTable dtoCandidate)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();

                string query = "SELECT * FROM CANDIDATES";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtoCandidate);
                con.Close();

                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdCandidate", "GetAll", e.Message);
            }
        }
        public static string Insert(ref int IDCandidate, List<string> ltCandidate,byte[] Image)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"INSERT INTO CANDIDATES VALUES('"+ ltCandidate[0] + "',N'"+ ltCandidate[1] + "','"+ ltCandidate[2] + "','"+ ltCandidate[3] + "',N'"+ ltCandidate[4] + "','"+ ltCandidate[5] + "','"+ ltCandidate[6] + "',@Image,'"+ ltCandidate[7] + "')";
                SqlCommand cmdInsert = new SqlCommand(query, con);
                cmdInsert.CommandType = CommandType.Text;             
                cmdInsert.Parameters.Add("@Image", SqlDbType.Image).Value = Image;
                cmdInsert.Transaction = sqlTrans;
                int result = cmdInsert.ExecuteNonQuery();
                if (result == 1)
                {
                    query = "SELECT IDENT_CURRENT('CANDIDATES')";
                    SqlCommand cmdGetID = new SqlCommand(query, con);
                    cmdGetID.CommandType = CommandType.Text;
                    cmdGetID.Transaction = sqlTrans;

                    object id = cmdGetID.ExecuteScalar();

                    IDCandidate = Convert.ToInt32(id);

                    if (result > 0)
                        sqlTrans.Commit();
                    else
                    {
                        sqlTrans.Rollback();
                        sqlTrans.Dispose();
                        con.Close();
                        return Provider.ErroString("Module", "mdCandidate", "insert", "Insert Candidate Erro");
                    }
                }
                else
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "mdCandidate", "insert", "Insert Candidate Erro");
                }

                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdCandidate", "insert", e.Message);
            }
        }
        public static string Delete(int IDCandidate)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"DELETE FROM  CANDIDATES WHERE Id = " + IDCandidate;//sql cấu hình sẵn xoá luôn những record ở table chứa khoá ngoại của bảng này
                SqlCommand cmdDelete = new SqlCommand(query, con);
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.Transaction = sqlTrans;
                int result = cmdDelete.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();
                    return Provider.ErroString("Module", "Candidate", "Delete", "Delete Candidate Erro");
                }
                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "Candidate", "Delete", e.Message);
            }
        }
        public static string GetByID(ref DataTable dtCandidate,int IDCandidate)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();

                string query = "SELECT * FROM CANDIDATES where ID = "+IDCandidate+"";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtCandidate);
                con.Close();

                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdCandidate", "GetByID", e.Message);
            }
        }     
        public static string UpdateByID(ref int IDCandidate, List<string> ltCandidate, byte[] Image)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);

                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();

                string query = @"update CANDIDATES set IDCard = '" + ltCandidate[0] + "',Name = N'" + ltCandidate[1] + "',Gender = N'" + ltCandidate[2] + "',DateOfBirth = '" + ltCandidate[3] + "',Phone = '" + ltCandidate[4] + "',Address=N'" + ltCandidate[5] + "',Email='" + ltCandidate[6] + "',Image=@Image,Status='" + ltCandidate[7] + "' where ID = '" + IDCandidate + "'";
                SqlCommand cmdUpdate = new SqlCommand(query, con);
                cmdUpdate.CommandType = CommandType.Text;
                cmdUpdate.Parameters.Add("@Image", SqlDbType.Image).Value = Image;
                cmdUpdate.Transaction = sqlTrans;
                int result = cmdUpdate.ExecuteNonQuery();
                if (result != 1)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                    con.Close();

                    return Provider.ErroString("Module", "mdCandidate", "UpdateByID", "Cập nhật dữ liệu Candidate lỗi");
                }

                sqlTrans.Commit();
                sqlTrans.Dispose();
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdCandidate", "UpdateByID", e.Message);
            }
        }
    }
}



