using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCharacterMove : MonoBehaviour
{
    SCharacter character;
    Vector3 curPos;

    void Update()
    {
        BorderCheck();

        // TODO : CurPos�� üũ�Ͽ� �̵����� ��쿡�� �̵� �������� ���ҽ�Ų��.
        if(curPos != transform.position)
        {
            curPos = transform.position;
            character.ConsumeMoveGauge();
        }
    }

    public void Awake()
    {
        character = GetComponent<SCharacter>();

        // �ʱ� ��ġ�� CurPos �ʱ�ȭ
        curPos = transform.position;
    }

    private void BorderCheck()
    {
        // Game Screen�� �ִ� ũ��
        Vector3 lt = new Vector3(-8.05f, 2.28f, 0);
        Vector3 rb = new Vector3(3.59f, -4.23f, 0);

        Vector3 newPos = transform.position;

        if (transform.position.x < lt.x)
        {
            newPos = new Vector3(lt.x, transform.position.y, 0);
        }
        else if (transform.position.x > rb.x)
        {
            newPos = new Vector3(rb.x, transform.position.y, 0);
        }
        else if (transform.position.y > lt.y)
        {
            newPos = new Vector3(transform.position.x, lt.y, 0);
        }
        else if (transform.position.y < rb.y)
        {
            newPos = new Vector3(transform.position.x, rb.y, 0);
        }
        else
        {
            return;
        }
        transform.position = newPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(6, 6);
    }

}
