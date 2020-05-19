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

    }

}
