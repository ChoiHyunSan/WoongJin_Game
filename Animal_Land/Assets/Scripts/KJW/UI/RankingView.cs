using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingView : PopUpUI
{
    [SerializeField] private Button rankButton;
    [SerializeField] private Transform pScrollCotnet;
    [SerializeField] private GameObject Rank;
    [SerializeField] private List<Sprite> RankSprites;

    public override void Initialize()
    {
       base.Initialize();

        rankButton.onClick.AddListener(() => UpdateRanking());
    }

    void UpdateRanking()
    {
        OnRankingPannel();

       foreach (Transform child in pScrollCotnet)
        {
            Destroy(child.gameObject);
        }

        if (DatabaseManager.Instance != null)
        {
            int i = 0;
            foreach(var data in DatabaseManager.Instance.RankingList)
            {
                GameObject rank = Instantiate(Rank, pScrollCotnet);
                rank.transform.Find("Ranking").GetComponent<TextMeshProUGUI>().text = $"{i + 1}";
                rank.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = $"{data.Value}";
                rank.transform.Find("NickName").GetComponent<TextMeshProUGUI>().text = data.Key;

                Image topRank = rank.transform.Find("TopRank").GetComponent<Image>();
                if (i==0) // 1��
                {
                    topRank.gameObject.SetActive(true);
                    topRank.sprite = RankSprites[i];
                }
                else if(i==1)  // 2��
                {
                    topRank.gameObject.SetActive(true);
                    topRank.sprite = RankSprites[i];
                }
                else if (i==2) //3��
                {
                    topRank.gameObject.SetActive(true);
                    topRank.sprite = RankSprites[i];
                }
                i++;
            }
        }
        ViewManager.Show<RankingView>(true, true); // ��ŷ �˾� ǥ��
    }

    void OnRankingPannel()
    {
      
        DatabaseManager.Instance.ReadDB(DataType.Users);
        DatabaseManager.Instance.RankingList = DatabaseManager.Instance.RankingList.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        Debug.Log("���� ��");
        int i = 0;
        foreach (var data in DatabaseManager.Instance.RankingList)
        {
            Debug.Log($"{i + 1}, Nickname: {data.Key}" +
                      $" Score: {data.Value}");
            i++;
        }
    }
}
