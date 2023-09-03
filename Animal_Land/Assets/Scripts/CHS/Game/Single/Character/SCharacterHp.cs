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
    private bool _isDead = false;
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
        if (_isDead)
            return;

        float newHp = _hp - damage;
        if (newHp <= 0)
        {
            newHp = 0;
            _isDead = true;
            // TODO : �÷��̾ �׾��ٴ� �Ϳ� ���� �۾� ó��
            GameObject.Find("GameManager").GetComponent<SGameManager>().EndGame(false);
        }
        _hp = newHp;
        Debug.Log(_hp);

        // ȿ���� ���
        GameObject.Find("UIManager").GetComponent<SoundManager>().PlayEffect(Effect.Punch);

        // TODO : UI�� �����Ѵ�.
        float value = _hp / _maxHp;
        gameUIManager.UpdateHp(value);
    }
}
