using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using TMPro;
using Mono.Cecil.Cil;

// ������ ���� ���� ����, ���� UI�� �����ϴ� ���� �Ŵ���
public class SGameManager : MonoBehaviour
{
    // �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
    public static GameManager instance;
    private static GameManager _instance; // �̱����� �Ҵ�� static ����
    public Tilemap _tileMap;
    public TilemapCollider2D _tileCollider;

    public bool isGameover { get; private set; } // ���� ���� ����

    [SerializeField]
    GameObject[] _SpawnPos = new GameObject[4];
    GameObject _localUser;

    [Header("UI")]
    public TextMeshProUGUI _playButton;
    [SerializeField] private SGameUIManager UIManager;

    [Header("���ھ�")]
    [SerializeField] private int _areaScore;
    [SerializeField] private int _monsterScore;
    [SerializeField] private int _timeScore;
    [SerializeField] private int _solveScore;

    [Header("���� ��ȭ ȹ�� ��")]
    [SerializeField] private int _money = 0;

    [Header("�ð�")]
    [SerializeField] private float _gameTime;
    [SerializeField] private float _maxGameTime;
    [SerializeField] private Slider _timeSlider;

    [Header("���� ����")]
    [SerializeField] private bool _gameOver = false;
    [SerializeField] private bool _gameStop = false;

    [Header("������ ��� ���� ����")]
    [SerializeField] private bool _canUseHpItem = false;
    [SerializeField] private bool _canUseSpeedItem = false;
    [SerializeField] private bool _canUseGaugeItem = false;

    [Header("������ ȿ�� ��ġ")]
    [SerializeField] private float _hpItemValue;
    [SerializeField] private float _speedItemValue;
    [SerializeField] private float _gaugeItemValue;

    DataManager _dataManager;

    [Header("�÷��̾� ������Ʈ")]
    [SerializeField] private GameObject _player;

    private void Awake()
    {
        // ���� ���۰� ���ÿ� �÷��̾ �� ���� ������Ʈ�� ����
        // TODO : ������ ĳ������ ���� �� ������ �޾ƿ´�.
        _dataManager = GameObject.Find("@DataManager").GetComponent<DataManager>();
        if(_dataManager !=null)
        {
            Init();
        }

        int playerCharacterNum = 1;

        GameObject user = SetUser(playerCharacterNum);
        _localUser = user;

        // ���� ������ ������Ʈ
        UpdateUser(user, playerCharacterNum);

        // ���Ϳ� ������ ������Ʈ
        UpdateMonsters();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //TODO : ���� â Ű��

        }

