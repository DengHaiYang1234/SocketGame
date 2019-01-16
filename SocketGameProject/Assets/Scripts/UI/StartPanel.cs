using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;


public class StartPanel : BasePanel
{
    //auto
    private Button Btn_Login = null;

    public override void InitStart()
    {
        Btn_Login = gameObject.transform.Find("Btn_Login").GetComponent<Button>();
        Btn_Login.onClick.AddListener(OnLoginClick);
    }
    
    private void OnLoginClick()
    {
        PlayClickSound();
        uiMgr.PushPanel(UIPanelType.Login);
    }


    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {
        transform.DOScale(0, 0.4f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
        transform.DOScale(1, 0.4f);
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
