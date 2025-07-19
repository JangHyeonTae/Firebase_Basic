using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private TextMeshProUGUI currentState;

    [Header("�κ� ������ - �α���")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button leaveLobby;

    [Header("�����")]
    [SerializeField] private Button createRoom;
    [SerializeField] private TMP_InputField roomnameInput;
    [SerializeField] private TMP_InputField roomMaxPlayerInput;

    
    

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        createRoom.onClick.AddListener(CreateRoom);
        leaveLobby.onClick.AddListener(LeaveLobby);
    }

    

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("���� ����");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("�����ͼ��� ����");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("���� ���� ����");
    }

    public void LeaveLobby()
    {
        PhotonNetwork.Disconnect();
        loginPanel.SetActive(true);
        FirebaseManager.Auth.SignOut();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //������� �̺�Ʈ
        Debug.Log("�κ� ����");
    }

    private void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        int maxPlayer = 0;
        bool succes = int.TryParse(roomMaxPlayerInput.text, out maxPlayer);
        if (succes && maxPlayer < 5)
        {
            options.MaxPlayers = maxPlayer;
            PhotonNetwork.CreateRoom(roomnameInput.text, options);
        }
        else
        {
            maxPlayer = 0;
            Debug.Log("���� ���� �ο� �ʰ�");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("�� ����");
    }

    private void Upate()
    {
        currentState.text = $"currentState : {PhotonNetwork.NetworkClientState}";
    }
}
