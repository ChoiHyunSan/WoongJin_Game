using Contents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler
{

    public ItemInfo SlotInfo { get; private set; }
    public int SlotIndex { get; private set; }
    bool isCanClick;

    void Start()
    {
        SlotIndex = transform.GetSiblingIndex();
        var element = DataManager.Instance.HatItemInfoList.ElementAtOrDefault(SlotIndex); // ����Ʈ�� �ش� �ε��� �����Ͱ� �ִٸ�
        if (element != default)
        {
            SlotInfo = element;
            isCanClick = true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"{SlotIndex} ��° ���� ���� ���� ����");
#endif
            isCanClick = false;
            this.GetComponent<Image>().color = new Color(0, 0, 0);// ���� �̹����� ������ �ʿ� ����
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCanClick) return; // ������ ���ϴ� �����̶�� 

        string message;
        bool notHave= true;
        if (DataManager.Instance.PlayerData.ShoppingList.ContainsKey(SlotInfo.Name))
        {
            message = "�̹� ������ �������Դϴ�.";
            notHave = false;
        }
        else
        {
            message = $"{SlotInfo.Price} Gold�� �Ҹ�˴ϴ�.\n{SlotInfo.Name}�� �����Ͻðڽ��ϱ�?";
        }
        
        PurchasePopUp purchasePopUp = ViewManager.GetView<PurchasePopUp>();
        if (purchasePopUp != null)
        {
            purchasePopUp.ClickSlotItemInfo = SlotInfo;
            purchasePopUp.checkAction?.Invoke(notHave); //
            purchasePopUp.SetCheckMessage(message); // �޽��� �ѱ��
        }


#if UNITY_EDITOR
        Debug.Log($"{SlotIndex} ��° ���� Ŭ��");
#endif
    }
}
