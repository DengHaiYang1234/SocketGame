using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 抽象类是不能实例化。且抽象函数只能定义在抽象类中，并且抽象方法是不可实现的。虚函数是可实现的
/// </summary>
public class BaseManager
{
    protected GameFacade gameFacade;

    public BaseManager(GameFacade facade)
    {
        this.gameFacade = facade;
    }
    public virtual void OnInit()
    {
    }

    public virtual void OnDestroy()
    { }

    public virtual void Update()
    {
        
    }


}
