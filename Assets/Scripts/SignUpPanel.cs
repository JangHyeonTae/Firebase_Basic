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
            Debug.LogError("패스워드가 일치하지 않습니다");
            return;
        }

        //CreateUserWithEmailAndPasswordAsync 신청
        //ContinueWithOnMainThread 결과
        FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(idInput.text, passInput.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("이메일 가입 취소 됨");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError($"이메일 가입 실패함 : {task.Exception}");
                    return;
                }

                //유저의 정보(인증이 됐는지, ~~됐는지)
                //AuthResult result = task.Result;
                //성공하고 끝났을경우
                //task.IsCompletedSuccessfully
                //
                Debug.Log("이메일 가입 성공");
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
