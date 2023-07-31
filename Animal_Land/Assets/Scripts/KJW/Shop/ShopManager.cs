using Contents;
using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Action<Contents.ItemType> UpdateShopItemListAction = null;
    public ItemInfo ItemInfo { get; set; } = null;
    public CharacterType CharacterType { get; set; } = CharacterType.Frog;
    public ItemType ItemType { get; set; } = ItemType.Face;

    public CharacterCustom CharacterCustom { get; set; } = new CharacterCustom();

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

    private void Start()
    {
        LoadCharacterCustom();
    }

    public void LoadCharacterCustom()
    {
        CharacterCustom = DataManager.Instance.CharacterCustomData[CharacterType.ToString()];
    }

    public void SetCharacterCustom() // �����Ϸ� �ÿ� �ӽ� ĳ���� Ŀ���ҿ� �ش� �������� �����صα� ���� �Լ�
    {
        CharacterCustom.ItemDict[ItemType.ToString()] = ItemInfo.Name; 
    }

    public static void CheckItemList(Contents.ItemType itemType)
    {
        instance.UpdateShopItemListAction.Invoke(itemType);
        instance.ItemType = itemType;
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
