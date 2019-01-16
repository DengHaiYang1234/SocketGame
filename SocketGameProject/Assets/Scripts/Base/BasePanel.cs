using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BasePanel : MonoBehaviour
{
    protected UIManager uiMgr;
    protected GameFacade facade;

    protected BasePanel panel;

    public UIManager UIMgr
    {
        set { uiMgr = value; }
    }

    public GameFacade Facade
    {
        set { facade = value; }
    }

    protected void PlayClickSound()
    {
        facade.PlayNoramSound(AudioMAnager.Sound_ButtonClick,1f,false);
    }

    public virtual void InitStart()
    {
        
    }

    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public virtual void OnPause()
    {
        Tweener tween = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.2f);
        tween.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public virtual void OnResume()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }
    
    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {
        Tweener tween = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.2f);
        tween.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
