using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string UserName { get; private set; }
    public int TotalCount { get;  set; }
    public int WinCount { get;  set; }
    public int Id { get; set; }


    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        Id = int.Parse(strs[0]);
        UserName = strs[1];
        TotalCount = int.Parse(strs[2]);
        WinCount = int.Parse(strs[3]);
    }

    public UserData(string userName,int totalCount,int winCount)
    {
        UserName = userName;
        TotalCount = totalCount;
        WinCount = winCount;
    }

    public UserData(int id, string userName, int totalCount, int winCount)
    {
        Id = id;
        UserName = userName;
        TotalCount = totalCount;
        WinCount = winCount;
    }
}
