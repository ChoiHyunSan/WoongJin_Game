using Contents;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler
{

    public ItemInfo SlotInfo { get; private set; }
    public int SlotIndex { get; private set; }
    bool isCanClick; // Ŭ���� �� �ִ���
    ItemType _itemType = ItemType.Face; // ������ Ÿ�� ����

    void Start()
    {
        SlotIndex = transform.GetSiblingIndex();
        InitializeSlot(_itemType); // �⺻�� �� ��� Slot �ʱ�ȭ
        ShopManager.Instance.UpdateShopItemListAction += InitializeSlot;
        ViewManager.GetView<ShopMenuView>().OnShopClick += ResetSlot;
    }


    void InitializeSlot(ItemType itemType) // ���� ���� ����
    {

        _itemType = itemType; // ���� ������ ������ Ÿ�� ����

        var element = DataManager.Instance.PropsItemDict[itemType.ToString()].ElementAtOrDefault(SlotIndex); // ����Ʈ�� �ش� �ε��� �����Ͱ� �ִٸ�

        if (element != default)
        {
            SlotInfo = element;
            isCanClick = true;
            if (DataManager.Instance.PlayerData.ShoppingList.Count > 0 && DataManager.Instance.PlayerData.ShoppingList.ContainsKey(SlotInfo.Name))
            {
                this.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            }
            else
            {
                this.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            }
        }
        else
        {
            isCanClick = false;
            this.GetComponent<Image>().color = new Color(0, 0, 0, 0);// ���� �̹����� ������ �ʿ� ����
        }
    }

    void ResetSlot() // ������ ��ȭ�� ���� �� ����
    {
        InitializeSlot(_itemType);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCanClick) return; // ������ ���ϴ� �����̶�� 

        string message; // ���� �� �޽���
        bool notHave = true; // ������ �ִ� ����������

        if (DataManager.Instance.PlayerData.ShoppingList.ContainsKey(SlotInfo.Name)) // �̹� ������ �������̶��
        {
            message = "���� �Ϸ�.";
            notHave = false;
            ViewManager.GetView<ShopMenuView>().ToggleBuy = false;
        }
        else
        {
            message = $"{SlotInfo.Price} Gold�� �Ҹ�˴ϴ�.\n{SlotInfo.Name}�� �����Ͻðڽ��ϱ�?";
            ViewManager.GetView<ShopMenuView>().ToggleBuy = true;
        }

        ViewManager.GetView<ShopMenuView>()?.UpdateBuyText(notHave); // ���� ��ư�� �������� �������� ���� �� ������Ʈ
        ShopManager.Instance.ItemInfo = SlotInfo;

        PurchasePopUp purchasePopUp = ViewManager.GetView<PurchasePopUp>();
        if (purchasePopUp != null)
        {
            purchasePopUp.checkAction?.Invoke(notHave);
            purchasePopUp.SetCheckMessage(message); // �޽��� �ѱ��
        }


#if UNITY_EDITOR
        Debug.Log($"{SlotIndex} ��° ���� Ŭ��");
#endif
    }
}
