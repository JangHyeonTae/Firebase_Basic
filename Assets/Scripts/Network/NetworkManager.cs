using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private TextMeshProUGUI currentState;

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private Button createRoom;


    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        createRoom.onClick.AddListener(CreateRoom);
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

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //방입장시 이벤트
        Debug.Log("로비에 입장");
    }

    private void CreateRoom()
    {
        //클릭했을때 이벤트
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
