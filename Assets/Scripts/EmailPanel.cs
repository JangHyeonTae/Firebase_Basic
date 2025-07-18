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
                    Debug.LogError("���� �̸��� ������ ��� �Ǿ����ϴ�");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError($"���� �̸��� ���� ���� ���� :{task.Exception}");
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
                Debug.Log("���� �Ϸ�");
                nicknamePanel.SetActive(true);
                gameObject.SetActive(false);
                StopCoroutine(emailVerificationRoutine);
            }
            else
            {
                Debug.Log("���� �����");
            }
        }
    }
}
