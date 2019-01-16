using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using Common;


public class GamePanel : BasePanel
{
    //---------------------------------------注意以下会完成变量初始化，刷新时会更新变量--------------------------------------------

    //auto
    private Text Txt_Timer = null;
    private Button Btn_Success = null;
    private Button Btn_Fail = null;
    private Button Btn_ExitButton = null;

    private QuitBattleRequest quitBattleRequest;

    public override void InitStart()
    {
        Txt_Timer = gameObject.transform.Find("Txt_Timer").GetComponent<Text>();
        Btn_Success = gameObject.transform.Find("Btn_Success").GetComponent<Button>();
        Btn_Fail = gameObject.transform.Find("Btn_Fail").GetComponent<Button>();
        Btn_ExitButton = gameObject.transform.Find("Btn_ExitButton").GetComponent<Button>();
        quitBattleRequest = gameObject.GetComponent<QuitBattleRequest>();
        AddClicks();
    }

    private void AddClicks()
    {
        Btn_Success.onClick.AddListener(OnSuccessClick);
        Btn_Fail.onClick.AddListener(OnFailClick);
        Btn_ExitButton.onClick.AddListener(OnExitButtonClick);
    }

    //---------------------------------------注意以上会完成变量初始化，刷新时会更新变量--------------------------------------------

    //defaultFcuntion


    private void OnExitButtonClick()
    {
        quitBattleRequest.SendRequest();
    }


    public void OnExitResponse()
    {
        uiMgr.PopPanel();
        uiMgr.PopPanel();
        facade.GameOver();

    }

    private int time = -1;

    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }
    }

    private void OnSuccessClick()
    {
        uiMgr.PopPanel();
        uiMgr.PopPanel();
        Btn_Success.gameObject.SetActive(false);
        facade.GameOver();
    }
    private void OnFailClick()
    {
        uiMgr.PopPanel();
        uiMgr.PopPanel();
        Btn_Fail.gameObject.SetActive(false);
        facade.GameOver();
    }

    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        Btn_ExitButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {

    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public override void OnExit()
    {
        Btn_ExitButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    //autoEnd

    public void ShowTimeSync(int time)
    {
        this.time = time;
    }

    public void ShowTime(int time)
    {
        if (time == 1)
        {
            Btn_ExitButton.gameObject.SetActive(true);
        }
        Txt_Timer.gameObject.SetActive(true);
        Txt_Timer.text = time.ToString();
        Txt_Timer.transform.localScale = Vector3.one;
        Color tempColor = Txt_Timer.color;
        tempColor.a = 1;
        Txt_Timer.color = tempColor;
        Txt_Timer.transform.DOScale(2, 0.5f).SetDelay(0.3f);
        Txt_Timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => Txt_Timer.gameObject.SetActive(false));
        facade.PlayNoramSound(AudioMAnager.Sound_Timer, 1f, false);
    }



    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button tempBtn = null;
        switch (returnCode)
        {
            case ReturnCode.Success:
                tempBtn = Btn_Success;

                break;
            case ReturnCode.Fail:
                tempBtn = Btn_Fail;
                break;
        }

        tempBtn.gameObject.SetActive(true);
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.5f);
    }

}
