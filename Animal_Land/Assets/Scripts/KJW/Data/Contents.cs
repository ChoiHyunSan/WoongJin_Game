using Newtonsoft.Json;
using System.Collections.Generic;

namespace Contents
{
    public enum ItemType
    {
        Face,
        Hat,
        Necklace,
        Glass,
        Wing
    }

    public enum CharacterType
    {
        Frog,
        Bird,
        Dog,
        Cat
    }

    public class CharacterCustom
    {
        [JsonProperty("CustomList")]
        public IDictionary<string, string> ItemDict { get; set; } = new Dictionary<string, string>(); // Key : ������ Ÿ��, Vlaue : ������ �̸�
    }

    public class PlayerData
    {
        [JsonProperty("Gold")]
        public int Gold { get; set; }
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

}
