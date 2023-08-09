using Contents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartView : View
{
    [SerializeField] private List<Button> statButtons;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button closeButton;

    private PlayerStat _playerStat = new PlayerStat();
    private int _gold = 0; // �����ص״� â�� ������ ��� �����ֱ� ����

    public override void Initialize()
    {
        closeButton.onClick.AddListener(() => OnCloseGameStartPanel());
        gameStartButton.onClick.AddListener(() => OnGameStartButton());
        for (int i = 0; i < statButtons.Count; i++)
        {
            int index = i;
            statButtons[index].onClick.AddListener(() => OnStatPurchaseButton(index));
        }
    }

    private void OnEnable()
    {
        _playerStat = new PlayerStat(); // ���� �� ���� ������ �ʱ�ȭ �ϱ� ���ؼ�
    }

    #region GAME_PANEL_BUTTONS

    private void OnCloseGameStartPanel()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.PlayerStat = new PlayerStat(); // ���� �� ���ӿ� ���� ��� �� ������ �����͸Ŵ����� �Ѱ���
        }
        DataManager.Instance.PlayerData.Gold += _gold; // ��� �ǵ�������
        _gold = 0; // �ٽ� 0���� �ʱ�ȭ
    }

    void OnGameStartButton()
    {
        if (DataManager.Instance.PlayerStat == null)
        {
#if UNITY_EDITOR
            Debug.LogError("������ �Ŵ����� �÷��̾� ������ NULL�Դϴ�.");
#endif
            return;
        }
        DataManager.Instance.PlayerStat = _playerStat; // ���� �� ���ӿ� ���� ��� �� ������ �����͸Ŵ����� �Ѱ���
    }

    void OnStatPurchaseButton(int index)
    {
        switch (index)
        {
            case 0:
                if (_playerStat.CheckForPurchase(index))
                {
                    _playerStat.Speed++;
                }
                break;
            case 1:
                if (_playerStat.CheckForPurchase(index))
                {
                    _playerStat.HP++;
                }
                break;
            case 2:
                if (_playerStat.CheckForPurchase(index))
                {
                    _playerStat.Shield++;
                }
                break;
        }

        Debug.Log($"Speed: {_playerStat.Speed} HP: {_playerStat.HP} Shield {_playerStat.Shield}");
    }
    #endregion
}
