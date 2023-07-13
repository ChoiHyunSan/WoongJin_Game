using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColorChange : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Tile Color ����")]
    [SerializeField]
    private Color _moveColor;
    [SerializeField]
    private Color _areaColor;
    [SerializeField]
    private Color _borderColor;

    Vector3Int      _prevPos;
    Tilemap         _tilemap;
    PhotonView      _pv;

    Tilemap         _curTilemap;
    PlayInfo        _playInfo;
    Vector3Int      _startPosition;         // moveTileList�� ������ ��ġ

    int test;
    void Awake()
    {
        PhotonNetwork.SendRate = 30;            // 5��/�ʷ� �޽����� ����
        PhotonNetwork.SerializationRate = 60;   // 5��/�ʷ� �޽����� ���� �� ������Ʈ
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO : ��� ���� �ʱ�ȭ
        _prevPos = new Vector3Int(10000, 10000, 0);
        _tilemap = new Tilemap();
        _pv = GetComponent<PhotonView>();
        _playInfo = GetComponent<PlayInfo>();

        Vector3Int cellPosition = GetTileMap().WorldToCell(transform.position);
        UpdateTileColorToArea(new int[3] { cellPosition.x, cellPosition.y, cellPosition.z});
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �ʱ�ȭ�� �̷����� �ʾҴٸ� return
        if(IsInit())
        {
            return;
        }

        // ������ Ŭ���̾�Ʈ������ Ȯ���� �����Ѵ�.
        // ������ Ŭ���̾�Ʈ�� ������ Ŭ���̾�Ʈ�� ��Ȳ�� �����޾� �÷����Ѵ�.
        if (PhotonNetwork.IsMasterClient == true)
        {
            CheckTile();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient == true && _pv.IsMine == true)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckTile()
    {

        Vector3Int cellPos = GetTileMap().WorldToCell(transform.position);
        TileBase tile = GetTileMap().GetTile(cellPos);

        if (tile == null)
        {
            Debug.Log("tile is Null");
            return;
        }

        // ������ ��ġ��� ��ȯ�Ѵ�.
        if (_prevPos.x == cellPos.x && _prevPos.y == cellPos.y)
        {
            return;
        }

        // TODO : �׽�Ʈ�� ���� �������� Ÿ���� ������ �ڽ��� �������� ��ȯ�Ѵ�.
        
        // �ڽ��� ������ ���Դٸ�
        if(GetTileMap().GetColor(cellPos) == _areaColor)
        {
            // ���� ��ġ�� ���� �ȿ� ��ġ�Ѵٸ�
            if (isInMoveTileList(new Vector3Int(_prevPos.x, _prevPos.y, _prevPos.z)) == false)
            {
                UpdatePrevPos(cellPos);
                UpdateStartPos(cellPos);
                return;
            }
            // ���� ��ġ�� ���� ���� �ƴ϶��
            else
            {
                // ���� ĥ���ش�.
                ChangeMoveLIstTile(cellPos);
                FillArea(cellPos);
                _playInfo.ClearMoveTileList();

                UpdatePrevPos(cellPos);
                return;
            }
        }
        // MoveTile�� ���
        else if(GetTileMap().GetColor(cellPos) == _moveColor)
        {


            return;
        }
        // �� ���� Ÿ���� ���
        else
        {
            Color color = _tilemap.GetColor(cellPos);
            _playInfo.AddMoveTileList(cellPos, color);
            UpdateTileColorToMove(new int[3] {cellPos.x,cellPos.y,cellPos.z});

            // ���� ����������� ��ĭ �� ���� ��, �ڽ��� ������ ��� �ٸ� ä��� �õ�
            Vector3Int nextPos = FindNextPos(cellPos);
            if(GetTileMap().GetColor(nextPos) == _areaColor)
            {
                ChangeMoveLIstTile(cellPos);
                FillArea(cellPos);
                _playInfo.ClearMoveTileList();
            }

            UpdatePrevPos(cellPos);
            return;
        }
    }


    private bool isInMoveTileList(Vector3Int cellPosition)
    {
        List<Vector2> list = _playInfo.GetMoveTileList();

        foreach (Vector2 pos in list)
        {
            // ���� �ۿ��� �̵� ���� ��θ� �ٽ� ������ ���
            if (pos.x == cellPosition.x && pos.y == cellPosition.y)
            {
                return true;
            }
        }
        return false;
    }


    // RPC �Լ���
    [PunRPC]
    private void UpdateTileColorToMoveRPC(int[] pos)
    {
        Vector3Int cellPos = new Vector3Int(pos[0], pos[1], pos[2]);
        GetTileMap().SetTileFlags(cellPos, TileFlags.None);
        GetTileMap().SetColor(cellPos, new Color(_moveColor[0], _moveColor[1], _moveColor[2], _moveColor[3]));
    }

    [PunRPC]
    private void UpdateTileColorToAreaRPC(int[] pos)
    {
        Vector3Int cellPos = new Vector3Int(pos[0], pos[1], pos[2]);
        GetTileMap().SetTileFlags(cellPos, TileFlags.None);
        GetTileMap().SetColor(cellPos, new Color(_areaColor[0], _areaColor[1], _areaColor[2], _areaColor[3]));
    }

    // Update Tile Color Functions
    private void UpdateTileColorToMove(int[] pos)
    {
        _pv.RPC("UpdateTileColorToMoveRPC", RpcTarget.All, pos);
    }

    private void UpdateTileColorToArea(int[] pos)
    {
        _pv.RPC("UpdateTileColorToAreaRPC", RpcTarget.All, pos);
    }

    [PunRPC]
    private void UpdateTileColorToThisColor(int[] pos, float[] colors)
    {
        Vector3Int cellPos = new Vector3Int(pos[0], pos[1], pos[2]);
        GetTileMap().SetTileFlags(cellPos, TileFlags.None);
        GetTileMap().SetColor(cellPos, new Color(colors[0], colors[1], colors[2], colors[3]));
    }

    [PunRPC]
    private void UpdateTileColor(int[] pos, string colorName)
    {
        // TODO : Color �������� �Լ��� ���� ���� �ӵ� �÷��� �ҵ�

        Color color = new Color();
        if(colorName == "Move")
        {
            color = _moveColor;
        }
        else if(colorName == "Area")
        {
            color = _areaColor;
        }
        else if(colorName == "Border")
        {
            color = _borderColor;
        }
        Vector3Int cellPos = new Vector3Int(pos[0], pos[1], pos[2]);
        GetTileMap().SetTileFlags(cellPos, TileFlags.None);
        GetTileMap().SetColor(cellPos, new Color(color[0], color[1], color[2], color[3]));
    }

    // Color ���� �Լ���
    public void SetMoveColor(float[] colorArray)
    {
        Color color = new Color(colorArray[0], colorArray[1], colorArray[2]);
        color.a = colorArray[3];
        _moveColor = color;
    }
    public void SetAreaColor(float[] colorArray)
    {
        Color color = new Color(colorArray[0], colorArray[1], colorArray[2]);
        color.a = colorArray[3];
        _areaColor = color;
    }
    public void SetBorderColor(float[] colorArray)
    {
        Color color = new Color((byte)colorArray[0], (byte)colorArray[1], (byte)colorArray[2]);
        color.a = colorArray[3];

        _borderColor = color;
    }

    // Pos ���� �Լ���
    private void UpdateStartPos(Vector3Int cellPos)
    {
        _startPosition = cellPos;
    }

    private void UpdatePrevPos(Vector3Int cellPos)
    {
        _prevPos = cellPos;
    }

    private Vector3Int FindNextPos(Vector3Int cellPosition)
    {
        int dx = 0;
        int dy = 0;

        Vector2 dir = _playInfo.GetCurDir();

        if (dir == Vector2.up)
        {
            dx = 0; dy = 1;
        }
        else if (dir == Vector2.down)
        {
            dx = 0; dy = -1;
        }
        else if (dir == Vector2.left)
        {
            dx = -1; dy = 0;
        }
        else if (dir == Vector2.right)
        {
            dx = 1; dy = 0;
        }

        Vector3Int nextPos = new Vector3Int(cellPosition.x + dx, cellPosition.y + dy, cellPosition.z);

        return nextPos;
    }
    private List<Vector3Int> GetFillAreaPos(Vector3Int cellPos)
    {
        List<Vector2> list = new List<Vector2>();
        list = _playInfo.GetMoveTileList();

        // ó�� ���� ��ġ�� ����Ʈ�� �߰�
        Vector2 pos = new Vector2(cellPos.x,cellPos.y);
        list.Add(pos);

        List<Vector3Int> FillAreaPosList = new List<Vector3Int>();

        for (int i = list.Count - 2; i > 0; i--)
        {
            bool result;

            if (list[i + 1].y == list[i].y)
            {
                result = PerformTilemapSearch(new Vector3Int((int)list[i].x, (int)list[i].y + 1, cellPos.z));
                if (result == true)
                {
                    FillAreaPosList.Add(new Vector3Int((int)list[i].x, (int)list[i].y + 1, cellPos.z));
                }

                result = PerformTilemapSearch(new Vector3Int((int)list[i].x, (int)list[i].y - 1, cellPos.z));
                if (result == true)
                {
                    FillAreaPosList.Add(new Vector3Int((int)list[i].x, (int)list[i].y - 1, cellPos.z));
                }
            }
            else
            {
                result = PerformTilemapSearch(new Vector3Int((int)list[i].x + 1, (int)list[i].y, cellPos.z));
                if (result == true)
                {
                    FillAreaPosList.Add(new Vector3Int((int)list[i].x + 1, (int)list[i].y, cellPos.z));
                }

                result = PerformTilemapSearch(new Vector3Int((int)list[i].x - 1, (int)list[i].y, cellPos.z));
                if (result == true)
                {
                    FillAreaPosList.Add(new Vector3Int((int)list[i].x - 1, (int)list[i].y, cellPos.z));
                }
            }
        }

        return FillAreaPosList;
    }


    private bool IsInit()
    {
        // �ʱ�ȭ�� �̷����ٸ� _moveColor�� _borderColor�� ������ �����Ƿ� return
        return _moveColor == _borderColor;
    }

    // Ÿ�� ���� �Լ���
    private void ChangeMoveLIstTile(Vector3Int celPos)
    {
        List<Vector2> moveList = _playInfo.GetMoveTileList();

        if (moveList.Count == 0)
        {
            return;
        }

        foreach (var move in moveList)
        {
            UpdateTileColorToArea(new int[3] { (int)move.x, (int)move.y, celPos.z });
        }

    }

    private void FillArea(Vector3Int cellPos)
    {
        List<Vector3Int> FillAreaList = new List<Vector3Int>();

        FillAreaList = GetFillAreaPos(cellPos);

        if (FillAreaList.Count > 0)
        {
            foreach (Vector3Int fillArea in FillAreaList)
            {
                FloodFillArea(fillArea);
            }
        }
        else
        {
            Debug.Log("FillAreaList Count is 0");
        }
    }

    private void FloodFillArea(Vector3Int cellPos)
    {
        // �ش� Ÿ���� passedColor���� Ȯ��
        Color color1 = _tilemap.GetColor(cellPos);

        if (color1 == _areaColor || color1 == _borderColor)
        {
            Debug.Log("return");
            return;
        }

        UpdateTileColorToArea(new int[3] {cellPos.x,cellPos.y,cellPos.z});
        int[] dx = new int[4] { 0, 0, -1, 1 };
        int[] dy = new int[4] { 1, -1, 0, 0 };

        Queue<Vector3Int> Q = new Queue<Vector3Int>();
        Dictionary<Vector3Int, bool> Dict = new Dictionary<Vector3Int, bool>();

        Q.Enqueue(cellPos);
        Dict.Add(cellPos, true);

        while (Q.Count > 0)
        {
            Vector3Int pos = Q.Dequeue();
            UpdateTileColorToArea(new int[3] { pos.x, pos.y, pos.z });

            for (int i = 0; i < 4; i++)
            {
                Vector3Int vPos = new Vector3Int(pos.x + dx[i], pos.y + dy[i], pos.z);
                Color color = _tilemap.GetColor(vPos);

                if (color != _areaColor && color != _borderColor)
                {
                    if (Dict.ContainsKey(vPos) == false)
                    {
                        Q.Enqueue(vPos);
                        Dict.Add(vPos, true);
                    }
                }
            }
        }

    }

    private bool PerformTilemapSearch(Vector3Int startTilePosition)
    {
        Color startColor = _tilemap.GetColor(startTilePosition);
        if (startColor == _borderColor || startColor == _areaColor)
        {
            return false;
        }

        Queue<Vector3Int> queue; //
        queue = new Queue<Vector3Int>();
        queue.Enqueue(startTilePosition);

        Dictionary<Vector3Int, bool> Dict = new Dictionary<Vector3Int, bool>();
        Dict.Add(startTilePosition, true);

        while (queue.Count > 0)
        {
            Vector3Int currentPosition = queue.Dequeue();
            Color currentTileColor = _tilemap.GetColor(currentPosition);

            if (currentTileColor == _areaColor)
            {
                // �÷��̾� Ÿ���� ã���� ��
                
            }
            else if (currentTileColor == _borderColor)
            {
                // �׵θ� Ÿ���� ������ ��
                return false;
            }
            else
            {
                // ��� Ÿ���� ��� �ֺ� Ÿ�ϵ��� ť�� �߰�
                Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

                foreach (Vector3Int direction in directions)
                {
                    Vector3Int adjacentPosition = currentPosition + direction;
                    if (Dict.ContainsKey(adjacentPosition) == false && !queue.Contains(adjacentPosition))
                    {
                        queue.Enqueue(adjacentPosition);
                        Dict.Add(adjacentPosition, true);
                    }
                }
            }
        }

        // �׵θ� Ÿ���� ������ ���� ���
        return true;
    }

    private Tilemap GetTileMap()
    {
        if(_tilemap != null)
        {
            return _tilemap;
        }

        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        if(tilemap != null)
        {
            _tilemap = tilemap;
            return _tilemap;
        }
        else
        {
            Debug.Log("Tile Map is Null");
            return null;
        }
    }
}
