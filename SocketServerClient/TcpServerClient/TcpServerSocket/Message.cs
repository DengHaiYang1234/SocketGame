using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerSocket
{
    public class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;//存储的字节数据长度

        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        //剩余长度
        public int RemainSize
        {
            get {return data.Length - startIndex; }
        }

        public void AddCount(int count)
        {
            //目前的消息总长度
            startIndex += count;
        }

        /// <summary>
        /// 解析数据   解决粘包
        /// </summary>
        public void ReadMessage()
        {
            while (true)
            {
                //是否解析出来消息的长度
                if (startIndex <= 4) return;
                //消息长度
                int count = BitConverter.ToInt32(data, 0); //默认0~4.这是因为ToInt32默认是4个字节
                if ((startIndex - 4) >= count)
                {
                    Console.WriteLine("count:" + count);
                    //前四个为消息长度，不要解析。后面才是消息内容
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出来的一条数据：" + s);
                    //count + 4: 消息中的前四个字节是该消息的长度 
                    //startIndex - (4 + count): 解析完该消息之后。数组剩余的消息长度
                    Array.Copy(data, count + 4, data, 0, startIndex - (4 + count));
                    //更新消息存储的起始位置 
                    startIndex -= (count + 4);
                }
                else
                {
                    return;
                }
            }


        }

    }
}
