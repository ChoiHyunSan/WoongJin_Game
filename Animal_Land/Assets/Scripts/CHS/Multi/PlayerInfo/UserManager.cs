using Contents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    private static UserManager instance = null;

    // �� 4���� ������ ĳ������ ������ ������ �ִ�
    private CharacterInfo[] userInfos = new CharacterInfo[4];
    public CharacterDefaultInfo[] CharacterDefaultInfos = new CharacterDefaultInfo[4];

    public PlayerData PlayerData { get; private set; } = null; // ��ȭ,������ ���� ����Ʈ

    // TODO : Ŭ���̾�Ʈ�� ����Ǿ� �ִ� ���� �ַ��´�.
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        LoadUserInfo();
    }

    void LoadUserInfo()
    {
        // �÷��̾� ���� ������ 

        PlayerData playerData = DataManager.Instance.PlayerData;
        if (playerData != null)
        {
            this.PlayerData = playerData;
        }

        //// ĳ���� ���� �ε�
        //LoadCharcterInfo();

        //// ���� ���� �ε�
        //LoadStoreInfo();

        //// ��ȭ �ε�
        //LoadCreditInfo();
    }

    void LoadCharcterInfo()
    {
        // ĳ������ ������ ������ �´�.

    }

    void LoadStoreInfo()
    {
        // ���� ������ ������ �´�.

    }

    void LoadCreditInfo()
    {
        // ��Ÿ(��ȭ ��)�� ������ ������ �´�.

    }

    public CharacterInfo GetCharcterInfo(int playerCount)
    {
        return userInfos[playerCount - 1];
    }

    public CharacterDefaultInfo GetCharcterDefaultInfo(int playerCount)
    {
        CharacterDefaultInfo characterDefaultInfo = CharacterDefaultInfos[playerCount - 1];
        if (characterDefaultInfo != null)
        {
            return characterDefaultInfo;
        }
        else
        {
            Debug.Log("characterDefaultInfo is null");
            return null;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
