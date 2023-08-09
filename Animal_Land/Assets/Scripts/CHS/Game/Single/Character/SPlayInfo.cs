using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


// ���� �÷��� �� �ʿ��� ������ ������ �� �ִ� ��ũ��Ʈ
// ĳ���� �����տ� ������Ʈ�� �߰��Ͽ� ���� ���� ������ �����Ѵ�.

public class SPlayInfo : MonoBehaviourPunCallbacks
{
    private Vector2 _curDir;                // ���� �÷��̰� ���� ����
    private Vector2 _prevDir;               // ������ �÷��̾ �� ����
    private Vector2 _prevprevDir;           // ������ ������ �÷��̾ �� ����

    public List<Vector2> _moveTileList;          // �÷��̾ �ڽ��� ���� �ۿ��� �̵��ϸ鼭 ������ �ٲ� Ÿ�ϵ��� ����Ʈ
    public List<Color> _moveTileColorList;     // moveTileList�� �������� �ٲ�� ������� ����Ʈ 

    private void Awake()
    {
        _curDir = Vector2.zero;
        _prevDir = Vector2.zero;
        _prevprevDir = Vector2.zero;

        _moveTileColorList = new List<Color>();
        _moveTileList = new List<Vector2>();
    }

    void Start()
    {

    }

    [PunRPC]
    public void SetCurDir(Vector2 dir)
    {
        if (dir == _curDir)
        {
            return;
        }

        SetPrevDIR(_curDir);
        _curDir = dir;
    }

    public void SetPrevDIR(Vector2 dir)
    {
        SetPrevPrevDIR(_prevDir);
        _prevDir = dir;
    }

    private void SetPrevPrevDIR(Vector2 dir)
    {
        _prevprevDir = dir;
    }

    public Vector2 GetCurDir()
    {
        return _curDir;
    }

    public Vector2 GetPrevDir()
    {
        return _prevDir;
    }

    public Vector2 GetPrevPrevDir()
    {
        return _prevprevDir;
    }

    public List<Color> GetMoveTileColorList()
    {
        return _moveTileColorList;
    }

    public List<Vector2> GetMoveTileList()
    {
        return _moveTileList;
    }
    public void AddMoveTileColorList(Vector3Int Pos, Color color)
    {
        _moveTileColorList.Add(color);
    }

    public void AddMoveTileList(Vector3Int cellPosition, Color color)
    {
        Vector2 pos = new Vector2(cellPosition.x, cellPosition.y);

        if (_moveTileList.Count > 0)
        {
            Vector2 prevPos = _moveTileList[_moveTileList.Count - 1];
            if (prevPos.x == pos.x && prevPos.y == pos.y)
            {
                return;
            }
        }
        _moveTileList.Add(pos);
        AddMoveTileColorList(cellPosition, color);
    }

    public void ClearMoveTileList()
    {
        _moveTileList.Clear();
        _moveTileColorList.Clear();
    }
}
