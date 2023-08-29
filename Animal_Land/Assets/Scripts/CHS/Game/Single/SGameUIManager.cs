using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SGameUIManager : MonoBehaviour
{
    [Header("ȭ��")]
    [SerializeField] private GameObject _resultScreen;
    [SerializeField] private GameObject _solveScreen;
    [SerializeField] private GameObject _settingScreen;

    [Header("�����̴�")]
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private Slider _moveGaugeSlider;
    [SerializeField] private Slider _hpSlider;

    [Header("�׽�Ʈ ��")]
    [SerializeField] private float _addGaugeValue;

    private DrawLine _line;
    private SGameManager _gameManager;
    private SoundManager _soundManager;
    private void Awake()
    {
        _line = GetComponent<DrawLine>();
        _gameManager = GameObject.Find("GameManager").GetComponent<SGameManager>();
        _soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSolveScreen()
    {
        // Solve Screen Ȱ��ȭ
        _solveScreen.SetActive(true);

        _gameManager.StopGame();

        _soundManager.PlayEffect(Effect.Button);
    }

    public void CloseSolveScreen()
    {
        // Solve Screen ��Ȱ��ȭ
        _solveScreen.SetActive(false);

        _gameManager.StartGame();

        _line.ClearLIne();

        _soundManager.PlayEffect(Effect.Back);
    }

    public void OpenSettingScreen()
    {
        _settingScreen.SetActive(true);

        _gameManager.StopGame();

        _soundManager.PlayEffect(Effect.Button);
    }

    public void CloseSettingScreen()
    {
        _settingScreen.SetActive(false);

        _gameManager.StartGame();

        _soundManager.PlayEffect(Effect.Back);
    }

    public void OpenResultPanel(bool gameClear, int score, int money, float gameTime)
    {
        // Result Screen Ȱ��ȭ
        _resultScreen.SetActive(true);

        TextMeshProUGUI Score = _resultScreen.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Gold = _resultScreen.transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>();

        Score.text = score.ToString();
        Gold.text = money.ToString();

        // ȿ���� ���
        _soundManager.PlayEffect(Effect.Gameover);
    }
    
    public void UpdateHp(float value)
    {
        _hpSlider.value = value;
    }

    public void UpdateGauge(float value)
    {
        _moveGaugeSlider.value = value;
    }

    public void UpdateTime(float value)
    {
        _timeSlider.value = value;
    }

    public void TimeOver()
    {
        _timeSlider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void AddMoveGauge(float val)
    {
        float value = _moveGaugeSlider.value + val;

        if(value > 1f)
        {
            value = 1f;
        }

        _moveGaugeSlider.value = value;
    }

    public void BackToLobby()
    {
        // �κ�� �̵�
        SceneManager.LoadScene("Lobby 1");

        _soundManager.PlayEffect(Effect.Button);
    }

    public void Restart()
    {
        // �����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        _soundManager.PlayEffect(Effect.Button);
    }

    public void Resume()
    {
        // ���� �г� ��Ȱ��ȭ
        CloseSettingScreen();

        _soundManager.PlayEffect(Effect.Back);
    }


}

