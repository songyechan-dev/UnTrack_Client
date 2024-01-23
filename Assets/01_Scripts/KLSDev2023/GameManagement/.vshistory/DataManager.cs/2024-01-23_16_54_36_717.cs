using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager : MonoBehaviour
{
    private static string userID;

    public static string GetUserID()
    {
        return userID;
    }

    public static void SetUserID(string _userID)
    {
        this.userID = _userID;
    }
}
