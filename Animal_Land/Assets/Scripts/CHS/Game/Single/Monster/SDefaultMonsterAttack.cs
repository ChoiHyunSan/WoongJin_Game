using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���Ͱ� �⺻������ ���ϴ� ���� ���
// �ش� ��ũ��Ʈ�� ������ ���Ϳ� �ε��� �÷��̾�� ü���� �Ұ� ���� ��ġ�� ���ư���.

public class SDefaultMonsterAttack : MonoBehaviour
{
    private float _attackDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            // TODO : �÷��̾�� �������� �ش�.
            other.GetComponent<SCharacterHp>().Damage(_attackDamage);

            // TODO : �÷��̾ ���� ��ġ�� ������.
            other.GetComponent<STileColorChange>().ResetMoveTileList();
        }
    }
}
