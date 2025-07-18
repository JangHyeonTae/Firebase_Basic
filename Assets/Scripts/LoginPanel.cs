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
                    Debug.LogError("로그인 취소");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"{task.Exception}로 인한 로그인 실패");
                    return;
                }

                Debug.Log("로그인 성공!");
                //FirebaseManager.Auth.CurrentUser 로그인이 완료됐을경우 현재 유저의 정보 이걸로 가져다 쓸 수잇음
                //FirebaseUser에 유저에 대한 정보 있음
                
                FirebaseUser user = task.Result.User;
                // 1. 이미 이메일 인증을 마친 유저인 경우 로비로
                if (user.IsEmailVerified)
                {
                    // 1-1 아직 닉네임 설정을 하지 않은 경우
                    if(user.DisplayName == "")
                    {
                        nicknamePanel.SetActive(true);
                        gameObject.SetActive(false);
                    }
                    // 1-2 닉네임 설정도 완료한 경우
                    else
                    {
                        lobbyPanel.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
                // 2. 이메일 인증이 되지 않은 유저인 경우 이메일 인증을 대기
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
                    Debug.LogError("패스워드 재설정 이메일 전송 취소");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"패스워드 재설정 이메일 전송 실패 원인 : {task.Exception}");
                    return;
                }

                Debug.Log("재설정 이메일 보냄");
            });
    }

}
