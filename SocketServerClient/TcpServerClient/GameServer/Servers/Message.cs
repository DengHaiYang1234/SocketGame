using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Servers
{
    /// <summary>
    /// 消息处理
    /// </summary>
    class Message
    {

        private int startIndex = 0;
        private byte[] data = new byte[1024];


        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }

        public void AddCount(int count)
        {
            //目前消息接收开始位置
            startIndex += count; 
        }

        /// <summary>
        /// 读取接收的消息。并做粘包处理
        /// </summary>
        /// <param name="newDataAmount"></param>
        public void ReadMessage(int newDataAmount,Action<RequestCode,ActionCode,string> processDataCallBack)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return; //消息长度都未解析出来

                //获得消息的长度
                int count = BitConverter.ToInt32(data, 0); //从0开始解析4个字节.这是因为ToInt32默认是4个字节

                if ((startIndex - 4) >= count)
                {
                    //string s = Encoding.UTF8.GetString(data, 4, count); //从第四个字节开始解析count长度的数据

                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);

                    string s = Encoding.UTF8.GetString(data, 12, count - 8);

                    processDataCallBack(requestCode, actionCode, s);

                    Array.Copy(data, count + 4, data, 0, startIndex - (4 + count));

                    startIndex -= (count + 4);
                }
                else
                    return;

            }
        }


        /// <summary>
        /// 打包数据
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PackData(ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray();
            return newBytes.Concat(dataBytes).ToArray();
        }
    }
}
