using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

/// <summary>
/// 注册请求
/// </summary>
public class RegistRequest : BaseRequest
{
    private RegistPanel registPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        registPanel = GetComponent<RegistPanel>();
        base.Awake();
    }

    public void SendRequest(string userName, string password)
    {
        string data = userName + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        registPanel.OnRegistReponse(returnCode);
    }

}
