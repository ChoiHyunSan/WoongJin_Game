using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� HP�� �����ϴ� ��ũ��Ʈ
// �÷��̾��� �ǰݿ� ���� ó���� ����Ѵ�.

public class SCharacterHp : MonoBehaviour
{
    private float _hp = 100f;

    void Start()
    {
        // TODO : ���� UI���� ĳ���� HP�ٿ� ����

    }

    void Update()
    {

    }

    public void Damage(float damage)
    {
        float newHp = _hp - damage;
        if (newHp <= 0)
        {
            newHp = 0;

            // TODO : �÷��̾ �׾��ٴ� �Ϳ� ���� �۾� ó��
        }
        _hp = newHp;
        Debug.Log(_hp);

        // TODO : UI�� �����Ѵ�.
        
    }
}
