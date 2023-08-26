using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLionAttack : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField] private float _attackCoolTime;
    [SerializeField] private float _attackMaxCoolTime;

    [SerializeField] private float _attackTime;         // ��ų ���ӽð�

    [Header("��ų")]
    [SerializeField] private GameObject _attackObject;

    SMonsterMove monsterMove;

    // Start is called before the first frame update
    void Start()
    {
        monsterMove = GetComponent<SMonsterMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCoolTime())
        {
            Attack();
        }
    }

    void Attack()
    {
        // ��ų Ȱ��ȭ  &&  ������ ����
        _attackObject.SetActive(true);
        monsterMove.SetCanMove(false);

        // ���� �ð� ���� ��ų ����
        Invoke("SkillEnd", _attackTime);
    }

    void SkillEnd()
    {
        // ��ų ��Ȱ��ȭ && ������ Ȱ��ȭ
        _attackObject.SetActive(false);
        monsterMove.SetCanMove(true);
    }

    bool CheckCoolTime()
    {
        if (_attackCoolTime <= _attackMaxCoolTime)
        {
            _attackCoolTime += Time.deltaTime;
            return false;
        }
        else
        {
            _attackCoolTime = 0f;
            return true;
        }
    }
}
