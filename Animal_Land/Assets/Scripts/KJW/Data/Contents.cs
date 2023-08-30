using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contents
{
    public enum ItemType
    {
        Face,
        Hat,
        Glasses,
        Etc
    }

    public enum CharacterType
    {
        Bird,
        Cat,
        Dog,
        Frog
    }

    public enum StatType
    {
        SPEED,
        HP,
        ENERGY
    }

    public class CharacterCustom
    {
        public CharacterCustom(CharacterCustom characterCustom)
        {
            this.ItemDict = new Dictionary<string, string>(characterCustom.ItemDict);
        }
        public CharacterCustom() { }
        [JsonProperty("CustomList")]
        public IDictionary<string, string> ItemDict { get; set; } = new Dictionary<string, string>(); // Key : ������ Ÿ��, Vlaue : ������ �̸�
    }

    public class PlayerData
    {
        [JsonProperty("Character")]
        public string Character { get; set; } = "Bird";
        [JsonProperty("Gold")]
        public int Gold { get; set; } = 0;
        [JsonProperty("ShoppingList")]
        public IDictionary<string, bool> ShoppingList { get; set; } = new Dictionary<string, bool>();
    }

    public class ItemInfo
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Price")]
        public int Price { get; set; }
    }

    public class PlayerStat
    {
        private const int MAX_PURCHASE = 1;

        private int _hp = 0;
        private int _energy = 0; // Key�� ���� Ƚ��, Value�� ���� ü��
        private int _speed = 0;

        public int HP
        {
            get => _hp;
            set => _hp = Math.Clamp(value, 0, MAX_PURCHASE);
        }

        public int Energy
        {
            get => _energy;
            set => _energy = Math.Clamp(value, 0, MAX_PURCHASE);
        }

        public int Speed
        {
            get => _speed;
            set => _speed = Math.Clamp(value, 0, MAX_PURCHASE);
        }

        public bool CheckForPurchase(int index)
        {
            switch (index)
            {
                case 0:
                    if (_speed >= MAX_PURCHASE)
                    {

#if UNITY_EDITOR
                        Debug.LogWarning("�� �̻� SPEED�� ������ �� �����ϴ�.");
#endif  
                        return false;
                    }
                    break;

                case 1:
                    if (_hp >= MAX_PURCHASE)
                    {

#if UNITY_EDITOR
                        Debug.LogWarning("�� �̻� HP�� ������ �� �����ϴ�.");
#endif
                        return false;
                    }
                    break;
                case 2:
                    if (_energy >= MAX_PURCHASE)
                    {

#if UNITY_EDITOR
                        Debug.LogWarning("�� �̻� ENERGY�� ������ �� �����ϴ�.");
#endif
                        return false;
                    }
                    break;
            }
            return true;
        }
    }

}