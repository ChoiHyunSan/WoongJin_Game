using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ð� �������� ����ü ������ ������. (�߹ٴ� ���)
// 

public class SBearAttack : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField] private float _attackCoolTime;
    [SerializeField] private float _attackMaxCoolTime;

    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackRange;

    [Header("����ü")]
    [SerializeField] private GameObject _attackObject;

    // Start is called before the first frame update
    void Start()
    {
        
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
        // ��.��.��.�� �߿� ���� �÷��̾�� ����� �������� ����ü�� �߻��Ѵ�.
        Vector3 dir = GetAttackDir();
        if(dir == null || dir == Vector3.zero)
        {
            return;
        }

        // ����ü ���� ��, �ʱ�ȭ
        GameObject attackObject = Instantiate(_attackObject);
        attackObject.transform.position = this.transform.position;

        SBearAttackSkill sBearAttackSkill = attackObject.GetComponent<SBearAttackSkill>();
        if (sBearAttackSkill != null)
        {
            sBearAttackSkill.SetDir(dir);
        }
    }

    Vector3 GetAttackDir()
    {
        // ���ư� ���� ���
        Vector3 myPos = transform.position;
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        int randNum = Random.Range(0, 4);
        switch(randNum)
        {
            case 0:
                return Vector3.up;
            case 1:
                return Vector3.down;
            case 2:
                return Vector3.right;
            case 3:
                return Vector3.left;
        }

        return Vector3.zero;
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
