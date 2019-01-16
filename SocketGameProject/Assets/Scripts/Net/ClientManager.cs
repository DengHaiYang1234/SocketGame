using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using Common;

/// <summary>
/// 与服务器的socket连接
/// </summary>
public class ClientManager :BaseManager
{

    public ClientManager(GameFacade facade) : base(facade)
    {

    }
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

    private Socket clientSocket;

    private Message msg = new Message();

    public override void OnInit()
    {
        base.OnInit(); //父类方法执行


        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //建立Socket
        try
        {
            clientSocket.Connect(IP, PORT); //连接Socket
            Start();
        }
        catch(Exception e)
        {
            Debug.LogWarning("无法连接服务端。请检查网络." + e);
        }
    }

    /// <summary>
    /// 启动Socket并开始接收消息
    /// </summary>
    private void Start()
    {
        clientSocket.BeginReceive(msg.Data,msg.StartIndex,msg.RemainSize, SocketFlags.None,ReceiveCallBack,null);
    }
    /// <summary>
    /// 消息回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }
            //接收到的消息长度
            int count = clientSocket.EndReceive(ar);
            //解析消息
            msg.ReadMessage(count,OnProcessDataCallBack);
            Start();
        }
        catch(Exception e)
        {
            Debug.LogError("ClientManager ReceiveCallBack is Called.But " + e);
        }
    }

    /// <summary>
    /// 解析消息回调
    /// </summary>
    /// <param name="actionCode"> 调用方法 </param>
    /// <param name="data"> 服务器返回结果 </param>
    private void OnProcessDataCallBack(ActionCode actionCode, string data)
    {
        GameFacade.Instance.HandleReponse(actionCode, data);
    }

    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法关闭跟服务器端的连接:" + e);
        }
    }


}
