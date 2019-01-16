using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RequestManager : BaseManager
{
    public RequestManager(GameFacade facade) : base(facade)
    {

    }


    private Dictionary<ActionCode, BaseRequest> actionDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode,BaseRequest request)
    {
        actionDict.Add(actionCode, request);
    }

    public void RemoveRequset(ActionCode actionCode)
    {
        actionDict.Remove(actionCode);
    }

    /// <summary>
    /// 获取对应BaseRequest的调用方法
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void HandleReponse(ActionCode actionCode, string data)
    {
        BaseRequest request = actionDict.TryGetV<ActionCode, BaseRequest>(actionCode);
        if (request == null)
        {
            Debug.LogError("无法得到RequestCode[" + actionCode + "]对应的Request");
            return;
        }
        request.OnResponse(data);
    }
}
