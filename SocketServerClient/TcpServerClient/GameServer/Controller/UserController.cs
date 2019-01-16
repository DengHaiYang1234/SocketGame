using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;
using Common;


namespace GameServer.Controller
{
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        /// <summary>
        /// 注：  该方法是由反射调用 ControllerManager---->HandleRequest
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user =  userDAO.VerifyUser(client.MySqlConnection,strs[0], strs[1]);
            if (user == null)
            {
                return ((int) ReturnCode.Fail).ToString();
            }
            else
            {
                Result result = resultDAO.GetResultByUserId(client.MySqlConnection, user.Id);
                client.SetUserData(user, result);
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(),user.Username,result.TotalCount,result.WinCount);
            }
        }

        /// <summary>
        /// 注：  该方法是由反射调用 ControllerManager---->HandleRequest
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string userName = strs[0];
            string password = strs[1];
            bool isHasUser = userDAO.CheckHasUserByUserName(client.MySqlConnection, userName);
            if (isHasUser)
            {
                return ((int) ReturnCode.Fail).ToString();
            }
            userDAO.AddUser(client.MySqlConnection, userName, password);
            return ((int)ReturnCode.Success).ToString();
        }
    }
}
