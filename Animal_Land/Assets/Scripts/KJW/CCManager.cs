using Contents;
using System;
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
        CharacterCustom characterCustom = ShopManager.Instance.CharacterCustom;

        foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            Sprite tempSprite = Resources.Load<Sprite>($"Sprites/Items/{itemType}/{characterCustom.ItemDict[itemType.ToString()]}");
            if (tempSprite != null)
            {
                Parts[(int)itemType].sprite = tempSprite;
                Parts[(int)itemType].color = new Color(255, 255, 255, 255);
            }
            else
            {
                Parts[(int)itemType].sprite = null;
                 Parts[(int)itemType].color = new Color(0, 0, 0, 0);
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
            Parts[(int)type].color = new Color(255, 255, 255, 255);
        }
        else
        {
            Parts[(int)type].sprite = null;
            Parts[(int)type].color = new Color(0, 0, 0, 0);
        }
    }


    public void CharacterOriginPartsSprite()
    {
        CharacterType characterType = (CharacterType)Enum.Parse(typeof(CharacterType), DataManager.Instance.PlayerData.Character); // ��ǥ ĳ���� ���� 
        CharacterCustom characterCustom = DataManager.Instance.CharacterCustomData[characterType.ToString()];

        foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            Sprite tempSprite = Resources.Load<Sprite>($"Sprites/Items/{itemType}/{characterCustom.ItemDict[itemType.ToString()]}");
            if (tempSprite != null)
            {
                Parts[(int)itemType].sprite = tempSprite;
                Parts[(int)itemType].color = new Color(255, 255, 255, 255);
            }
            else
            {
                Parts[(int)itemType].sprite = null;
                Parts[(int)itemType].color = new Color(0, 0, 0, 0);
            }
        }
    }
}
