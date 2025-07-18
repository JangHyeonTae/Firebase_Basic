using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    [SerializeField] GameObject lobbyPanel;

    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] TMP_InputField passConfirmInput;

    [SerializeField] TextMeshProUGUI emailText;
    [SerializeField] TextMeshProUGUI userIdText;

    [SerializeField] Button nicknameConfirmButton;
    [SerializeField] Button passConfirmButton;
    [SerializeField] Button backButton;
    private void Awake()
    {
        nicknameConfirmButton.onClick.AddListener(NicknameConfirm);
        passConfirmButton.onClick.AddListener(PassConfirm);
        backButton.onClick.AddListener(Back);
    }

    private void OnEnable()
    {
        FirebaseUser user = FirebaseManager.Auth.CurrentUser;
        emailText.text = user.Email;
        userIdText.text = user.UserId;
        nameInput.text = user.DisplayName;
    }

    private void NicknameConfirm()
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = nameInput.text;

        FirebaseUser user = FirebaseManager.Auth.CurrentUser;
        user.UpdateUserProfileAsync(profile)
            .ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled)
                {
                    Debug.LogError($"닉네임 변경 취소");
                    return;
                }

                if(task.IsFaulted)
                {
                    Debug.LogError($"{task.Exception} 때문에 닉네임 변경 실패");
                    return;
                }

                Debug.Log("닉네임 변경 성공");
            });
            
    }

    private void PassConfirm()
    {
        if (passInput.text != passConfirmInput.text)
        {
            Debug.LogError("비밀번호가 일치하지 않음");
            return;
        }

        UserProfile profile = new UserProfile();

        FirebaseUser user = FirebaseManager.Auth.CurrentUser;
        user.UpdatePasswordAsync(passInput.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError($"비밀번호 변경 취소");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError($"{task.Exception} 때문에 비밀번호 변경 실패");
                    return;
                }

                Debug.Log("비밀번호 변경 성공");
            });
    }

    private void Back()
    {
        lobbyPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
