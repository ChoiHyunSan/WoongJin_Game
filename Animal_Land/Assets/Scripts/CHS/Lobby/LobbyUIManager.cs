using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LobbyUIManager : MonoBehaviourPunCallbacks
{
    [Header("�κ� �ؽ�Ʈ ����Ʈ")]
    [SerializeField]
    private string          ConnectingText       = "Connecting...";
    [SerializeField]
    private string          TryConnectText       = "Touch To Start";
    [SerializeField]
    private string          JoinToRoomText       = "Join To Room...";

    [Header("UI ���")]
    // UI ��� ����
    public Button           joinToMultiButton;                 // �� ���� ��ư
    public Button           joinToSingleButton;                // �� ���� ��ư
    public TextMeshProUGUI  connectionInfoText;                // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ

    public GameObject       singleGamePanel;

    [Header("�Ŵ��� Ŭ����")]
    public LobbyManager     lobbyManager;

    // Start is called before the first frame update
    void Start()
    {
        SetConnectText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectMultiGame()
    {
        Debug.Log("ConnectGame");
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        joinToMultiButton.interactable = false;
        lobbyManager.Connect();
    }

    public void OpenSingleGamePanel()
    {
        singleGamePanel.SetActive(true);
    }

    public void CloseSingleGamePanel()
    {
        singleGamePanel.SetActive(false);
    }

    public void ConnectSingleGame()
    {
        // TODO : ���� ���� ������ �̵��ϴ� ���� �������� ���� �̵��� �� �ְ� ����
        SceneManager.LoadScene("Single Game Room");
    }

    public void SetJoinToRoomText()
    {
        ChangeText(JoinToRoomText);
    }

    public void SetConnectText()
    {
        Debug.Log("SetConnectText");
        ChangeText(ConnectingText);
    }
    public void SetTryConnectText()
    {
        joinToMultiButton.interactable = true;
        ChangeText(TryConnectText);
    }

    void ChangeText(string text)
    {
        connectionInfoText.text = text;
    }

}
