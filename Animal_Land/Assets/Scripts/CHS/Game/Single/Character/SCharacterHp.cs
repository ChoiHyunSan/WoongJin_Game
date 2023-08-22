using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� HP�� �����ϴ� ��ũ��Ʈ
// �÷��̾��� �ǰݿ� ���� ó���� ����Ѵ�.

public class SCharacterHp : MonoBehaviour
{
    [SerializeField] private float _hp = 100f;
    [SerializeField] private float _maxHp = 100f;

    private SGameUIManager gameUIManager;

    void Start()
    {
        // TODO : ���� UI���� ĳ���� HP�ٿ� ����
        gameUIManager = GameObject.Find("UIManager").GetComponent<SGameUIManager>();
    }

    void Update()
    {

    }

    public float GetHp()
    {
        return _hp;
    }

    public void Heal(float val)
    {
        float newHp = _hp + val;
        if(newHp > _maxHp)
        {
            newHp = _maxHp;
        }

        _hp = newHp;

        // TODO : UI�� �����Ѵ�.
        float value = _hp / _maxHp;
        gameUIManager.UpdateHp(value);
    }

    public void Damage(float damage)
    {
        float newHp = _hp - damage;
        if (newHp <= 0)
        {
            newHp = 0;

            // TODO : �÷��̾ �׾��ٴ� �Ϳ� ���� �۾� ó��
            GameObject.Find("GameManager").GetComponent<SGameManager>().EndGame(false);
        }
        _hp = newHp;
        Debug.Log(_hp);

        // TODO : UI�� �����Ѵ�.
        float value = _hp / _maxHp;
        gameUIManager.UpdateHp(value);
    }
}
