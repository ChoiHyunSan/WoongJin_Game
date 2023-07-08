using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using Photon.Pun;           // ����Ƽ�� ���� ������Ʈ
using Photon.Realtime;      // ���� ���� ���� ���̺귯��
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    public TextMeshProUGUI connectionInfoText;     // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    public Button joinButton;           // �� ���� ��ư

    // ���� ����� ���ÿ� ������ �� ���� �õ�
    private void Start()
    {
        // ���ϴ� ����ȭ �ֱ� ����
        PhotonNetwork.SendRate = 30; // 5��/�ʷ� �޽����� ����
        PhotonNetwork.SerializationRate = 60; // 5��/�ʷ� �޽����� ���� �� ������Ʈ

        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ���� ���� �õ� 
        PhotonNetwork.ConnectUsingSettings();

        // �� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        // ���� �õ������� �ؽ�Ʈ�� ǥ��
        connectionInfoText.text = "Connecting...";
    }

    // ������ ���� ���� ���� �� �ڵ� ���� -> ConnectUsingSettings() ���� ��
    public override void OnConnectedToMaster()
    {
        // �� ���� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        // ���� ���� ǥ��
        connectionInfoText.text = "Online : Master Server";
    }

    // ������ ���� ���� ���� �� �ڵ� ���� -> ConnectUsingSettings() ���� �� 
    // ���� ���� ������ DisconnectCause Ÿ������ ���´�.
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        // ���� ���� ǥ��
        connectionInfoText.text = "ReConnecting...";

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
    public void Connect()
    {
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ������ ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {
            // �� ���� ����
            connectionInfoText.text = "Join To Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // ������ ������ ���� ���� �ƴ϶�� ������ ������ ���� �õ�
            connectionInfoText.text = "ReConnecting...";
            // ������ �������� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (�� ���� ����) ���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� ���� ǥ��
        connectionInfoText.text = "Create New Room";
        // �ִ� 4���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom("GameRoom" + PhotonNetwork.CountOfRooms, new RoomOptions { MaxPlayers = 4 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // ���� ���� ǥ��
        connectionInfoText.text = "Success";

        // ��� �� �����ڰ� Main ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Game Room");
    }
}
