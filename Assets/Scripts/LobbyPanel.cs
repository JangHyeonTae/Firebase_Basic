using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject editPanel;
    [SerializeField] private GameObject deletePopUP;
    
                     
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button editProfileButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button findRoomButton;
                     
    [SerializeField] private TextMeshProUGUI emailText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI uidText;

    void Awake()
    {
        logoutButton.onClick.AddListener(Logout);
        editProfileButton.onClick.AddListener(EditProfile);
        deleteButton.onClick.AddListener(ShowDelete);
        findRoomButton.onClick.AddListener(FindRoom);
    }

    private void OnEnable()
    {
        FirebaseUser user = FirebaseManager.Auth.CurrentUser;
        emailText.text  = user.Email;
        nameText.text   = user.DisplayName;
        uidText.text    = user.UserId;

        //foreach (IUserInfo info in user.ProviderData)
        //{
        //    //다양한 제공업체 - 여러개 가입했을 때
        //}
    }

    private void FindRoom()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Logout()
    {
        FirebaseManager.Auth.SignOut();
        loginPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void EditProfile()
    {
        editPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ShowDelete()
    {
        deletePopUP.SetActive(true);
        gameObject.SetActive(false);
    }

    
}
