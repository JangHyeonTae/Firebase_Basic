using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailPanel : MonoBehaviour
{
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject nicknamePanel;

    [SerializeField] Button backButton;

    Coroutine emailVerificationRoutine;
    private void Awake()
    {
        backButton.onClick.AddListener(Back);
    }
    private void OnEnable()
    {
        FirebaseManager.Auth.CurrentUser.SendEmailVerificationAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("인증 이메일 전송이 취소 되었습니다");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError($"인증 이메일 전송 실패 원인 :{task.Exception}");
                    return;
                }

                emailVerificationRoutine = StartCoroutine(EmailVerificationRoutine());
            });
    }
    private void Back()
    {
        FirebaseManager.Auth.SignOut();
        loginPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator EmailVerificationRoutine()
    {
        FirebaseUser user = FirebaseManager.Auth.CurrentUser;
        WaitForSeconds delay = new WaitForSeconds(2f);

        while (true)
        {
            yield return delay;

            user.ReloadAsync();

            if (user.IsEmailVerified)
            {
                Debug.Log("인증 완료");
                nicknamePanel.SetActive(true);
                gameObject.SetActive(false);
                StopCoroutine(emailVerificationRoutine);
            }
            else
            {
                Debug.Log("인증 대기중");
            }
        }
    }
}
