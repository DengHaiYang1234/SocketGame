using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class MessagePanel : BasePanel
{
    //auto
    private Text Txt_Message = null;
    private float showTime = 1f;
    private string message = null;
    
    public override void InitStart()
    {
        Txt_Message = gameObject.transform.Find("Txt_Message").GetComponent<Text>();

    }

    private void Update()
    {
        if (message != null)
        {
            ShowMessage(message);
            message = null;
        }
    }

    public void ShowMessageSync(string msg)
    {
        message = msg;
    }


    public void ShowMessage(string msg)
    {
        Txt_Message.CrossFadeAlpha(1, 0.2f, false);
        Txt_Message.color = Color.white;
        Txt_Message.text = msg;
        Txt_Message.enabled = true;
        Invoke("Hide", showTime);
    }

    private void Hide()
    {
        Txt_Message.CrossFadeAlpha(0, 0.2f, false);
    }

    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        //base.OnEnter();
        Txt_Message = gameObject.transform.Find("Txt_Message").GetComponent<Text>();
        Txt_Message.enabled = false;
        uiMgr.MapingPanelByName<MessagePanel>(UIPanelType.Message, this);
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {
        //base.OnPause();
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {
        //base.OnResume();
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public override void OnExit()
    {
        //base.OnExit();
    }

    //autoEnd
}
