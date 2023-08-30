using Contents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//CharacterCustomManager
public class CCManager : MonoBehaviour
{
    [SerializeField] List<Image> Parts;


    /// <summary>
    /// ĳ������ ��� ������ Ÿ���� �̹��� ����
    /// </summary>
    public void ChangeCharacterPartsSprite()
    {
        Sprite temp = Resources.Load<Sprite>
            ($"Sprites/Items/Face/{ShopManager.Instance.CharacterCustom.ItemDict[ItemType.Face.ToString()]}");
        Parts[(int)ItemType.Face].sprite = temp;
        //ShopManager.Instance.CharacterCustom.ItemDict[ItemType.Hat.ToString()]
        //ShopManager.Instance.CharacterCustom.ItemDict[ItemType.Glasses.ToString()]
        //ShopManager.Instance.CharacterCustom.ItemDict[ItemType.Etc.ToString()]

        CharacterCustom characterCustom = ShopManager.Instance.CharacterCustom;

        foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            Sprite tempSprite = Resources.Load<Sprite>($"Sprites/Items/{itemType}/{characterCustom.ItemDict[itemType.ToString()]}");
            if(tempSprite != null ) 
            {
            Parts[(int)itemType].sprite = tempSprite;
            }
            else
            {
                Parts[(int)itemType].color = new Color(0,0,0,0);
            }
        }
    }

    /// <summary>
    /// ĳ������ Ư�� ������ �̹����� ����
    /// </summary>
    /// <param name="type"></param>

    public void ChangeCharacterSpecificPartsSprite(ItemType type)
    {
        CharacterCustom characterCustom = ShopManager.Instance.CharacterCustom;
        Sprite tempSprite = Resources.Load<Sprite>($"Sprites/Items/{type}/{characterCustom.ItemDict[type.ToString()]}");
        if (tempSprite != null)
        {
            Parts[(int)type].sprite = tempSprite;
        }
        else
        {
            Parts[(int)type].color = new Color(0, 0, 0, 0);
        }
    }
}
