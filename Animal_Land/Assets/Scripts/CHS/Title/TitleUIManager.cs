using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [Header("타이틀 텍스트 리스트")]
    [SerializeField]
    private string          TestText = "Touch To Test";
    [SerializeField]
    private string          GoToLobbyText = "Go To Lobby";

    [Header("버튼")]
    public Button           BtChangeScene;
    public TextMeshProUGUI  BtText;

    [Header("패널")]
    public GameObject       HowToPlayPanel;

    // 멤버 변수
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
       if(Application.internetReachability == NetworkReachability.NotReachable) // 데이터 다운로드 또는 인터넷 연결
        {
            TitlePopUp popUp = ViewManager.GetView<TitlePopUp>();
            if(popUp!=null)
            {
                popUp.SetCheckMessage("인터넷 연결을 확인하세요.");
                ViewManager.Show<TitlePopUp>(true, true);
            }
            Debug.LogError("인터넷 연결을 확인하세요.");
            return;
        }
       if(DataManager.Instance.Downloader.isDownloading) // 데이터 다운로드 중
        {
            TitlePopUp popUp = ViewManager.GetView<TitlePopUp>();
            if(popUp != null )
            { 
                popUp.SetCheckMessage("데이터를 다운받는 중입니다.");
                ViewManager.Show<TitlePopUp>(true, true);
            }
           
            Debug.LogError("데이터를 다운받는 중입니다.");
            return;
        }

       if(DataManager.Instance.PropsItemDict.Count<=0) // 데이터가 안불러와진 경우
        {
            DataManager.Instance.ReloadData();
            return;
        }

        ChangeRoom("Lobby 1");
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
