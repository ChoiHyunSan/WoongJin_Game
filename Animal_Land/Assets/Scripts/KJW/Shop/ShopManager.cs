using Contents;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Action<Contents.ItemType> UpdateShopItemListAction = null;
    public Action<int> ChangeSlotStatusAction = null;
    public ItemInfo ItemInfo { get; set; } = null;
    public CharacterType CharacterType { get; set; } = CharacterType.Frog; // ���� ���� â�� ĳ����
    public ItemType ItemType { get; set; } = ItemType.Face; // ���� ���� â�� ������ ī�װ�

    public CharacterCustom CharacterCustom { get; set; } = new CharacterCustom();

    [SerializeField] private List<Slot> slots; // ���� �̹�����

    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ShopManager();
            }
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadCharacterCustom();

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ClickSlotAction += OnSlotClicked;
        }
    }

    public void LoadCharacterCustom()
    {
        CharacterCustom = DataManager.Instance.CharacterCustomData[CharacterType.ToString()];
    }

    public void SetCharacterCustom() // �����Ϸ� �ÿ� �ӽ� ĳ���� Ŀ���ҿ� �ش� �������� �����صα� ���� �Լ�
    {
        CharacterCustom.ItemDict[ItemType.ToString()] = ItemInfo.Name;
    }

    public void CheckItemList(Contents.ItemType itemType) // ������ ����Ʈ �ҷ��� �� ���
    {
        instance.UpdateShopItemListAction?.Invoke(itemType);
        instance.ItemType = itemType;
    }

    void OnSlotClicked(int slotIndex)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].isCanClick == false) continue; // Ŭ���� �ȵǴ� �����̶�� �ѱ�

            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage != null)
            {
                if (slots[i].SlotIndex == slotIndex)
                {
                    slotImage.color = Color.red;
                }
                else
                {
                    slotImage.color = slots[i].unLock ? Color.white : Color.black; // �������� �������� ���� �� ��  
                }
            }
        }
    }

    private void OnDisable()
    {
        if (ItemInfo != null)
        {
            ItemInfo = null;
            ViewManager.GetView<PurchasePopUp>()?.SetCheckMessage("���� �������� �������ּ���.");
            ViewManager.GetView<PurchasePopUp>()?.checkAction(false);
        }
    }

}
