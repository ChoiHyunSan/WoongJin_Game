using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Linq;
using UnityEngine.SocialPlatforms;

public class LobbyUIManager : MonoBehaviourPunCallbacks
{
    [Header("�κ� �ؽ�Ʈ ����Ʈ")]
    [SerializeField]
    private string          ConnectingText       = "Connecting...";
    [SerializeField]
    private string          TryConnectText       = "Touch To Start";
    [SerializeField]
    private string          JoinToRoomText       = "Join To Room...";

    [Header("�κ� UI ���")]
    // UI ��� ����
    public Button           joinButton;                 // �� ���� ��ư
    public TextMeshProUGUI  connectionInfoText;         // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    
    public Button           joinToMultiButton;                 // �� ���� ��ư
    public Button           joinToSingleButton;                // �� ���� ��ư

    [Header("�̱� ���� �г� ����")]
    public GameObject       singleGamePanel;

    [Header("���� �г� ����")]
    public GameObject       settingPanel;

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

    public void ConnectGame()
    {
    
    }

    public void OpenSingleGamePanel()
    {
        singleGamePanel.SetActive(true);
    }

    public void CloseSingleGamePanel()
    {
        singleGamePanel.SetActive (false);
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    public void ConnectMultiGame()
    {
        Debug.Log("ConnectGame");
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        lobbyManager.Connect();
    }

    public void ConnectSingleGame()
    {
        SceneManager.LoadScene(SStageInfo.StageName);
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
        joinButton.interactable = true;
        ChangeText(TryConnectText);
    }

    void ChangeText(string text)
    {
        connectionInfoText.text = text;
    }

    public void SetStageNumber(string stage)
    {
        // TODO : �������� �̸��� �Ѱ��ش�.
        SStageInfo.StageName = stage;
    }

}
