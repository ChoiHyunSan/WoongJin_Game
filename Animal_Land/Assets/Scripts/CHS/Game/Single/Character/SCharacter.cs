using Contents;
using Mono.Cecil.Cil;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


// ĳ������ ������ �����ϴ� ��ũ��Ʈ
// ĳ���� �����տ� ������Ʈ�� �߰��Ͽ� ĳ������ ������ �����Ѵ�.
public class SCharacter : MonoBehaviour
{
    [SerializeField]
    CharacterDefaultInfo    _defaultInfo;
    [SerializeField]
    CharacterInfo           _characterInfo;

    [Header("�̵� ������")]
    [SerializeField] private float _moveGauge = 10f;
    [SerializeField] private float _maxMoveGauge = 10f;
    [SerializeField] private float _moveGuageConsumption = 1f;

    [SerializeField] private SGameUIManager _gameUIManager;

    private void Awake()
    {
        _defaultInfo = new CharacterDefaultInfo();
        _characterInfo = new CharacterInfo();

        _gameUIManager = GameObject.Find("UIManager").GetComponent<SGameUIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    // �÷��̾� ī��Ʈ�� �޾� �ش� ĳ���ͷ� ������Ʈ�Ѵ�. 
    // Ŭ���̾�Ʈ ���� ��, ����Ǵ� �޼ҵ�
    public void SetupCharacter(int playerCount, string characterType)
    {
        UserManager userManager = GetUserManager();
        if (userManager != null)
        {
            // ���� ������ ���� ����Ʈ ������ �ҷ��´�.
            _defaultInfo = userManager.GetCharcterDefaultInfo(playerCount);

            // �ڽ��� ������ �����´�.
            _characterInfo = userManager.GetCharcterInfo(playerCount);

            // ���� ������ �̿��Ͽ� ĳ���� Ŀ������ �ҷ��´�.
            Equipment(characterType);

            // �ҷ��� ������ �̿��Ͽ� ĳ���� ������ ������Ʈ �Ѵ�.
            UpdateCharacter(
                _defaultInfo.GetMoveColor(),
                _defaultInfo.GetAreaColor(),
                _defaultInfo.GetBorderColor(),
                _defaultInfo.GetCharcterSpriteName());
        }
    }
    private void UpdateCharacter(
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
            SpriteRenderer spriteRenderer = this.gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Sprite sprite = Resources.Load<Sprite>("Sprites/Characters/" + _defaultInfo.GetCharcterSpriteName());
                spriteRenderer.sprite = sprite;
            }
        }
        // Color ���� ����&������Ʈ (Move, Area, Border)
        {
            STileColorChange tileColorChange = GetComponent<STileColorChange>();
            if (tileColorChange == null)
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
            // Ara Color ���� ������Ʈ
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

    public void StopPlayer()
    {
        StopMove();
    }

    private void SetMove(bool value)
    {
        // ���̽�ƽ 
        SJoyStick JoyStick = GameObject.FindGameObjectWithTag("GameController").GetComponent<SJoyStick>();
        if (JoyStick != null)
        {
            if (JoyStick.IsGameStart() == value)
            {
                return;
            }
            else
            {
                JoyStick.SetIsGameStart(value);
            }
        }

        this.GetComponent<Rigidbody2D>().velocity = Vector3.zero; 
    }

    public void StopMove()
    {
        SetMove(false);

    }

    public void StartPlayer()
    {
        SetMove(true);


    }

    private void Equipment(string type)
    {
        // Ŀ���� ��� �����Ѵ�.
        SpriteRenderer Hat = this.gameObject.transform.Find("Hat").GetComponent<SpriteRenderer>();
        SpriteRenderer Face = this.gameObject.transform.Find("Face").GetComponent<SpriteRenderer>();
        SpriteRenderer Glasses = this.gameObject.transform.Find("Glass").GetComponent<SpriteRenderer>();
        SpriteRenderer Etc = this.gameObject.transform.Find("Necklace").GetComponent<SpriteRenderer>();

        DataManager _dataManager = GameObject.Find("@DataManager").GetComponent<DataManager>();
        if(_dataManager != null )
        {
            CharacterCustom customData = _dataManager.CharacterCustomData[type];

            // TODO : ���⼭ ��������Ʈ name�� �ҷ��ͼ� ���Ͽ� ������ �ڿ� ������Ʈ
            if (customData.ItemDict["Hat"] != null)
            {
                string HatItem = RemoveSubstring(customData.ItemDict["Hat"], "_" + type);
#if UNITY_EDITOR
                Debug.Log(HatItem);
#endif
                Hat.sprite = Resources.Load<Sprite>("Sprites/Items/Hat/" + HatItem);
            }

            if (customData.ItemDict["Face"] != null)
            {
                string FaceItem = RemoveSubstring(customData.ItemDict["Face"], "_" + type);
#if UNITY_EDITOR
                Debug.Log(customData.ItemDict["Face"]);
#endif
                Face.sprite = Resources.Load<Sprite>("Sprites/Items/Face/" + customData.ItemDict["Face"]);
            }

            if (customData.ItemDict["Glasses"] != null)
            {
                string GlassesItem = RemoveSubstring(customData.ItemDict["Glasses"], "_" + type);
#if UNITY_EDITOR
                Debug.Log(GlassesItem);
#endif
                Glasses.sprite = Resources.Load<Sprite>("Sprites/Items/Glasses/" + GlassesItem);
            }

            if (customData.ItemDict["Etc"] != null)
            {
                string EtcItem = RemoveSubstring(customData.ItemDict["Etc"], "_" + type);
#if UNITY_EDITOR
                Debug.Log(EtcItem);
#endif
                Etc.sprite = Resources.Load<Sprite>("Sprites/Items/Hat/" + EtcItem);
            }
        }
    }

    public void ConsumeMoveGauge()
    {
        _moveGauge -= (_moveGuageConsumption * Time.deltaTime);

        if(_moveGauge <= 0f)
        {
            StopMove();
            _moveGauge = 0f;
            _gameUIManager.UpdateGauge(0f);

            return;
        }

        _gameUIManager.UpdateGauge( _moveGauge / _maxMoveGauge);
    }

    public float AddMoveGauge(float value)
    {
        float newGaugeValue = _moveGauge + value;
        if(newGaugeValue > _maxMoveGauge)
        {
            newGaugeValue = _maxMoveGauge;
        }

        _moveGauge = newGaugeValue;

        return _moveGauge / _maxMoveGauge;
    }

    string RemoveSubstring(string original, string substring)
    {
        int index = original.IndexOf(substring);
        if (index >= 0)
        {
            return original.Substring(0, index);
        }
        return original;
    }


}
