using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConversionToByte
{
    class Program
    {
        static void Main(string[] args)
        {
            //按字符来处理
            byte[] data = Encoding.UTF8.GetBytes("1");
            Console.WriteLine("length:" + data.Length);
            //按值来处理  始终占有4个字节
            //int count = 10000;
            //byte[] data = BitConverter.GetBytes(count);
            foreach (var b in data)
            {
                Console.WriteLine(b + ":");
            }

        }
    }
}
