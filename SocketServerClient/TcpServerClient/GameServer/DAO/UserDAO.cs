using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        /// <summary>
        /// 检验账号密码的有效性
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public User VerifyUser(MySqlConnection conn,string userName,string passWord)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand("select * from user where username = @username and password = @password", conn); //从user去除对应userName以及passWord。没有则返回null
                cmd.Parameters.AddWithValue("username", userName);
                cmd.Parameters.AddWithValue("password", passWord);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    User user = new User(id, userName, passWord);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser时出现异常:" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 检测是否已经存在用户名
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckHasUserByUserName(MySqlConnection conn,string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand("select * from user where username = @username", conn); //检测是否存在username.若没有就返回空
                cmd.Parameters.AddWithValue("username", username);

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在CheckHasUserByUserName时出现异常:" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return false;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        public void AddUser(MySqlConnection conn, string userName, string passWord)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand("insert into user set username = @username , password = @password", conn);//向user表中添加username以及password.记得添加完成之后ExecuteNonQuery(): 返回受影响函数，如增、删、改操作；
                cmd.Parameters.AddWithValue("username", userName);
                cmd.Parameters.AddWithValue("password", passWord);
                cmd.ExecuteNonQuery(); //受影响的语句数量
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser时出现异常:" + e);
            }
        }
    }
}
