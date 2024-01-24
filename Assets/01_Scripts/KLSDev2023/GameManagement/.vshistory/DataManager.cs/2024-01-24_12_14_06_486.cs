using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private static string userID;
    private static int needPeopleCount = 0;

    public static string GetUserID()
    {
        return userID;
    }

    public static void SetUserID(string _userID)
    {
        userID = _userID;
    }

    public static int GetNeedPeopleCount() 
    {
        return needPeopleCount;
    }

    public void SetNeedPeopleCount(int count)
    {
        needPeopleCount = count;
    }


}
