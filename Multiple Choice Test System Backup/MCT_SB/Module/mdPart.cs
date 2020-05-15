using System;
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
    }
}
