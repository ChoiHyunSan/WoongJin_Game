using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;           // ����Ƽ�� ���� ������Ʈ
using Photon.Realtime;      // ���� ���� ���� ���̺귯��
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string          _gameVersion = "1";
    public LobbyUIManager   lobbyUIManager;             

    private void Awake()
    {
    }

    private void Start()
    {
        // ���ϴ� ����ȭ �ֱ� ����
        PhotonNetwork.SendRate = 30;            // 5��/�ʷ� �޽����� ����
        PhotonNetwork.SerializationRate = 60;   // 5��/�ʷ� �޽����� ���� �� ������Ʈ

        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = _gameVersion;
        // ������ ������ ������ ���� ���� �õ� 
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ���� ���� ���� �� �ڵ� ���� -> ConnectUsingSettings() ���� ��
    public override void OnConnectedToMaster()
    {
        lobbyUIManager.SetTryConnectText();
    }

    // ������ ���� ���� ���� �� �ڵ� ���� -> ConnectUsingSettings() ���� �� 
    public override void OnDisconnected(DisconnectCause cause)
    {
        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
    public void Connect()
    {
        Debug.Log("TryToConnectGame");
        // ������ ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {
            // �� ���� ����
            lobbyUIManager.SetJoinToRoomText();
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            lobbyUIManager.SetConnectText();
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (�� ���� ����) ���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // TODO : �� �����ϴ� ���� ���� �� �˾ƺ���
        PhotonNetwork.CreateRoom("GameRoom" + PhotonNetwork.CountOfRooms, new RoomOptions { MaxPlayers = 4 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // ��� �� �����ڰ� Main ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Multi Game Room");
    }

    public string GetGameVersion()
    {
        return _gameVersion;
    }
}
