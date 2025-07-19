using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
        DatabaseReference leaderBoardRef = root.Child("LeaderBoard");

        #region json, �׳� �� ����
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        
        // ù �α��ν� ���� ����
        userInfo.SetRawJsonValueAsync(json);
        
        // �α��� �� �����͸� �ٽ� ������ ��
        DatabaseReference levelRef = userInfo.Child("Level");
        levelRef.SetValueAsync(3);
        #endregion

        #region update

        Dictionary<string, object> dic = new();
        dic["level"] = 10;
        dic["speed"] = 3.5;


        userInfo.Child("UserData").SetValueAsync(dic);
        // �ش� ������ �ٲٰ�ʹ�
        userInfo.UpdateChildrenAsync(dic);

        #endregion

        #region ������ �����
        userInfo.Child("clear").SetValueAsync(null);
        userInfo.Child("clear").RemoveValueAsync();
        #endregion

        #region Ʈ�����

        // data(����������) => �о����� ������
        leaderBoardRef.RunTransaction(data =>
        {
            List<object> leaders = data.Value as List<object>;

            if (leaders == null)
            {
                return TransactionResult.Abort();
            }

            //�����ߴٰ� ���ص� �Ǵ»�Ȳ�ϰ�� - ex) �������� ����ߴµ� ������ ��ü�� �����(�Ź��� ������)
            //return TransactionResult.Abort(); // �������

            // data => ������ ������  , ���⿡���� data�� ������ �߰�(data�� ��ȭ���� �־��ִ°�)

            return TransactionResult.Success(data);
        });


        #endregion

        #region ������ ��������(�ϳ��ϳ� ��������)

        userInfo.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("�� �������� ��ҵ�");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("�� �������� ����");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                Debug.Log($"snapshot child count : {snapshot.ChildrenCount}");

                bool clear = (bool)snapshot.Child("clear").Value;
                Debug.Log($"clear : {clear}");

                long level = (long)snapshot.Child("level").Value; // int�ȵ� long���� �޾ƿ;���(firebase�� int���� long���� float���� double��)
                Debug.Log($"level : {level}");

                string name = (string)snapshot.Child("name").Value;
                Debug.Log($"name : {name}");

                float speed = (float)(double)snapshot.Child("speed").Value;
                Debug.Log($"speed : {speed}");

                List<string> skill = (List<string>)snapshot.Child("skill").Value;
                for (int i = 0; i < skill.Count; i++)
                {
                    Debug.Log($"skill : {skill[i]}");
                }
            });

        #endregion

        #region Json���·� ��������

        userInfo.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("�� �������� ��ҵ�");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("�� �������� ����");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                Debug.Log($"snapshot child count : {snapshot.ChildrenCount}");

                string json = snapshot.GetRawJsonValue();
                Debug.Log(json);

                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

                Debug.Log($"clear : {playerData.clear}");
                Debug.Log($"name : {playerData.clear}");
                Debug.Log($"speed : {playerData.clear}");
                Debug.Log($"level : {playerData.clear}");
                for (int i = 0; i < playerData.skills.Count; i++)
                {
                    Debug.Log($"skills : {playerData.skills}");
                }
            });
        }

        #endregion
 }

//RunTransaction
[Serializable]
public class LeaderBoardData
{
    public List<Ranking> rander;

    [Serializable]
    public class Ranking
    {
        public string name;
        public int score;
    }
}

//SetRawJsonValueAsync
[Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public float speed;
    public bool clear;
    public List<string> skills;
}