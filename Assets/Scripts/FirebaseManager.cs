using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager instance;
    public static FirebaseManager Instance { get { return instance; } }


    private static FirebaseApp app;
    public static FirebaseApp App { get { return app; } }

    private static FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return auth; } }

    private static FirebaseDatabase database;
    public static FirebaseDatabase Database { get { return database; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //FirebaseApp���� �������ϴ���?, CheckAndFixDependenciesAsync ��û,  ContinueWithOnMainThread ���({}) -> �񵿱�
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task => 
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Debug.Log("���̾� ���̽� ������ ��� ����");
                    app = FirebaseApp.DefaultInstance;
                    auth = FirebaseAuth.DefaultInstance;
                    database = FirebaseDatabase.DefaultInstance;
                }
                else
                {
                    Debug.LogError($"{dependencyStatus}�� ���� Firebase ���� �Ұ�");
                    app = null;
                    auth = null;
                    database = null;
                }
            });
    }

}
