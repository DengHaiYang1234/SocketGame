using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    /// <summary>
    /// 控制层   主要控制与数据库的数据交互?
    /// </summary>
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();
        private Server server;


        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }


        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDic.Add(defaultController.RequestCode, defaultController);
            controllerDic.Add(RequestCode.User, new UserController());
            controllerDic.Add(RequestCode.Room, new RoomController());
            controllerDic.Add(RequestCode.Game, new GameController());
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public void HandleRequest(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            BaseController controller; //获取对象
            bool isGet =  controllerDic.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("无法得到[" + requestCode + "[所对应的Controller,无法处理请求");
                return;
            }

            //对象方法
            string methodName = Enum.GetName(typeof(ActionCode),actionCode);
            //反射
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告]在Controller[" + controller.GetType()  +  "]没有对应的处理方法：[" + methodName + "]");
                return;
            }
            //传递对象方法参数，并开始执行方法
            object[] parameters = new object[] {data,client,server};
            //调用方法处理消息
            object o =  mi.Invoke(controller, parameters);

            if (o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }

            //请求已处理。返回消息处理结果
            server.SendResponse(client, actionCode, o as string);
        }
    }
}
