using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDropItem : MonoBehaviour
{
    [SerializeField] private GameObject[] Items = new GameObject[4];

    public void DropItem()
    {
        // �������� ������ ����
        int rand = Random.Range(0, Items.Length);

        // ������ ���� �� ��ġ
        GameObject Item = Instantiate(Items[rand]);
        Item.transform.position = this.transform.position;
    }
}
