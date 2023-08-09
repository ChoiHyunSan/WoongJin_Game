using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [Header("Ÿ��Ʋ �ؽ�Ʈ ����Ʈ")]
    [SerializeField]
    private string          TestText = "Touch To Test";
    [SerializeField]
    private string          GoToLobbyText = "Go To Lobby";

    [Header("��ư")]
    public Button           BtChangeScene;
    public TextMeshProUGUI  BtText;

    [Header("�г�")]
    public GameObject       HowToPlayPanel;

    // ��� ����
    private bool            _isCanMoveToLobby = false;


    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetCanMoveToLobby(bool value)
    {
        _isCanMoveToLobby = value;
        ChangeButtonText(_isCanMoveToLobby);
    }
    public void JoinRoom()
    {
        if (_isCanMoveToLobby)
        {
            GoToLobby();
        }
        else
        {
            GoToTest();
        }
    }

    void ChangeButtonText(bool isCanMoveToLobby)
    {
        if(isCanMoveToLobby)
        {
            BtText.text = GoToLobbyText;
        }
        else
        {
            BtText.text = TestText;
        }
    }

    void GoToLobby()
    {
        ChangeRoom("Lobby");
    }

    void GoToTest()
    {
        ChangeRoom("Test");
    }

    void ChangeRoom(string roomName)
    {
        SceneManager.LoadScene(roomName);
    }

    public void OpenHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        HowToPlayPanel.SetActive(false);
    }
}
