using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SGameUIManager : MonoBehaviour
{
    [Header("ȭ��")]
    [SerializeField] private GameObject _resultScreen;
    [SerializeField] private GameObject _solveScreen;

    [Header("�����̴�")]
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private Slider _moveGaugeSlider;
    [SerializeField] private Slider _hpSlider;

    [Header("�׽�Ʈ ��")]
    [SerializeField] private float _addGaugeValue;

    private SGameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<SGameManager>();
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
    }

    public void CloseSolveScreen()
    {
        // Solve Screen ��Ȱ��ȭ
        _solveScreen.SetActive(false);

        _gameManager.StartGame();
    }

    public void OpenResultPanel(bool gameClear, int score, int money, float gameTime)
    {
        // Result Screen Ȱ��ȭ
        _resultScreen.SetActive(true);
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

    public void AddMoveGauge()
    {
        float value = _moveGaugeSlider.value + _addGaugeValue;

        if(value > 1f)
        {
            value = 1f;
        }

        _moveGaugeSlider.value = value;
    }
}