        UpdateTime();
    }

    private void Start()
    {
        StartGame();
    }

    private void Init()
    {
        // ������ ����
        if (_dataManager.PlayerStat.HP > 0)
        {
            _canUseHpItem = true;
        }
        if(_dataManager.PlayerStat.Speed >0)
        {
            _canUseSpeedItem = true;
        }
        if (_dataManager.PlayerStat.Shield >0)
        {
            _canUseGaugeItem = true;
        }

        // �÷��̾� ���� ����
        

    }

    void UpdateTime()
    {
        if(_gameStop || _gameOver)
        {
            return;
        }

        // TODO : ���� ���ѽð� ����
        if(_gameTime < _maxGameTime)
        {
            _gameTime += Time.deltaTime;
        }

        // TODO : ���ѽð� UI ������Ʈ
        if(_timeSlider != null)
        {
            float value = _gameTime / _maxGameTime;
            UIManager.UpdateTime(1f - value);
            // _timeSlider.value =  1f - value;
        }

        // TODO : ���ѽð��� �Ѿ�� ���� ����
        if(_gameTime >= _maxGameTime)
        {
            EndGame(false);
        }
    }

    private void UpdateMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            monster.GetComponent<SMonsterMove>().Init();
        }
    }

    private GameObject SetUser(int playerCount)
    {
        // ������ ���� ��ġ ����
        Vector2 SpawnPos = GetSpawnPos(playerCount);

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
    public void AddMonsterScore(int score)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            _monsterScore += score;
        }
    }

    public void SetAreaScore(int count)
    {
        // ������ �� %
        float wholeArea = ((46 + 28) * (26 + 16));

        if (!isGameover)
        {
            _areaScore = (int)(wholeArea / _areaScore) * 10;
        }

        // 100% �� ä�� ���
        if(count == wholeArea)
        {
            EndGame(true);
        }
    }

    public void AddSolveScore(int count = 1)
    {
        _solveScore += count * 100;
    }

    public int GetTotalScore()
    {
        return _monsterScore + _areaScore + _timeScore + _solveScore;
    }

    // ���� ���� ó��
    public void EndGame(bool gameClear)
    {
        // ���� ���� ���¸� ������ ����
        isGameover = gameClear;

        // ���� ���� ����
        StopGame();

        // TODO : ���� ���� �гΰ� �Բ� ��� â ��� (ȹ�� ����, ��ȭ ���� �Ѱ��ֱ�)
        if(UIManager == null)
        {
            return;
        }

        // TODO : _timeScore ����


        UIManager.OpenResultPanel(isGameover, GetTotalScore(), _money, _gameTime);
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

    public void StartGame()
    {
        // TODO : �÷��̾� �̵�
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<SCharacter>().StartPlayer();
        }

        // TODO : ���� �̵� 
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            if (monster != null && monster.GetComponent<SMonster>() != null)
            {
                monster.GetComponent<SMonster>().StartMonster();
            }
        }

        _tileCollider.enabled = true;

        // �ð��� �簳
        _gameStop = false;
    }

    public void StopGame()
    {
        // �÷��̾� ���� (Move)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.GetComponent<SCharacter>().StopPlayer();
        }

        // Monster ���� (Move, Attack ����)
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject monster in monsters)
        {
            if(monster != null && monster.GetComponent<SMonster>() != null)
            {
                monster.GetComponent<SMonster>().StopMonster();
            }
        }

        _tileCollider.enabled = false;

        // �ð��� ����
        _gameStop = true;
    }

    public int CalAreaScore()
    {
        int result = 0;
        int count = 0;

        // �÷��̾� AreaColor
        Color playerAreaColor = _localUser.GetComponent<STileColorChange>().GetAreaColor();

        // -45, 15 / 28 , -26
        // (46 + 28) * (26 + 16)

        for(int i = -45; i <= 28; i++)
        {
            for(int j = -26; j <= 15; j++)
            {
                if (_tileMap.GetColor(new Vector3Int(i, j, 0)) == playerAreaColor)
                    count++;
            }
        }

        // AreaScore ����
        SetAreaScore(count);

        // ��ȯ
        return result;
    }

    public void UseItemHp()
    {
        // ��� ���� ���� Ȯ��
        if(_canUseHpItem == false)
        {
            return;
        }
        _canUseHpItem = false;

        // Hp ȸ��
        _player.GetComponent<SCharacterHp>().Heal(_hpItemValue);

        // TODO : ������ ��Ȱ��ȭ


    }

    public void UseItemSpeed()
    {
        // ��� ���� ���� Ȯ��
        if (_canUseSpeedItem == false)
        {
            return;
        }
        _canUseSpeedItem = false;

        // ���ǵ� ���


        // ���� �ð� ���Ŀ� �پ���.


        // TODO : ������ ��Ȱ��ȭ
    }

    public void UseItemGauge()
    {
        // ��� ���� ���� Ȯ��
        if (_canUseGaugeItem == false)
        {
            return;
        }
        _canUseGaugeItem = false;

        // ������ ȸ��
        _player.GetComponent<SCharacter>().AddMoveGauge(_gaugeItemValue);

        // UI ����
        UIManager.AddMoveGauge(_gaugeItemValue);

        // TODO : ������ ��Ȱ��ȭ

    }
}