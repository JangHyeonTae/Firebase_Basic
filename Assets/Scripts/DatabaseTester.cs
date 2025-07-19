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

        #region json, 그냥 값 세팅
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        
        // 첫 로그인시 전부 세팅
        userInfo.SetRawJsonValueAsync(json);
        
        // 로그인 후 데이터를 다시 세팅할 때
        DatabaseReference levelRef = userInfo.Child("Level");
        levelRef.SetValueAsync(3);
        #endregion

        #region update

        Dictionary<string, object> dic = new();
        dic["level"] = 10;
        dic["speed"] = 3.5;


        userInfo.Child("UserData").SetValueAsync(dic);
        // 해당 정보만 바꾸고싶다
        userInfo.UpdateChildrenAsync(dic);

        #endregion

        #region 데이터 지우기
        userInfo.Child("clear").SetValueAsync(null);
        userInfo.Child("clear").RemoveValueAsync();
        #endregion

        #region 트랜잭션

        // data(원본데이터) => 읽었을때 데이터
        leaderBoardRef.RunTransaction(data =>
        {
            List<object> leaders = data.Value as List<object>;

            if (leaders == null)
            {
                return TransactionResult.Abort();
            }

            //수정했다가 안해도 되는상황일경우 - ex) 아이템을 살려했는데 아이템 자체가 사라짐(매물이 없어짐)
            //return TransactionResult.Abort(); // 변경취소

            // data => 수정된 데이터  , 여기에서는 data에 데이터 추가(data에 변화량을 넣어주는거)

            return TransactionResult.Success(data);
        });


        #endregion

        #region 데이터 가져오기(하나하나 가져오기)

        userInfo.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("값 가져오기 취소됨");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("값 가져오기 실패");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                Debug.Log($"snapshot child count : {snapshot.ChildrenCount}");

                bool clear = (bool)snapshot.Child("clear").Value;
                Debug.Log($"clear : {clear}");

                long level = (long)snapshot.Child("level").Value; // int안됨 long으로 받아와야함(firebase는 int형식 long으로 float형식 double로)
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

        #region Json형태로 가져오기

        userInfo.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("값 가져오기 취소됨");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("값 가져오기 실패");
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