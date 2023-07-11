using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;           // ����Ƽ�� ���� ������Ʈ
using Photon.Realtime;      // ���� ���� ���� ���̺귯��
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviourPunCallbacks
{
    [Header("�Ŵ��� ��ũ��Ʈ")]
    public TitleUIManager   titleUIManager; 
    public UserManager      userManager;


    // ���� ����� ���ÿ� ���� ���� �ҷ�����
    private void Start()
    {
        if (LoadData() == true)
        {
            // TODO : �ҷ��� �����Ϳ��� �����򰡸� �ߴ��� �Ǵ��ϰ� ��ư�� ��ģ��.
            bool check = IsAlreadyTest();
            titleUIManager.SetCanMoveToLobby(check);

        }
        else
        {
            Debug.LogError("���� ������ �ҷ����� ���߽��ϴ�.");
        }
    }

    private bool LoadData()
    {
        bool result = false;
        // TODO : Ŭ���̾�Ʈ�� ����� ���� �ҷ��´�.

        // �ҷ����µ��� �����ߴٸ� ��ư�� Ȱ��ȭ ��Ų��.        
        result = true;

        return result;
    }

    private bool IsAlreadyTest()
    {
        // TODO : ���� �򰡸� �ߴ��� �ľ��ϴ� �Լ��� �ۼ�

        return true;
    }

    private bool CheckPlayerAlreadyTest()
    {
        bool result = false;
        // TODO : �����Ϳ��� ���� �����ͼ� �Ǵ��Ѵ�.

        // ���� �򰡸� ������ �̷��� �ִٸ� �κ�� �̵��Ѵ�.
        result = true;

        return result;
    }

    
}
