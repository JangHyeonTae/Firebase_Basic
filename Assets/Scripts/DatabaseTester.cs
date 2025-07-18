using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseTester : MonoBehaviour
{
    [SerializeField] Button testButton;

    [SerializeField] PlayerData data;
    private void Awake()
    {
        testButton.onClick.AddListener(Test);
    }


    private void Test()
    {
        FirebaseUser user = FirebaseManager.Auth.CurrentUser;

        DatabaseReference root = FirebaseManager.Database.RootReference;
        DatabaseReference userInfo = root.Child("UserData").Child($"{user.UserId}");

        string json = JsonUtility.ToJson(data);
        Debug.Log(json);

        userInfo.SetRawJsonValueAsync(json);
    }
}


[Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public float speed;
    public bool clear;
    public List<string> skills;
}