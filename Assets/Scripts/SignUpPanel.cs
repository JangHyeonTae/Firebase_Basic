using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject popUpPanel;

    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] TMP_InputField passConfirmInput;

    [SerializeField] Button signUpButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button popUpButton;

    private bool canUse;
    private void Awake()
    {
        signUpButton.onClick.AddListener(SignUp);
        cancelButton.onClick.AddListener(Cancel);
        popUpButton.onClick.AddListener(CheckEmail);
    }

    private void OnEnable()
    {
        idInput.text = "";
        passInput.text = "";
        passConfirmInput.text = "";
    }

    private void SignUp()
    {
        if (passInput.text != passConfirmInput.text)
        {
            Debug.LogError("�н����尡 ��ġ���� �ʽ��ϴ�");
            return;
        }

        FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(idInput.text, passInput.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("�̸��� ���� ��� ��");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError($"�̸��� ���� ������ : {task.Exception}");
                    return;
                }

                //������ ����(������ �ƴ���, ~~�ƴ���)
                //AuthResult result = task.Result;
                //�����ϰ� ���������
                //task.IsCompletedSuccessfully
                //
                Debug.Log("�̸��� ���� ����");
                loginPanel.SetActive(true);
                gameObject.SetActive(false);
            });
    }

    private void CheckEmail()
    {
        FirebaseAuth.DefaultInstance.FetchProvidersForEmailAsync(idInput.text)
           .ContinueWithOnMainThread(task =>
           {

               if (task.IsCanceled || task.IsFaulted)
               {
                   popUpPanel.SetActive(true);
                   popUpPanel.GetComponent<PopUpPanel>().Init("Incorrect email address");
                   canUse = false;
                   return;
               }

               var provider = task.Result;
               if (provider != null && provider.Count() > 0)
               {
                   popUpPanel.SetActive(true);
                   popUpPanel.GetComponent<PopUpPanel>().Init("This email is already in use");
                   canUse = false;
                   return;
               }
               else
               {
                   popUpPanel.SetActive(true);
                   popUpPanel.GetComponent<PopUpPanel>().Init("You can use this Email");
                   signUpButton.interactable = true;
                   canUse = true;
                   return;
               }
           });
    }

    private void Cancel()
    {
        loginPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
