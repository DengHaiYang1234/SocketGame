using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using Common;

public class LoginPanel : BasePanel
{
    //auto
    private InputField Input_UserName = null;
    private Text Txt_UserNamePlaceholder = null;
    private Text Txt_UserNameText = null;
    private InputField Input_PassWord = null;
    private Text Txt_PassWordPlaceholder = null;
    private Text Txt_PassWordText = null;
    private Button Btn_Login = null;
    private Button Btn_Register = null;
    private Button Btn_Close = null;
    private LoginRequest loginRequest;

    public override void InitStart()
    {
        Input_UserName = gameObject.transform.Find("UserName/Input_UserName").GetComponent<InputField>();
        Txt_UserNamePlaceholder = gameObject.transform.Find("UserName/Input_UserName/Txt_UserNamePlaceholder").GetComponent<Text>();
        Txt_UserNameText = gameObject.transform.Find("UserName/Input_UserName/Txt_UserNameText").GetComponent<Text>();
        Input_PassWord = gameObject.transform.Find("PassWord/Input_PassWord").GetComponent<InputField>();
        Txt_PassWordPlaceholder = gameObject.transform.Find("PassWord/Input_PassWord/Txt_PassWordPlaceholder").GetComponent<Text>();
        Txt_PassWordText = gameObject.transform.Find("PassWord/Input_PassWord/Txt_PassWordText").GetComponent<Text>();
        Btn_Login = gameObject.transform.Find("Btn_Login").GetComponent<Button>();
        Btn_Register = gameObject.transform.Find("Btn_Register").GetComponent<Button>();
        Btn_Close = gameObject.transform.Find("Btn_Close").GetComponent<Button>();
        loginRequest = GetComponent<LoginRequest>();

        Btn_Login.onClick.AddListener(OnLoginClick);
        Btn_Register.onClick.AddListener(OnRegisterClick);
        Btn_Close.onClick.AddListener(OnClickClose);
    }

    private void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(Input_UserName.text))
        {
            msg += "用户名不能为空!";
        }
        if (string.IsNullOrEmpty(Input_PassWord.text))
        {
            msg += "密码不能为空!";
        }
        if (msg != "")
        {
            uiMgr.ShowMessage(msg);
        }

        loginRequest.SendRequest(Input_UserName.text, Input_PassWord.text);
    }

    public void OnLoginReponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMgr.PushPanelSync(UIPanelType.Room);
        }
        else
        {
            uiMgr.ShowMessageSync("用户名或密码错误，请重新输入!!!!");

        }
    }

    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMgr.PushPanel(UIPanelType.Register);
    }

    private void OnClickClose()
    {
        PlayClickSound();

        uiMgr.PopPanel();

        //Tweener tween = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.2f);
        //tween.OnComplete(() =>
        //{
        //    uiMgr.PopPanel();
        //});
    }


    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        //gameObject.SetActive(true);
        //transform.localScale = Vector3.zero;
        //transform.DOScale(1, 0.2f);
        //transform.localPosition = new Vector3(1000, 0, 0);
        //transform.DOLocalMove(Vector3.zero, 0.2f);
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
        //Tweener tween = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.2f);
        //tween.OnComplete(() =>
        //{
        //    uiMgr.PopPanel();
        //});
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {
        base.OnResume();
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
    }

    //autoEnd
}
