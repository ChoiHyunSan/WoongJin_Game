using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using TMPro;

// ������ ���� ���� ����, ���� UI�� �����ϴ� ���� �Ŵ���
public class SGameManager : MonoBehaviour
{
    // �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
    public static GameManager instance;
    private static GameManager _instance; // �̱����� �Ҵ�� static ����
    public Tilemap tileMap;

    public bool isGameover { get; private set; } // ���� ���� ����

    [SerializeField]
    GameObject[] _SpawnPos = new GameObject[4];

    GameObject _localUser;

    [Header("��ư")]
    public TextMeshProUGUI _playButton;

    private void Awake()
    {
        // ���� ���۰� ���ÿ� �÷��̾ �� ���� ������Ʈ�� ����
        int playerCharacterNum = 1;
        GameObject user = SetUser(playerCharacterNum);
        _localUser = user;

        // ���� ������ ������Ʈ
        UpdateUser(user, playerCharacterNum);
    }

    private void Start()
    {

    }

    private void Init()
    {

    }

    private GameObject SetUser(int playerCount)
    {
        // ������ ���� ��ġ ����
        Vector2 SpawnPos = GetSpawnPos(playerCount);

        // �÷��̾� ���� ������ ���� � ĳ������ ������ ������ ����

        // ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ����
        // ��, �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ����
        GameObject Object = GameObject.FindGameObjectWithTag("Player").gameObject;

        return Object;
    }

    private void UpdateUser(GameObject user, int playerCount)
    {
        // ���� Ŭ���̾�Ʈ������ ������Ʈ�Ǿ� �ٸ� Ŭ���̾�Ʈ�� ������ �Ѹ���.
        SCharacter character = user.GetComponent<SCharacter>();

        // ���̽�ƽ ����
        SetupJoyStick(user);

        // ĳ���� ���� ������Ʈ
        character.SetupCharacter(playerCount);
    }

    private void SetupJoyStick(GameObject user)
    {
        GameObject JoyStick = GameObject.FindGameObjectWithTag("GameController");

        JoyStick.GetComponent<SJoyStick>().Character = user;
    }

    private Vector2 GetSpawnPos(int playerCount)
    {
        Vector2 spawnPos = Vector2.zero;
        // TODO : Player�� Count�� ���� �˸´� ��ġ�� ����

        spawnPos = _SpawnPos[playerCount - 1].transform.position;

        Debug.Log(spawnPos);

        return spawnPos;
    }

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {

        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
    }

    // Ű���� �Է��� �����ϰ� ���� ������ ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //TODO : ���� â Ű��

        }
    }

    private UserManager GetUserManager()
    {
        GameObject Object = GameObject.Find("UserManager");
        if (Object != null)
        {
            UserManager userManager = Object.GetComponent<UserManager>();
            if (userManager != null)
            {
                return userManager;
            }
        }

        Debug.Log("User Manager is Null");
        return null;
    }

    public void StartGameBtn()
    {
        StartGame();
    }

    public void DropOutAllPlayerBtn()
    {

    }

    void DropOutAllPlayer()
    {

    }

    void StartGame()
    {
        SJoyStick JoyStick = GameObject.FindGameObjectWithTag("GameController").GetComponent<SJoyStick>();
        if (JoyStick != null)
        {
            if (JoyStick.IsGameStart() == true)
            {
                return;
            }
            else
            {
                JoyStick.SetIsGameStart(true);
                _playButton.text = "Game is Already Start";
            }
        }
    }
}