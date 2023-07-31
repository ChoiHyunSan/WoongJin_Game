using Contents;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePopUp : PopUpUI, IStatusCheckPopUP
{
    public Action<bool> checkAction; // �ܺο��� ��� �� ��������Ʈ
    public ItemInfo ClickSlotItemInfo { get; set; } // ���� ���� ���� ������ ����

    [SerializeField] private List<Button> selectBtns;
    [SerializeField] private Text checkMessage;

    public override void Initialize()
    {
        base.Initialize();

        SetCheckMessage("���� �������� �������ּ���."); // �⺻ �޽���
        checkAction += ActivateButtons; // ��������Ʈ�� ��ư Ȱ��ȭ/��Ȱ��ȭ �Լ� �߰�
        for (int i = 0; i < selectBtns.Count; i++)
        {
            int index = i; // ������ ĸ���ϱ� ������ i�� �ȳְ� ���÷� ���� ������ �Ҵ��ؼ� ���
            selectBtns[i].onClick.AddListener(() => OnSelectPurchase(index));
            selectBtns[i].gameObject.SetActive(false);
        }
    }

    void ActivateButtons(bool flag) // ��ư�� Ȱ��ȭ
    {
        closeButton?.gameObject.SetActive(!flag); // �ݴ�� �����ؾ��� ���� �Ⱥ���

        foreach (var button in selectBtns) // ��, �ƴϿ� ��ư Ȱ��ȭ
        {
            button.gameObject.SetActive(flag);
        }
    }

    void OnSelectPurchase(int index) // ���� ��ư 
    {
        switch (index)
        {
            case 0: // ����
                PurchaseCheckPopUp popUpUI = ViewManager.GetView<PurchaseCheckPopUp>();
                if (popUpUI != null && popUpUI.name == "Selection_Result_PopUp") // �˾� â �̸��� ��������
                {
                    if (DataManager.Instance.PlayerData != null)
                    {
                        if(ShopManager.Instance.ItemInfo != null) // ������ �� ������
                        {
                            string message = CheckPlayerGold(ShopManager.Instance.ItemInfo.Price);
                            popUpUI.SetCheckMessage(message);
                        }
                    }
                    ViewManager.Show<PurchaseCheckPopUp>(true, true); // ���� Ȯ�� â
                }
                break;
            case 1: // ���
                ViewManager.ShowLast(); // ���� ȭ������ 
                blocker.gameObject?.SetActive(false); // ����Ŀ ��Ȱ��ȭ
                break;
        }
    }


    string CheckPlayerGold(int price)
    {
        int playerGold = DataManager.Instance.PlayerData.Gold;

        if (playerGold >= price) // �� �� ������
        {
            DataManager.Instance.PlayerData.Gold -= price; // ��� ����
            DataManager.Instance.PlayerData.ShoppingList.Add(ShopManager.Instance.ItemInfo.Name, true); // ���� ����Ʈ�� �߰�
            DataManager.Instance.SaveData<PlayerData>(DataManager.Instance.PlayerData, "PlayerData"); // �÷��̾� ������ ����
            ViewManager.GetView<ShopMenuView>().OnShopClick?.Invoke(); // ���� Gold UI ������Ʈ

            return "���� �Ϸ�!";
        }
        else
        {
            int difference = Math.Abs(playerGold - price); // ������ �ݾ�
            return $"{difference} GOLD�� �����մϴ�."; 
        }
    }

    public void SetCheckMessage(string message)
    {
        checkMessage.text = message;
    }


}