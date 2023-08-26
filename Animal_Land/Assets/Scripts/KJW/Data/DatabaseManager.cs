using Firebase;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

    public string USER_NAME { get; private set; } = string.Empty;
    public static DatabaseManager Instance => instance;
    public Dictionary<string,int> RankingList = new Dictionary<string, int>(); // ���� ��ŷ ����Ʈ ��ųʸ�
    public DatabaseReference reference { get; set; }
    [SerializeField] private string DB_URL = "https://animalland-d2718-default-rtdb.firebaseio.com"; // DB URL

    private static DatabaseManager instance = null;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(instance);
        InitDBM();
    }

    async Task InitDBM()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync(); // �ĺ��� �ʿ��� ��� ���� �׸�� ���� ���� Ȯ��
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.Options.DatabaseUrl = new Uri(DB_URL); // URL ����
        reference = FirebaseDatabase.DefaultInstance.RootReference; // ��Ʈ�� ����

        await ReadDB(DataType.Users); // �ҷ��� ������ ��ٸ�
        await WriteDB();
    }


    public async Task WriteDB(int score = 0)
    {
        string uId = this.GetComponent<WJ_Connector>().UserID; // ���� API ���̵�

        reference = FirebaseDatabase.DefaultInstance.RootReference; // DB ��Ʈ�� �ʱ�ȭ

        if (USER_NAME == string.Empty)
        {
            USER_NAME = "Player" + uId.Substring(16, 4);
        }

        int saveTotalScore = score;

        if (RankingList.ContainsKey(USER_NAME)) // �̹� DB�� �ִ��� �Ǵ�
        {
            saveTotalScore = RankingList[USER_NAME] + saveTotalScore;
        }

        Rank rankDate = new Rank(USER_NAME, saveTotalScore);
        string jsonData = JsonUtility.ToJson(rankDate);
        await reference.Child("Users").Child(uId).SetRawJsonValueAsync(jsonData);

        await ReadDB(DataType.Users); // ������ �ٽ� ����
    }

    public async Task ReadDB(DataType root) // DB �о���� �޼ҵ�
    { 
        reference = FirebaseDatabase.DefaultInstance.GetReference(root.ToString());

        try
        {
            switch(root)
            {
                case DataType.Users: // ���� ����
                   await ReadUsersData();
                    break;
                case DataType.ItemData: // ���� ������ ����
                    await ReadItemData();
                    break;
                case DataType.CustomData: // ���� ó�� �����̶�� Ŀ���� ��� json ���� ���� �޾ƿͼ� ����
                    await ReadCustomData();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"������ ���̽��� �дµ� �����߽��ϴ�.: {e.Message}");
        }
    }

    async Task ReadUsersData()
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
        }
        catch (Exception e)
        {
            Debug.LogError($"���� �����͸� �дµ� �����߽��ϴ�.: {e.Message}");
        }
    }

    async Task ReadItemData()
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

    async Task ReadCustomData()
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


