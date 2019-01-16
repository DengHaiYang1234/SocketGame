using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
    class ConnHelper
    {
        public const string CONNECTIONSTRING  = "datasource = 127.0.0.1;port=3306;database=mygamesql;user=root;pwd=root;";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING); //连接数据库
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库出现异常:" + e);
                return null;
            }
        }

        public static void CloseConnection(MySqlConnection mySqlConnection)
        {
            try
            {
                if (mySqlConnection != null)
                    mySqlConnection.Close();
                else
                {
                    Console.WriteLine("mySqlConnection不能为空");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("关闭数据库出现异常:" + e);
                return;
            }
            
        }
    }
}
