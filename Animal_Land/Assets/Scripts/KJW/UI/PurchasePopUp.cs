using Contents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePopUp : PopUpUI, IStatusCheckPopUP
{
    public Action<bool> checkAction; 
    public ItemInfo ClickSlotItemInfo { get; set; } // ���� ���� ���� ������ ����

    [SerializeField]
    private List<Button> selectBtns;
    [SerializeField]
    Text checkMessage;

    public override void Initialize()
    {
        base.Initialize();

        SetCheckMessage("���� �������� �������ּ���.");
        checkAction += ActivateButtons;
        for (int i = 0; i < selectBtns.Count; i++)
        {
            int index = i; // ������ ĸ���ϱ� ������ i�� �ȳְ� ���÷� ���� ������ �Ҵ��ؼ� ���
            selectBtns[i].onClick.AddListener(() => OnSelectPurchase(index));
            selectBtns[i].gameObject.SetActive(false);
        }
    }

    void ActivateButtons(bool flag)
    {
        closeButton?.gameObject.SetActive(!flag); // �ݴ�� �����ؾ��� ���� �Ⱥ���

        foreach (var button in selectBtns)
        {
            button.gameObject.SetActive(flag);
        }
    }

    void OnSelectPurchase(int index)
    {
        switch (index)
        {
            case 0: // ����
                // TODO : ��� Ȯ���ϱ� 

                PurchaseCheckPopUp popUpUI = ViewManager.GetView<PurchaseCheckPopUp>();
                if (popUpUI != null && popUpUI.name == "Selection_Result_PopUp") // �˾� â �̸��� ��������
                {
                    if (DataManager.Instance.PlayerData != null)
                    {
                        if(ClickSlotItemInfo != null) // ������ �� ������
                        {
                            string message = CheckPlayerGold(ClickSlotItemInfo.Price);
                            popUpUI.SetCheckMessage(message);
                        }
                    }
                    ViewManager.Show<PurchaseCheckPopUp>(true, true); // ���� Ȯ�� â
                }
                break;
            case 1: // ���
                ViewManager.ShowLast(); // ���� ȭ������ 
                blocker.gameObject?.SetActive(false); // ���Ŀ ��Ȱ��ȭ
                break;
        }
    }


    string CheckPlayerGold(int price)
    {
        int playerGold = DataManager.Instance.PlayerData.Gold;

        if (playerGold >= price) // �� �� ������
        {
            DataManager.Instance.PlayerData.Gold -= price;
            ViewManager.GetView<ShopMenuView>().OnShopClick?.Invoke();
            DataManager.Instance.PlayerData.ShoppingList.Add(ClickSlotItemInfo.Name, true);
            DataManager.Instance.SaveData<PlayerData>(DataManager.Instance.PlayerData, "PlayerData");
            return "���� �Ϸ�!";
        }
        else
        {
            int difference = Math.Abs(playerGold - price);
            return $"{difference} GOLD�� �����մϴ�.";
        }
    }

    public void SetCheckMessage(string message)
    {
        checkMessage.text = message;
    }


}
