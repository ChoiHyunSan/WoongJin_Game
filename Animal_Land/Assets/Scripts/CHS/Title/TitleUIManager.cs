using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [Header("Ÿ��Ʋ �ؽ�Ʈ ����Ʈ")]
    [SerializeField]
    private string TestText = "Touch To Test";
    [SerializeField]
    private string GoToLobbyText = "Go To Lobby";

    [Header("��ư")]
    public Button BtChangeScene;
    public TextMeshProUGUI BtText;

    [Header("�г�")]
    public GameObject HowToPlayPanel;

    // ��� ����
    private bool _isCanMoveToLobby = false;


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
        if (isCanMoveToLobby)
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
        if (Application.internetReachability == NetworkReachability.NotReachable) // ������ �ٿ�ε� �Ǵ� ���ͳ� ����
        {
            TitlePopUp popUp = ViewManager.GetView<TitlePopUp>();
            if (popUp != null)
            {
                popUp.SetCheckMessage("���ͳ� ������ Ȯ���ϼ���.");
                ViewManager.Show<TitlePopUp>(true, true);
            }
            Debug.LogError("���ͳ� ������ Ȯ���ϼ���.");
            return;
        }

        if (DataManager.Instance.PropsItemDict.Count <= 0) // �����Ͱ� �Ⱥҷ����� ���
        {

            TitlePopUp popUp = ViewManager.GetView<TitlePopUp>();
            if (popUp != null)
            {
                popUp.SetCheckMessage("�����͸� �ٿ�޴� ���Դϴ�.");
                ViewManager.Show<TitlePopUp>(true, true);
            }

            Debug.LogError("�����͸� �ٿ�޴� ���Դϴ�.");
            if (DatabaseManager.Instance != null)
            {
              DatabaseManager.Instance.ReadDB(DataType.ItemData);
            }

            return;
        }

        if(DataManager.Instance.CharacterCustomData.Count <= 0)
        {
            TitlePopUp popUp = ViewManager.GetView<TitlePopUp>();
            if (popUp != null)
            {
                popUp.SetCheckMessage("�����͸� �ٿ�޴� ���Դϴ�.");
                ViewManager.Show<TitlePopUp>(true, true);
            }

            if (DatabaseManager.Instance != null)
            {
                DatabaseManager.Instance.ReadDB(DataType.CustomData);
            }
        }

        ChangeRoom("Lobby 1");
    }

    void GoToTest()
    {
        ChangeRoom("Test");
    }

    void ChangeRoom(string roomName)
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);

        SceneManager.LoadScene(roomName);
    }

    public void OpenHowToPlay()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);
        HowToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        GetComponent<SoundManager>().PlayEffect(Effect.Button);
        HowToPlayPanel.SetActive(false);
    }
}
