using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        //CreateUserWithEmailAndPasswordAsync ��û
        //ContinueWithOnMainThread ���
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
                   AuthError errorCode = AuthError.None;

                   var firebaseEx = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;

                   if (firebaseEx != null)
                   {
                       errorCode = (AuthError)firebaseEx.ErrorCode;
                   }

                   if (errorCode == AuthError.EmailAlreadyInUse)
                   {
                       popUpPanel.SetActive(true);
                       popUpPanel.GetComponent<PopUpPanel>().Init("This email is already in use");
                       Debug.Log("1");
                       canUse = false;
                       return;
                   }

                   popUpPanel.SetActive(true);
                   popUpPanel.GetComponent<PopUpPanel>().Init("Incorrect email address");
                   Debug.Log("2");
                   canUse = false;
                   return;
               }

               if (task.IsCompleted)
               {
                   popUpPanel.SetActive(true);
                   popUpPanel.GetComponent<PopUpPanel>().Init("You can use this Email");
                   Debug.Log("3");
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
