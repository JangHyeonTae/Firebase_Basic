using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NickNamePanel : MonoBehaviour
{
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject loginPanel;

    [SerializeField] TMP_InputField nicknameInput;

    [SerializeField] Button backButton;
    [SerializeField] Button confirmButton;

    private void Awake()
    {
        backButton.onClick.AddListener(Back);
        confirmButton.onClick.AddListener(Confirm);
    }

    private void Confirm()
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = nicknameInput.text;

        FirebaseUser user = FirebaseManager.Auth.CurrentUser;

        user.UpdateUserProfileAsync(profile)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("���� �г��� ���� ���");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError($"���� �г��� ���� ���� ���� : {task.Exception}");
                    return;
                }

                
                lobbyPanel.SetActive(true);
                gameObject.SetActive(false);
            });
    }

    private void Back()
    {
        FirebaseManager.Auth.SignOut();
        loginPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
