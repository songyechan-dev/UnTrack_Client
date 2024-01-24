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



}
