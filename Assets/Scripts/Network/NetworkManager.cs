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

    [Header("로비 떠나기 - 로그인")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button leaveLobby;

    [Header("방생성")]
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
        Debug.Log("서버 연결");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터서버 연결");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("서버 연결 끊김");
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
        //방입장시 이벤트
        Debug.Log("로비에 입장");
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
            Debug.Log("게임 제한 인원 초과");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("룸 입장");
    }

    private void Upate()
    {
        currentState.text = $"currentState : {PhotonNetwork.NetworkClientState}";
    }
}
