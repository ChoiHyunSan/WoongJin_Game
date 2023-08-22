using Contents;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuView : View
{
    public Action OnShopClick; // ���� Ŭ���� ���� ������Ʈ ��������Ʈ
    public bool canPutOn;

    [SerializeField] private Button backButton; // �ڷΰ��� ��ư
    [SerializeField] private Button buyButton; // ���� ��ư
    [SerializeField] private Button putOnButton; // ���� ��ư
    [SerializeField] private List<Button> categoryBtns; // ������ ī�װ� ��ư��
    [SerializeField] private List<Button> selectCharaceterBtns; // ĳ���� ���� ��ư
    [SerializeField] private Image characterChangeImage; // �ٲ� ĳ���� �̹���
    [SerializeField] private List<Sprite> characterSprites; // ĳ���� ��������Ʈ��
    [SerializeField] private Text goldText;
    [SerializeField] private Text buyText;

    public override void Initialize()
    {
        backButton?.onClick.AddListener(() => ViewManager.ShowLast()); // ������ â  
        buyButton?.onClick.AddListener(() => OnBuyButtonClicked()); // ����â Ȱ��ȭ
        putOnButton?.onClick.AddListener(() => OnPutOnButtonClicked()); // ĳ���� Ŀ���� ���� ��ư

        OnShopClick += UpdateShopInterface;
        OnShopClick?.Invoke(); ; // ��� UI Text ������Ʈ

        for (int i = 0; i < selectCharaceterBtns.Count; i++) // ĳ���� ���� ��ư�� �ʱ�ȭ
        {
            int characterIndex = i; // ������ ĸ���ϱ� ������ i�� �ȳְ� ���÷� ���� ������ �Ҵ��ؼ� ���
            selectCharaceterBtns[i]?.onClick.AddListener(() => OnCharacterButtonClicked(characterIndex));
        }

        for (int i = 0; i < categoryBtns.Count; i++) // ī�װ� ��ư�� �ʱ�ȭ
        {
            Contents.ItemType itemType = (Contents.ItemType)i; // ������ Ÿ������ ��ȯ
            categoryBtns[i]?.onClick.AddListener(() => OnItemCategoryButtonClicked(itemType));
        }
    }

    void UpdateShopInterface() // ���� ������Ʈ�� ����
    {
        DataManager.Instance.ReloadData();
        UpdateGoldText();
    }

    void UpdateGoldText() // ��� ������Ʈ ���� �ÿ� ������ ����
    {
        goldText.text = $"{DataManager.Instance.PlayerData.Gold}";
    }

    #region UI_Buttons

    void OnCharacterButtonClicked(int characterIndex) // ĳ���� ������ ���� �̺�Ʈ
    {
        if (characterChangeImage != null && characterSprites.Count > 0)
        {
            characterChangeImage.sprite = characterSprites[characterIndex];
            ShopManager.Instance.CharacterType = (CharacterType)characterIndex;
            ShopManager.Instance.LoadCharacterCustom();
        }
    }

    void OnItemCategoryButtonClicked(Contents.ItemType itemType) // ������ ī�װ� ����
    {
        ShopManager.Instance.CheckItemList(itemType);
    }

    void OnBuyButtonClicked()
    {
        ViewManager.Show<PurchasePopUp>(true, true);
    }

    void OnPutOnButtonClicked()
    {
        CharacterType characterType = ShopManager.Instance.CharacterType;
        PurchasePopUp popUp = ViewManager.GetView<PurchasePopUp>();

        if (popUp == null)
        {
#if UNITY_EDITOR
            Debug.LogError("PurchasePopUp�� ViewManager�� �����ϴ�");
#endif
            return;
        }
        if (ShopManager.Instance.ItemInfo == null)
        {
            popUp.SetCheckMessage("���� �� �������� ������");
        }
        else
        {
            if (DataManager.Instance.PlayerData.ShoppingList.ContainsKey(ShopManager.Instance.ItemInfo.Name) == false) // ������ �̺���
            {
                popUp.SetCheckMessage("�����ϰ� ���� �ʽ��ϴ�!");
            }
            else
            {
                ShopManager.Instance.SetCharacterCustom(); // ���� �ÿ� ���� ������ �ִ� �������̶��
                popUp.SetCheckMessage("���� �Ϸ�!");
            }

        }
        popUp.checkAction(false); // �Ϸ� ��ư�� �ߵ���
        ViewManager.Show<PurchasePopUp>(true, true);

        // ������ �Ŵ����� ĳ���� Ŀ���� �����͸� ���� ���� â�� ĳ���� Ŀ�������� �Ҵ�
        DataManager.Instance.CharacterCustomData[characterType.ToString()] = ShopManager.Instance.CharacterCustom;
        DataManager.Instance.SaveData<IDictionary<string, Contents.CharacterCustom>>
            (DataManager.Instance.CharacterCustomData, "CustomData");
        DataManager.Instance.ReloadData(); // �ٽ� ������ ������ �ҷ���
    }
    #endregion
}
