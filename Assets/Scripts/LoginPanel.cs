using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Extensions;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] GameObject signUpPanel;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject emailPanel;
    [SerializeField] GameObject nicknamePanel;

    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField passInput;

    [SerializeField] Button signUpButton;
    [SerializeField] Button loginButton;
    [SerializeField] Button resetPassButton;

    void Awake()
    {
        signUpButton.onClick.AddListener(SignUp);
        loginButton.onClick.AddListener(Login);
        resetPassButton.onClick.AddListener(ResetPass);
    }

    private void SignUp()
    {
        signUpPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Login()
    {
        FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(idInput.text, passInput.text)
            .ContinueWithOnMainThread(task => 
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("�α��� ���");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"{task.Exception}�� ���� �α��� ����");
                    return;
                }

                Debug.Log("�α��� ����!");
                //FirebaseManager.Auth.CurrentUser �α����� �Ϸ������� ���� ������ ���� �̰ɷ� ������ �� ������
                //FirebaseUser�� ������ ���� ���� ����
                
                FirebaseUser user = task.Result.User;
                // 1. �̹� �̸��� ������ ��ģ ������ ��� �κ��
                if (user.IsEmailVerified)
                {
                    // 1-1 ���� �г��� ������ ���� ���� ���
                    if(user.DisplayName == "")
                    {
                        nicknamePanel.SetActive(true);
                        gameObject.SetActive(false);
                    }
                    // 1-2 �г��� ������ �Ϸ��� ���
                    else
                    {
                        lobbyPanel.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
                // 2. �̸��� ������ ���� ���� ������ ��� �̸��� ������ ���
                else
                {
                    emailPanel.SetActive(true);
                    gameObject.SetActive(false);
                }
            });
    }

    private void ResetPass()
    {
        FirebaseUser user = FirebaseManager.Auth.CurrentUser;

        FirebaseManager.Auth.SendPasswordResetEmailAsync(idInput.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("�н����� �缳�� �̸��� ���� ���");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"�н����� �缳�� �̸��� ���� ���� ���� : {task.Exception}");
                    return;
                }

                Debug.Log("�缳�� �̸��� ����");
            });
    }

}
