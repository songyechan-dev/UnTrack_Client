using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private static string userID;
    private static int needReadyUserCount = 0;

    public static string GetUserID()
    {
        return userID;
    }

    public static void SetUserID(string _userID)
    {
        userID = _userID;
    }

    public static int GetNeedReadyUserCount() 
    {
        return needReadyUserCount;
    }

    public static void SetNeedReadyUserCount(int _count)
    {
        needReadyUserCount = _count;
    }


}