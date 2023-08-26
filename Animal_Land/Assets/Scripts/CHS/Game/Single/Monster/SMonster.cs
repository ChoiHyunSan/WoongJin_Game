using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SMonster : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private int _monsterScore = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AreaCheck()
    {
        Tilemap tilemap = GetComponent<SMonsterMove>().GetTilemap();
        if(tilemap != null)
        {
            Vector3Int pos = tilemap.WorldToCell(this.transform.position);
            if(tilemap.GetColor(pos) == GetComponent<SMonsterMove>().GetPlayerAreaColor())
            {
                Dead();
            }
        }
    }

    private void Dead()
    {
        // ���� �߰�
        GameObject.Find("GameManager").GetComponent<SGameManager>().AddMonsterScore(_monsterScore);
        
        // ������ ���
        this.GetComponent<SDropItem>().DropItem();
        
        GameObject.Destroy(this.gameObject);
    }

    public void SetMonster(bool val)
    {
        GetComponent<SMonsterMove>().enabled = val;
        
        // �� ��ų Ȱ��ȭ ����
        if(GetComponent<SBearAttack>() != null)
        {
            GetComponent<SBearAttack>().enabled = val;
        }
        
        // TODO : ���� ��ų Ȱ��ȭ ����
    }

    public void StopMonster()
    {
        SetMonster(false);
    }

    public void StartMonster()
    {
        SetMonster(true);
    }
}
