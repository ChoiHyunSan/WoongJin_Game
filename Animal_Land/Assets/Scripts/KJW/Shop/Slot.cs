using Contents;
using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler
{

    public ItemInfo SlotInfo { get; private set; }
    public int SlotIndex { get; private set; }
    public Action<int> ClickSlotAction = null;
    public bool isCanClick { get; private set; } // Ŭ���� �� �ִ���   
    public bool unLock { get; private set; } // �����ߴ���   
    ItemType _itemType = ItemType.Face; // ������ Ÿ�� ����
    Image _itemImage;
    GameObject _lock;

    void Start()
    {

        SlotIndex = transform.GetSiblingIndex();
        InitializeSlot(_itemType); // �⺻�� �� ��� Slot �ʱ�ȭ
        ShopManager.Instance.UpdateShopItemListAction += InitializeSlot; // ���� �ʱ�ȭ
        ViewManager.GetView<ShopMenuView>().OnShopClick += ResetSlot;
    }


    void InitializeSlot(ItemType itemType) // ���� ���� ����
    {
        if (_lock == null)
        {
            _lock = transform.GetChild(0).gameObject;
        }
        _itemType = itemType; // ���� ������ ������ Ÿ�� ����

        var element = DataManager.Instance.PropsItemDict[itemType.ToString()].ElementAtOrDefault(SlotIndex); // ����Ʈ�� �ش� �ε��� �����Ͱ� �ִٸ�

        if (element != default)
        {
            SlotInfo = element;
            isCanClick = true;
            if (DataManager.Instance.PlayerData.ShoppingList.Count > 0 && DataManager.Instance.PlayerData.ShoppingList.Contains(SlotInfo.Name)) // �̹� ������ ��ǰ�̶��
            {
                this.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                unLock = true;
                _lock.gameObject?.SetActive(false); // �ڹ��� ��Ȱ��ȭ 
            }
            else
            {
                this.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                unLock = false;
                _lock.gameObject?.SetActive(true); // �ڹ��� Ȱ��ȭ
            }
        }
        else
        {
            isCanClick = false;
            this.GetComponent<Image>().color = new Color(0, 0, 0, 0);// ���� �̹����� ������ �ʿ� ����
            _lock.gameObject?.SetActive(false); // �ڹ��� ��Ȱ��ȭ 
        }

        UpdateItemImage();
    }

    void UpdateItemImage() // ���� �������� �̹��� ������Ʈ
    {
        if (SlotInfo == null) return; // ���� ���Կ��� ��� �� �������� ���� ����� 

        _itemImage = GetComponent<Image>();
        if (_itemImage == null)
        {
            _itemImage = GetComponent<Image>();
        }

        Sprite itemSprite = Resources.Load<Sprite>($"Sprites/Items/{_itemType.ToString()}/{SlotInfo.Name}");
        if (itemSprite != null)
        {
            _itemImage.sprite = itemSprite;
        }
        else
        {
            _itemImage.sprite = null;
#if UNITY_EDITOR
            Debug.LogError($"���ҽ� ������ ������ {SlotInfo.Name} �̹����� �����ϴ�.");
#endif  
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
        ShopMenuView shopMenu = ViewManager.GetView<ShopMenuView>();
        if (DataManager.Instance.PlayerData.ShoppingList.Contains(SlotInfo.Name)) // �̹� ������ �������̶��
        {
            message = "�̹� ������ �������Դϴ�.";
            
            notHave = false;
            if (shopMenu != null)
            {
                shopMenu.canPutOn = true;
            }
        }
        else
        {
            message = $"{SlotInfo.Price} ��尡 �Ҹ�˴ϴ�.\n���� �Ͻðڽ��ϱ�?";
            if (shopMenu != null)
       
     {
                shopMenu.canPutOn = false;
            }
        }

        ClickSlotAction?.Invoke(SlotIndex);
        ShopManager.Instance.ItemInfo = SlotInfo; // ������ ���� �� ������ ������ ������ ������ ������ ������Ʈ
        ShopManager.Instance.SetCharacterCustom(); // ���� ���� �������� ĳ���Ϳ� �ӽ÷� ����
        ShopManager.Instance.cManager.ChangeCharacterSpecificPartsSprite(_itemType); // ��������Ʈ ����
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
