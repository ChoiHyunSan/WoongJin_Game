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
    [SerializeField] private int _timeScore;
    [SerializeField] private int _solveScore;

    [Header("���� ��ȭ ȹ�� ��")]
    [SerializeField] private int _money = 0;

    [Header("�ð�")]
    [SerializeField] private float _gameTime;
    [SerializeField] private float _maxGameTime;
    [SerializeField] private Slider _timeSlider;

    [Header("���� ����")]
    [SerializeField] private bool   _gameOver = false;
    [SerializeField] private bool   _gameStop = false;
    [SerializeField] private float  _stageBonus = 1f;   // �������� �� ���� ����

    [Header("������ ��� ���� ����")]
    [SerializeField] private bool _canUseHpItem = false;
    [SerializeField] private bool _canUseSpeedItem = false;
    [SerializeField] private bool _canUseGaugeItem = false;

    [Header("������ ȿ�� ��ġ")]
    [SerializeField] private float _hpItemValue;
    [SerializeField] private float _speedItemValue;
    [SerializeField] private float _speedtimeValue;  // ���� �ð�
    [SerializeField] private float _gaugeItemValue;
    [SerializeField] private float _timeItemValue;

    [Header("���� Ǯ�� ����")]
    [SerializeField] private float _solveGaugeValue;

    DataManager _dataManager;

    [Header("�÷��̾� ������Ʈ")]
    [SerializeField] private GameObject _player;
    private string _character;
    

    private void Awake()
    {
        // ���� ���۰� ���ÿ� �÷��̾ �� ���� ������Ʈ�� ����
        // TODO : ������ ĳ������ ���� �� ������ �޾ƿ´�.

        int playerCharacterNum = 1;
        _dataManager = GameObject.Find("@DataManager").GetComponent<DataManager>();
        if(_dataManager !=null)
        {
            playerCharacterNum = Init();
        }

        GameObject user = SetUser(playerCharacterNum);
        _localUser = user;

        // ���� ������ ������Ʈ
        UpdateUser(user, playerCharacterNum, _character);

        // ���Ϳ� ������ ������Ʈ
        UpdateMonsters();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //TODO : ���� â Ű��
            UIManager.OpenSettingScreen();
        }

        UpdateTime();
    }

    private void Start()
    {
        StartGame();
    }

    private int Init()
    {
        // ������ ����
        if (_dataManager.PlayerStat.HP > 0)
        {
            _canUseHpItem = true;
            UIManager.ActiveButton("Hp", true);
        }
        if(_dataManager.PlayerStat.Speed >0)
        {
            _canUseSpeedItem = true;
            UIManager.ActiveButton("Speed", true);
        }
        if (_dataManager.PlayerStat.Energy >0)
        {
            _canUseGaugeItem = true;
            UIManager.ActiveButton("Gauge", true);
        }

        // �÷��̾� ���� ����
        int playerNum = 0;
        _character = _dataManager.PlayerData.Character;
        if (_character == "Bird")
        {
            playerNum = 1;
        }
        else if (_character == "Dog")
        {
            playerNum = 2;
        }
        else if (_character == "Frog")
        {
            playerNum = 3;
        }
        else
        {
            playerNum = 4;
        }

        // ������ ���� ����    


        return playerNum;
    }

    void UpdateTime()
    {
        if (_gameStop || _gameOver)
        {
            return;
        }

        // TODO : ���� ���ѽð� ����
        if(_gameTime < _maxGameTime)
        {
            _gameTime += Time.deltaTime;

            // TODO : ���ѽð��� �Ѿ�� ���� ����
            if (_gameTime + Time.deltaTime >= _maxGameTime)
            {
                EndGame(false);
            }
        }

        // TODO : ���ѽð� UI ������Ʈ
        if(_timeSlider != null)
        {
            float value = _gameTime / _maxGameTime;
            UIManager.UpdateTime(1f - value);
            // _timeSlider.value =  1f - value;
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

    private void UpdateUser(GameObject user, int playerCount, string characterType)
    {
        // ���� Ŭ���̾�Ʈ������ ������Ʈ�Ǿ� �ٸ� Ŭ���̾�Ʈ�� ������ �Ѹ���.
        SCharacter character = user.GetComponent<SCharacter>();

        // ���̽�ƽ ����
        SetupJoyStick(user);

        // ĳ���� ���� ������Ʈ
        character.SetupCharacter(playerCount, characterType);
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

    }

    public void SetAreaScore(int count)
    {
        // ������ �� %
        float wholeArea = ((46 + 28) * (26 + 16));

        if (!isGameover)
        {
            _areaScore = (int)((count / wholeArea) * 100);
#if UNITY_EDITOR
            Debug.Log("areaScore  : " + _areaScore);
#endif
        }

        // 100% �� ä�� ���
        if(count == wholeArea)
        {
            EndGame(true);
        }
    }

    public void AddSolveScore()
    {
        _solveScore += 5;
    }

    public int GetTotalScore()
    {
        return (int)((_areaScore + _timeScore + _solveScore) * _stageBonus);
    }

    // ���� ���� ó��
    public void EndGame(bool gameClear)
    {
        // ���� ���� ���¸� ������ ����
        isGameover = gameClear;

        // ���� ���� ����
        StopGame();

        // TODO : ���� ���� �гΰ� �Բ� ��� â ��� (ȹ�� ����, ��ȭ ���� �Ѱ��ֱ�)
        if (UIManager == null)
        {
            return;
        }

        // TODO : _timeScore ���� (���� ���� ��, �ð� ���� X)
        _timeScore = (int)(_maxGameTime - _gameTime);
        if(isGameover == false)
        {
            _timeScore = 0;
        }

        // ȹ�� ��ȭ ���
        _money = GetTotalScore() / 5;
        UIManager.CloseSettingScreen();
        UIManager.CloseSolveScreen();
        UIManager.OpenResultPanel(isGameover, GetTotalScore(), _money, _gameTime);

        // DataManager�� �� ���� (��ȭ, ���ھ�, Ŭ���� ���� ��)
        int plusScore = GetTotalScore();
        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.WriteDB(plusScore);
        }

        // ȹ�� ��ȭ ���
        _money = GetTotalScore() / 5;
        int newGold = _dataManager.PlayerData.Gold + _money;
        _dataManager.PlayerData.Gold = newGold;
        DataManager.Instance.SavePlayerData(); // �÷��̾� ������ ����
    }

    public void TimeOver(float value)
    {
        if(value <= 0)
        {
            // TImeSlider�� FillArea�� ��Ȱ��ȭ


            EndGame(false);
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

    public void StartObject()
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
    }

    public void StopObject()
    {
        // �÷��̾� ���� (Move)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<SCharacter>().StopPlayer();
        }

        // Monster ���� (Move, Attack ����)
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            if (monster != null && monster.GetComponent<SMonster>() != null)
            {
                monster.GetComponent<SMonster>().StopMonster();
            }
        }

        _tileCollider.enabled = false;
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

        // ȿ���� ���
        UIManager.GetComponent<SoundManager>().PlayEffect(Effect.Button);

        // TODO : ������ ��Ȱ��ȭ
        UIManager.ActiveButton("Hp", false);
    }

    public void UseItemSpeed()
    {
        // ��� ���� ���� Ȯ��
        if (_canUseSpeedItem == false)
        {
            return;
        }
        _canUseSpeedItem = false;

        SJoyStick JoyStick = GameObject.FindGameObjectWithTag("GameController").GetComponent<SJoyStick>();
        // ���ǵ� ���
        JoyStick.SpeedUp(_speedItemValue);

        // ���� �ð� ���Ŀ� �پ���.
        Invoke("SpeedDown", _speedtimeValue);

        // ȿ���� ���
        UIManager.GetComponent<SoundManager>().PlayEffect(Effect.Button);

        // TODO : ������ ��Ȱ��ȭ
        UIManager.ActiveButton("Speed", false);

    }

    public void SpeedDown()
    {
        SJoyStick JoyStick = GameObject.FindGameObjectWithTag("GameController").GetComponent<SJoyStick>();
        JoyStick.SpeedDown();
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
        float sliderValue = _player.GetComponent<SCharacter>().AddMoveGauge(_gaugeItemValue);
        _player.GetComponent<SCharacter>().StartPlayer();

        // UI ����
        UIManager.AddMoveGauge(sliderValue);

        // ȿ���� ���
        UIManager.GetComponent<SoundManager>().PlayEffect(Effect.Button);

        // TODO : ������ ��Ȱ��ȭ
        UIManager.ActiveButton("Gauge", false);
    }

    public void Solve()
    {
        // ������ ȸ��
        float sliderValue = _player.GetComponent<SCharacter>().AddMoveGauge(_solveGaugeValue);

        // UI ����
        UIManager.AddMoveGauge(sliderValue);
    }

    public void DropItemHp()
    {
        _player.GetComponent<SCharacterHp>().Heal(_hpItemValue);
    }

    public void DropItemTime()
    {
        _gameTime -= _timeItemValue;
        if(_gameTime < 0)
        {
            _gameTime = 0;
        }
        // TODO : ���ѽð� UI ������Ʈ
        if (_timeSlider != null)
        {
            float value = _gameTime / _maxGameTime;
            UIManager.UpdateTime(1f - value);
            // _timeSlider.value =  1f - value;
        }

    }
    public void DropItemGauge()
    {
        // ������ ȸ��
        float sliderValue = _player.GetComponent<SCharacter>().AddMoveGauge(_gaugeItemValue);

        // UI ����
        UIManager.AddMoveGauge(sliderValue);
    }
    public void DropItemSpeed()
    {
        SJoyStick JoyStick = GameObject.FindGameObjectWithTag("GameController").GetComponent<SJoyStick>();
        // ���ǵ� ���
        JoyStick.SpeedUp(_speedItemValue);

        // ���� �ð� ���Ŀ� �پ���.
        Invoke("SpeedDown", _speedtimeValue);
    }
}