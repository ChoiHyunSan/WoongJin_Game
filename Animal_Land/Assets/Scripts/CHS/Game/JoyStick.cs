using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Profiling;
using System;
using UnityEngine.Tilemaps;

struct MapSize
{
    public float t;
    public float l;
    public float b;
    public float r;
}

public class JoyStick : MonoBehaviourPun, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject Character;                // ĳ���� ������Ʈ.
    public RectTransform TouchArea;             // Joystick Touch Area �̹����� RectTransform.
    public Image OuterPad;                      // OuterPad �̹���.
    public Image InnerPad;                      // InnerPad �̹���.

    private Vector2 _joystickVector;            // ���̽�ƽ�� ���⺤������ �÷��̾�� �ѱ� ��������.
    private Vector3 _characterDirection;        // ĳ���� �̵� ����
    [SerializeField]
    private float _speed;                       // ĳ���� ���ǵ�
    private float _rotateSpeed;                 // ȸ�� �ӵ�
    private Coroutine _runningCoroutine;        // �ε巯�� ȸ�� �ڷ�ƾ
    private bool _OnTouch;

    private PhotonView  _pv;
    private PlayInfo    _playInfo;
    [SerializeField]
    private bool        _isGameStart;

    // Size of Map
    private MapSize _mapSize;
    private bool _check = false;

    void Awake()
    {
        _speed = 1.5f;
        _rotateSpeed = 5.0f;
        _OnTouch = false;
        _characterDirection = Vector3.zero;
        _isGameStart = false;
    }

    void Start()
    {
        _pv = Character.GetComponent<PhotonView>();
        _playInfo = Character.GetComponent<PlayInfo>();
    }

    void Update()
    {
        if(Character == null)
        {
            return;
        }

        // ������ �����ϱ� �������� �Է��� ���� �ʴ´�.
        if(_isGameStart == false)
        {
            return;
        }

        if(_check == true)
        {
            return;
        }

        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        if (_OnTouch)
        {
            SetVelocity(Character, _characterDirection);
        }
        else
        {
            SetVelocityZero(Character);
        }
    }

    private void SetVelocityZero(GameObject Object)
    {
        if (_pv.IsMine == false)
        {
            return;
        }

        Object.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void SetVelocity(GameObject Object, Vector3 _direction)
    {
        if (_pv.IsMine == false)
        {
            return;
        }

        Object.GetComponent<Rigidbody2D>().velocity = _direction * _speed;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(TouchArea,
            eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            localPoint.x = (localPoint.x / TouchArea.sizeDelta.x);
            localPoint.y = (localPoint.y / TouchArea.sizeDelta.y);
            // Joystick Touch Area�� ���� ���ϱ� ( -0.5 ~ 0.5 )

            // Calculate the direction vector from the joystick's center to the current position
            Vector2 direction = localPoint - TouchArea.rect.center;

            // Calculate the angle in radians using Atan2
            float angle = Mathf.Atan2(direction.y, direction.x);

            // Convert the angle to degrees
            float angleDegrees = angle * Mathf.Rad2Deg;

            // Ensure the angle is within the range of 0 to 360 degrees
            if (angleDegrees < 0)
            {
                angleDegrees += 360f;
            }

            _joystickVector = new Vector2(localPoint.x * 2.6f, localPoint.y * 2);

            _characterDirection = GetDirection(angleDegrees);

            _joystickVector = (_joystickVector.magnitude > 0.35f) ? _joystickVector.normalized * 0.35f : _joystickVector;
            // innerPad �̹����� outerPad�� �Ѿ�ٸ� ��ġ �������ֱ�

            InnerPad.rectTransform.anchoredPosition = new Vector2(_joystickVector.x * (OuterPad.rectTransform.sizeDelta.x),
                _joystickVector.y * (OuterPad.rectTransform.sizeDelta.y));
        }
    }

    private Vector3 GetDirection(float angleDegrees)
    {
        Vector3 direction = Vector3.zero;

        if (angleDegrees <= 45f || angleDegrees > 315f)
        {
            direction = Vector3.right;
            //_pv.RPC("SetCurDir", RpcTarget.MasterClient, Vector2.right);
        }
        else if (angleDegrees > 45f && angleDegrees <= 135f)
        {
            direction = Vector3.up;
            //_pv.RPC("SetCurDir", RpcTarget.MasterClient, Vector2.up);
        }
        else if (angleDegrees > 135f && angleDegrees <= 225f)
        {
            direction = Vector3.left;
            //_pv.RPC("SetCurDir", RpcTarget.MasterClient, Vector2.left);
        }
        else if (angleDegrees > 225f && angleDegrees <= 315f)
        {
            direction = Vector3.down;
            //_pv.RPC("SetCurDir", RpcTarget.MasterClient, Vector2.down);
        }

        return direction;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (_pv.IsMine == false)
        {
            return;
        }

        OnDrag(eventData); // ��ġ�� ���۵Ǹ� OnDrag ó��.
        _OnTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_pv.IsMine == false)
        {
            return;
        }

        InnerPad.rectTransform.anchoredPosition = Vector2.zero;
        _OnTouch = false;
    }

    public void SetIsGameStart(bool isGameStart)
    {
        _isGameStart = isGameStart;
    }

    public bool IsGameStart()
    {
        return _isGameStart;
    }

    public void SetCheck(bool check)
    {
        _check = check;
    }
}
