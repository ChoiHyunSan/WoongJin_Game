using Mono.Cecil.Cil;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Test
{
    Sprite CharcterSprite; 
}

// ĳ������ ������ �����ϴ� ��ũ��Ʈ
// ĳ���� �����տ� ������Ʈ�� �߰��Ͽ� ĳ������ ������ �����Ѵ�.
public class Character : MonoBehaviour
{
    [SerializeField]
    CharacterDefaultInfo    _defaultInfo;
    [SerializeField]
    CharacterInfo           _characterInfo;

    private PhotonView _pv;

    private void Awake()
    {
        _defaultInfo = new CharacterDefaultInfo();
        _characterInfo = new CharacterInfo();
        _pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���� Ŭ���̾�Ʈ�� ������ �ƴ� ĳ���͵��� ������ Ŭ���̾�Ʈ�κ��� �޾ƿ´�.
        if(_pv.IsMine == false && PhotonNetwork.IsMasterClient == false)
        {
            _pv.RPC("SetupCharacterFromMasterClientRPC", RpcTarget.MasterClient);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �÷��̾� ī��Ʈ�� �޾� �ش� ĳ���ͷ� ������Ʈ�Ѵ�. 
    // Ŭ���̾�Ʈ ���� ��, ����Ǵ� �޼ҵ�
    public void SetupCharacter(int playerCount)
    {
        if (GetUserManager() != null)
        {
            // ���� ������ ���� ����Ʈ ������ �ҷ��´�.
            _defaultInfo = GetUserManager().GetCharcterDefaultInfo(playerCount);

            // �ڽ��� ������ �����´�.
            _characterInfo = GetUserManager().GetCharcterInfo(playerCount);

            // ���� ������ �̿��Ͽ� ĳ���� Ŀ������ �ҷ��´�.
            
        }

        // �ҷ��� ������ �̿��Ͽ� ĳ���� ������ ������Ʈ �Ѵ�.
        SetupCharacterFromMasterClientRPC();
    }

    [PunRPC]
    public void SetupCharacterFromMasterClientRPC()
    {
        // �ش� �Լ��� ȣ����� ������ Ŭ���̾�Ʈ�� �ڽ��� ������ ��� RPC�� �����Ѵ�
        _pv.RPC("UpdateCharacterRPC", RpcTarget.All,
            _defaultInfo.GetMoveColor(),
            _defaultInfo.GetAreaColor(),
            _defaultInfo.GetBorderColor(),
            _defaultInfo.GetCharcterSpriteName());
    }

    [PunRPC]
    private void UpdateCharacterRPC(
        /*���⿡ ������Ʈ�� ������ �Ѱ��ش�.*/
        /*Default Info*/
        float[] moveColor,
        float[] areaColor,
        float[] borderColor,
        string sprieName

        /*Custom Info*/


        /**/

     )
    {
        // CharcterSprite ���� ����&������Ʈ
        {
            // ������ �����Ѵ�.
            _defaultInfo.SetCharacterSpriteName(sprieName);

            // ������ ������Ʈ�Ѵ�.
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Sprite sprite = Resources.Load<Sprite>("Sprites/Characters/" + _defaultInfo.GetCharcterSpriteName());
                spriteRenderer.sprite = sprite;
            }
        }
        // Color ���� ����&������Ʈ (Move, Area, Border)
        {
            TileColorChange tileColorChange = GetComponent<TileColorChange>();
            if(tileColorChange == null)
            {
                Debug.Log("TileColorChange Component is Null");
                return;
            }

            // MoveColor ���� ������Ʈ
            {
                // ������ �����Ѵ�.
                _defaultInfo.SetMoveColor(moveColor);

                // ������ ������Ʈ�Ѵ�.
                tileColorChange.SetMoveColor(_defaultInfo.GetMoveColor());
            }
            // Araa Color ���� ������Ʈ
            {
                // ������ �����Ѵ�.
                _defaultInfo.SetAreaColor(areaColor);

                // ������ ������Ʈ�Ѵ�.
                tileColorChange.SetAreaColor(_defaultInfo.GetAreaColor());
            }
            // Border Color ���� ������Ʈ
            {
                // ������ �����Ѵ�.
                _defaultInfo.SetBorderColor(borderColor);

                // ������ ������Ʈ�Ѵ�.
                tileColorChange.SetBorderColor(_defaultInfo.GetBorderColor());
            }
        }
        // 
    }

    private UserManager GetUserManager()
    {
        GameObject Object = GameObject.Find("UserManager");
        if (Object != null)
        {
            UserManager userManager = Object.GetComponent<UserManager>();
            if (userManager != null)
            {
                return userManager;
            }
        }

        Debug.Log("User Manager is Null");
        return null;
    }
}
