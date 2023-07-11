using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

// ������ ���� ���� ����, ���� UI�� �����ϴ� ���� �Ŵ���
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
    public static GameManager   instance;
    private static GameManager  _instance; // �̱����� �Ҵ�� static ����
    public Tilemap              tileMap;

    public bool                 isGameover { get; private set; } // ���� ���� ����

    [SerializeField]
    GameObject[]                _SpawnPos = new GameObject[4];

    GameObject _localUser;

    // �ֱ������� �ڵ� ����Ǵ�, ����ȭ �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ���� ������Ʈ��� ���� �κ��� �����
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }

        // TODO : ĳ���Ͱ� ��������� ���� �Է��� ���� ���ϰ� �Ѵ�.


        // ���� ���۰� ���ÿ� �÷��̾ �� ���� ������Ʈ�� ����
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        GameObject user = SpawnUser(playerCount);
        _localUser = user;

        // ���� ������ ������Ʈ
        UpdateUser(user, playerCount);
    }

    private void Start()
    {

    }

    private void Init()
    {
        
    }

    private GameObject SpawnUser(int playerCount)
    {
        // ������ ���� ��ġ ����
        Vector2 SpawnPos = GetSpawnPos(playerCount);

        // �÷��̾� ���� ������ ���� � ĳ������ ������ ������ ����

        // ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ����
        // ��, �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ����
        GameObject Object = PhotonNetwork.Instantiate("Prefabs/Character", SpawnPos, Quaternion.identity, 0);

        return Object;
    }

    private void UpdateUser(GameObject user, int playerCount)
    {
        // ���� Ŭ���̾�Ʈ������ ������Ʈ�Ǿ� �ٸ� Ŭ���̾�Ʈ�� ������ �Ѹ���.
        PhotonView pv = user.GetComponent<PhotonView>();
        Character character = user.GetComponent<Character>();
        if(pv.IsMine == true)
        {
            // ���̽�ƽ ����
            SetupJoyStick(user);

            // ĳ���� ���� ������Ʈ
            character.SetupCharacter(playerCount);
        }
    }

    private void SetupJoyStick(GameObject user)
    {
        GameObject JoyStick = GameObject.FindGameObjectWithTag("GameController");

        if (user.GetComponent<PhotonView>().IsMine == true)
        {
            JoyStick.GetComponent<JoyStick>().Character = user;
        }
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
            PhotonNetwork.LeaveRoom();
        }
    }

    // ���� ������ �ڵ� ����Ǵ� �޼���
    public override void OnLeftRoom()
    {
        // ���� ������ �κ� ������ ���ư�
        SceneManager.LoadScene("Lobby");
    }

    private UserManager GetUserManager()
    {
        GameObject Object = GameObject.Find("UserManager");
        if(Object != null)
        {
            UserManager userManager = Object.GetComponent<UserManager>();
            if(userManager != null)
            {
                return userManager;
            }
        }

        Debug.Log("User Manager is Null");
        return null;
    }
}