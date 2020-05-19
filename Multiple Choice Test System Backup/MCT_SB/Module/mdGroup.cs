using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public static class mdGroup
    {
        public static string GetAll (ref DataTable dtGroup)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "select * from GROUPS";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtGroup);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGroup", "GetAll", e.Message);
            }
        }
        public static string GetTreePart(ref DataTable dtGroup)
        {
            try
            {
                string conStr = Provider.ConnectString();
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                string query = "select GROUPS.ID+0.1 as ID,0 as PARENTID, Name,Status from GROUPS UNION ALL select PART.ID + 0.2 as ID,PART.IdGroup + 0.1 as PARENTID,Name,Status from PART";
                SqlCommand cmdGetData = new SqlCommand(query, con);
                cmdGetData.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdGetData);
                int result = da.Fill(dtGroup);
                con.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return Provider.ErroString("Module", "mdGroup", "GetTreePart", e.Message);
            }
        }

    }
}
