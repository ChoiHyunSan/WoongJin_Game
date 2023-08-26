using Firebase;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Rank
{
    public Rank() { }
    public Rank(string name, int score)
    {
        this.Name = name;
        this.Score = score;
    }

    public string Name; // �г��� ����
    public int Score; // ����
}

public enum DataType // ������ Ÿ�� enum
{
    Users,
    ItemData,
    CustomData
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance => instance;
    public bool isReadDB = true;
    public Dictionary<string,int> RankingList = new Dictionary<string, int>(); // ���� ��ŷ ����Ʈ ��ųʸ�
    public DatabaseReference reference { get; set; }
    [SerializeField] private string DB_URL = "https://animalland-d2718-default-rtdb.firebaseio.com"; // DB URL

    private static DatabaseManager instance = null;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(instance);
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(DB_URL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        WriteDB();
        ReadDB(DataType.Users);
    }

    void WriteDB() // TODO : �ڱ� �ڽ��� ���̵�� �г������� ������ �� �ִ� �Լ� ����
    {
        string uId = this.GetComponent<WJ_Connector>().UserID;

        Rank rankDate = new Rank("Player" + uId.Substring(16, 4), 0);

        string jsonData = JsonUtility.ToJson(rankDate);
        reference.Child("Users").Child(uId).SetRawJsonValueAsync(jsonData);

        //string js = JsonConvert.SerializeObject(DataManager.Instance.CharacterCustomData);
        //reference.Child("CustomData").SetRawJsonValueAsync(js);
    }

    public void ReadDB(DataType root) // DB �о���� �޼ҵ�
    { 
        reference = FirebaseDatabase.DefaultInstance.GetReference(root.ToString());

        try
        {
            switch(root)
            {
                case DataType.Users: // ���� ����
                    ReadUsersData();
                    break;
                case DataType.ItemData: // ���� ������ ����
                     ReadItemData();
                    break;
                case DataType.CustomData: // ���� ó�� �����̶�� Ŀ���� ��� json ���� ���� �޾ƿͼ� ����
                    ReadCustomData();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"������ ���̽��� �дµ� �����߽��ϴ�.: {e.Message}");
        }
    }

    async void ReadUsersData()
    {
        try
        {
            DataSnapshot snapshot = await reference.GetValueAsync();

            foreach (DataSnapshot data in snapshot.Children)
            {
                Rank userInfo = JsonUtility.FromJson<Rank>(data.GetRawJsonValue());
                if (RankingList.ContainsKey(userInfo.Name))
                {
                    RankingList[userInfo.Name] = userInfo.Score;
                }
                else
                {
                    RankingList.Add(userInfo.Name, userInfo.Score);
                }
            }
            isReadDB = false;
        }
        catch (Exception e)
        {
            Debug.LogError($"���� �����͸� �дµ� �����߽��ϴ�.: {e.Message}");
        }
    }

    async void ReadItemData()
    {
        try
        {
            DataSnapshot dataSnapshot = await reference.GetValueAsync();
            string jsonData = dataSnapshot.GetRawJsonValue();
            string savePath = Path.Combine(Application.persistentDataPath, $"Data/{DataType.ItemData.ToString()}.json");
            File.WriteAllText(savePath, jsonData);
            DataManager.Instance?.ReloadData();
        }
        catch (Exception e)
        {
            Debug.LogError($"������ �����͸� �дµ� �����߽��ϴ�.: {e.Message}");
        }
    }

    async void ReadCustomData()
    {
        try
        {
            DataSnapshot dataSnapshot = await reference.GetValueAsync();
            string jsonData = dataSnapshot.GetRawJsonValue();
            string savePath = Path.Combine(Application.persistentDataPath, $"Data/{DataType.CustomData.ToString()}.json");
            File.WriteAllText(savePath, jsonData);
            DataManager.Instance?.ReloadData();
        }
        catch (Exception e)
        {
            Debug.LogError($"������ �����͸� �дµ� �����߽��ϴ�.: {e.Message}");
        }
    }
}


