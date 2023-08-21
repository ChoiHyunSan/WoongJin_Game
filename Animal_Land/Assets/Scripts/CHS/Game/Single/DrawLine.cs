using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField]
    public GameObject Prefab_line;

    [SerializeField]
    public SpriteRenderer DrawingPaper;

    LineRenderer lr;
    List<Vector2> List_Points;
    Stack<GameObject> Stack_Line;   // �ǵ����� ����� ���� ������Ʈ�� Stack���� ����

    private void Start()
    {
        List_Points = new List<Vector2>();
        if (List_Points == null)
        {
            Debug.LogError("List_Points�� �������� �ʾҽ��ϴ�.");
        }

        Stack_Line = new Stack<GameObject>();
        if (Stack_Line == null)
        {
            Debug.LogError("Stack_Line�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        if (DetectHoverDrawingPaper() == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Draw Start");
            // ���� ��ġ���� ����
            GameObject go = Instantiate(Prefab_line);

            if (go == null)
            {
                Debug.LogError("Prefab_Line�� �������� ���Ͽ����ϴ�.");
                return;
            }

            lr = go.GetComponent<LineRenderer>();

            // �Էµ� ���콺�� ��ġ���� ����Ʈ�� �߰�
            List_Points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // Line Renderer�� ī��Ʈ�� ������Ű�� �׸��� �׸���.
            lr.positionCount = 1;
            lr.SetPosition(0, List_Points[0]);

            // ���ÿ� Line�� �߰��Ѵ�.
            Stack_Line.Push(go);
        }
        else if (Input.GetMouseButton(0))
        {
            Debug.Log("Drawing");
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // �߰������� Line Renderer�� �̿��Ͽ� �׸���.
            List_Points.Add(pos);

            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, pos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Draw end");
            List_Points.Clear();
        }
    }

    private bool DetectHoverDrawingPaper()
    {
        // ���콺�� ��ġ�� ��ũ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ����ĳ��Ʈ�� ���� �浹 ���� Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);

        // �浹�� ��������Ʈ�� �ִ��� Ȯ��
        if (hit.collider != null)
        {
            return hit.collider.gameObject.tag == "paper" ? true : false;
        }
        else
        {
            return false;
        }
    }

    public void RevertLine()
    {
        if (Stack_Line == null || Stack_Line.Count == 0) return;

        // Stack���� ������ �޾ƿͼ� Destroy
        GameObject line = Stack_Line.Pop();

        if (line == null) return;

        GameObject.Destroy(line);
    }

    public void ClearLIne()
    {
        if (Stack_Line == null || Stack_Line.Count == 0) return;

        while (Stack_Line.Count > 0)
        {
            RevertLine();
        }
    }
}