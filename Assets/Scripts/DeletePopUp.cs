using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePopUp : MonoBehaviour
{
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject loginPanel;

    [SerializeField] private Button deleteUserButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(Back);
        deleteUserButton.onClick.AddListener(DeleteUser);
    }

    private void Back()
    {
        lobbyPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void DeleteUser()
    {
        FirebaseUser user = FirebaseManager.Auth.CurrentUser;

        user.DeleteAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("유저 삭제 취소됨");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"유저 삭제 실패 원인 : {task.Exception}");
                }

                Debug.Log("삭제 됨");
                FirebaseManager.Auth.SignOut();
                loginPanel.SetActive(true);
                gameObject.SetActive(false);
            });
    }
}
