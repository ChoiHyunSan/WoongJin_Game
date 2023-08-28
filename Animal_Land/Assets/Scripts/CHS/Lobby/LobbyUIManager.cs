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
    public Button           joinButton;                         // �� ���� ��ư
    public TextMeshProUGUI  connectionInfoText;                 // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    
    public Button           joinToMultiButton;                  // �� ���� ��ư
    public Button           joinToSingleButton;                 // �� ���� ��ư

    [Header("�̱� ���� �г� ����")]
    public GameObject       singleGamePanel;

    [Header("���� �г� ����")]
    public GameObject       settingPanel;
    public GameObject       exitPanel;

    [Header("�Ŵ��� Ŭ����")]
    public LobbyManager     lobbyManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //TODO : ���� â Ű��
            exitPanel.SetActive(true);
        }
    }

    public void ConnectGame()
    {
    
    }

    public void OpenSingleGamePanel()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

        singleGamePanel.SetActive(true);
    }

    public void CloseSingleGamePanel()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

        singleGamePanel.SetActive (false);
    }

    public void OpenSettingPanel()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

        settingPanel.SetActive(true);
    }

    public void OpenExitPanel()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

        exitPanel.SetActive(true);
    }

    public void CloseExitPanel()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

        exitPanel.SetActive (false);
    }

    public void CloseSettingPanel()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

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
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

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

    public void ButtonSound()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);
    }

    public void BackSound()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Back);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        // �����Ϳ��� ���� ���� ���� �÷��� ��带 �ߴ��մϴ�.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����� ��Ÿ�ӿ����� ���ø����̼��� �����մϴ�.
        Application.Quit();
#endif
    }
}
