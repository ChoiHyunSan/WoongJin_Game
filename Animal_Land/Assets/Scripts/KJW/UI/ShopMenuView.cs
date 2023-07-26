using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuView : View
{
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private List<Button> selectCharaceterBtn;
    [SerializeField]
    private Image characterChangeImage;
    [SerializeField]
    private List<Sprite> characterSprites;

    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowList()); // ������ â  

        for (int i = 0; i < selectCharaceterBtn.Count; i++)
        {
            int characterIndex = i; // ������ ĸ���ϱ� ������ i�� �ȳְ� ���÷� ���� ������ �Ҵ��ؼ� ���
            selectCharaceterBtn[i].onClick.AddListener(() => OnCharacterButtonClicked(characterIndex)); 
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
