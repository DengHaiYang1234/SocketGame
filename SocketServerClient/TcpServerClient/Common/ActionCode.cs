using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 用哪个方法执行
    /// </summary>
    public enum ActionCode
    {
        Node,
        Login,
        Register,
        ListRoom,
        CreatRoom, 
        JoinRoom,
        UpdateRoom,
        QuitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
        UpdateResult,
        QuitBattle,
    }
}
