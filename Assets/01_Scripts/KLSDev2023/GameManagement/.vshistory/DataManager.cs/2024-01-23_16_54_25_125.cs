using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager : MonoBehaviour
{
    private string userID;

    public string GetUserID()
    {
        return userID;
    }

    public void SetUserID(string _userID)
    {
        this.userID = _userID;
    }
}
