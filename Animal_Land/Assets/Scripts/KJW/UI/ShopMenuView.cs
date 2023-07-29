using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuView : View
{
    [SerializeField]
    private Button backButton; // �ڷΰ��� ��ư
    [SerializeField]
    private Button buyButton; // ���� ��ư
    [SerializeField]
    private Button saveButton; // ���� ��ư
    [SerializeField]
    private List<Button> selectCharaceterBtns; // ĳ���� ���� ��ư
    [SerializeField]
    private Image characterChangeImage; // �ٲ� ĳ���� �̹���
    [SerializeField]
    private List<Sprite> characterSprites; // ĳ���� ��������Ʈ��
    [SerializeField]
    private Text goldText;

    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast()); // ������ â  
        buyButton.onClick.AddListener(() => ViewManager.Show<PurchasePopUp>(true, true)); // ����â Ȱ��ȭ
        saveButton.onClick.AddListener(() => DataManager.Instance.SaveData<IDictionary<string, Contents.CharacterCustom>>(DataManager.Instance.CharacterCustomData, "TestJson")); ;
        goldText.text = $"GOLD : {DataManager.Instance.Gold}";

        for (int i = 0; i < selectCharaceterBtns.Count; i++)
        {
            int characterIndex = i; // ������ ĸ���ϱ� ������ i�� �ȳְ� ���÷� ���� ������ �Ҵ��ؼ� ���
            selectCharaceterBtns[i].onClick.AddListener(() => OnCharacterButtonClicked(characterIndex));
        }
    }

    void OnCharacterButtonClicked(int characterIndex)
    {
        if (characterChangeImage != null && characterSprites.Count > 0)
        {
            characterChangeImage.sprite = characterSprites[characterIndex];
        }
    }

}
