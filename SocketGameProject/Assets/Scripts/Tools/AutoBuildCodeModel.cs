﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBuildCodeModel
{
    public static string UIClass =
        @"using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class #类名#:MonoBehaviour
{
    //---------------------------------------注意以下会完成变量初始化，刷新时会更新变量--------------------------------------------

    //auto
    #成员#
    public override void InitStart()
    {
        #查找#
    }

    private void AddClicks()
    {
        #AddListener#
    }
    
    //---------------------------------------注意以上会完成变量初始化，刷新时会更新变量--------------------------------------------
    
    //defaultFcuntion
    #CallBack#

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
        base.OnPause();
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
            ";

}
