using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public class Provider
    {
        private static string ServerName = "DESKTOP-SSRU1D3\\SQLEXPRESS";
        private static string UserName = "sa";
        private static string Password = "sa2012";
        private static string DatabaseName = "MCTS";
        /// <summary>
        /// Chuỗi Connect kết nối Database
        /// </summary>
        /// <returns> Chuỗi kết nối có pass </returns>
        public static string ConnectString()
        {
            return @"Data Source = " + ServerName + "; Initial Catalog = " + DatabaseName + " ; User ID = " + UserName + "; Password = " + Password + "";
        }
        public static string ErroString(string Project, string Class, string Function, string Erro)
        {
            return string.Format("Erro this {0} => Class: {1}=> Function: {2} => ex: {3}", Project, Class, Function, Erro);
        }
    }
}
