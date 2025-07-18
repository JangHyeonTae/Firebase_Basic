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

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //������� �̺�Ʈ
        Debug.Log("�κ� ����");
    }

    private void CreateRoom()
    {
        //Ŭ�������� �̺�Ʈ
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
