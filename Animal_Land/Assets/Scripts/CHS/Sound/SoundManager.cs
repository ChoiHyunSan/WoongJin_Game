using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public enum Effect
{
    Gameover = 0,   // ���� ����
    Button,         // ��ư Ŭ�� 
    Back,           // �ڷ� ����
    Ground,         // ���� ȹ���� ���
    Item,           // ������ ȹ��
    Countdown,      // �ð��� �� ���� ���� ���
    Correct,        // ������ ���� ���
    Wrong,          // ������ Ʋ�� ���
    Erase,          // �޸��� �����
    Punch           // �ǰ� �� 
}

// ������ �ش� ��ũ��Ʈ�� UIManager�� �߰��ϰ� ������ǰ� ������ �����Ѵ�.
public class SoundManager : MonoBehaviour
{
    [Header("���� Ʈ��")]
    public AudioClip BackGroundSound;

    [Header("����")]
    public Slider BGSoundSlider;
    public Slider EffectSoundSlider;

    [Header("����Ʈ")]
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioSource _playerSource;
    [SerializeField] private AudioSource _monsterSource;
    [SerializeField] private AudioSource _itemSource;

    [Header("����Ʈ ���ҽ�")]
    [SerializeField] private AudioClip _gameOver;
    [SerializeField] private AudioClip _button;
    [SerializeField] private AudioClip _back;
    [SerializeField] private AudioClip _ground;
    [SerializeField] private AudioClip _item;
    [SerializeField] private AudioClip _countdown;
    [SerializeField] private AudioClip _correct;
    [SerializeField] private AudioClip _wrong;
    [SerializeField] private AudioClip _erase;
    [SerializeField] private AudioClip _punch;

    private AudioSource audioSource;
    UserManager userManager;

    private void Awake()
    {
        Init();
    }

    private void ChangeAudioSource(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }

    private void Init()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BackGroundSound;

#if UNITY_EDITOR
        Debug.Log("���� �� �ʱ�ȭ");
#endif

        userManager = GameObject.Find("UserManager").GetComponent<UserManager>();
        if(userManager != null )
        {
            float bgVolume = userManager.GetSoundVolume() + 0.06f;
            float effectVolume = userManager.GetEffectVolume() + 0.06f;
            if (bgVolume > 0.5f)
            {
                bgVolume = 0.5f;
            }
            if (effectVolume > 0.5f)
            {
                effectVolume = 0.5f;
            }

            userManager.SetEffectVolume(effectVolume);
            userManager.SetSoundVolume(bgVolume);

            audioSource.volume = userManager.GetSoundVolume();
            if (BGSoundSlider != null)
            {
                BGSoundSlider.value = userManager.GetSoundVolume();
            }

            if (EffectSoundSlider != null)
            {
                EffectSoundSlider.value = userManager.GetEffectVolume();
            }

            if (_effectSource != null)
            {
                _effectSource.volume = userManager.GetEffectVolume();
            }

            if (_playerSource != null)
            {
               _playerSource.volume = userManager.GetEffectVolume();
            }

            if (_monsterSource != null)
            {
                _monsterSource.volume = userManager.GetEffectVolume();
            }

            if (_itemSource != null)
            {
                _itemSource.volume = userManager.GetEffectVolume();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float volume)
    {
        if(volume < 0.06f)
        {
            volume = 0.06f;
            BGSoundSlider.value = volume;
        }

        // �ּ� �� ����
        volume -= 0.06f;

        if (audioSource != null)
        {
            audioSource.volume = volume;
            if(userManager != null)
            {
                userManager.SetSoundVolume(volume);
            }
        }
    }

    public void SetEffectVolume(float volume)
    {
        if (volume < 0.06f)
        {
            volume = 0.06f;
            EffectSoundSlider.value = volume;
        }

        // �ּ� �� ����
        volume -= 0.06f;

        if (userManager != null)
        {
            userManager.SetEffectVolume(volume);
        }

        if (_effectSource != null)
        {
            _effectSource.volume = volume;
        }

        if (_playerSource != null)
        {
            _playerSource.volume = volume;
        }

        if (_monsterSource != null)
        {
            _monsterSource.volume = volume;
        }

        if (_itemSource != null)
        {
            _itemSource.volume = volume;
        }
    }

    public void PlayEffect(Effect effect)
    {
        if(_effectSource == null)
        {
            return;
        }

        switch(effect)
        {
            case Effect.Back:
                _effectSource.clip = _back;
                _effectSource.Play();
                break;
            case Effect.Button:
                _effectSource.clip = _button;
                _effectSource.Play();
                break;
            case Effect.Erase:
                _effectSource.clip = _erase;
                _effectSource.Play();
                break;
            case Effect.Countdown:
                _effectSource.clip = _countdown;
                _effectSource.Play();
                break;
            case Effect.Item:
                _itemSource.clip = _item;
                _itemSource.Play();
                break;
            case Effect.Correct:
                _effectSource.clip = _correct;
                _effectSource.Play();
                break;
            case Effect.Ground:
                _playerSource.clip = _ground;
                _playerSource.Play();
                break;
            case Effect.Gameover:
                _effectSource.clip = _gameOver;
                _effectSource.Play();
                break;
            case Effect.Wrong:
                _effectSource.clip = _wrong;
                _effectSource.Play();
                break;
            case Effect.Punch:
                _monsterSource.clip = _punch;
                _monsterSource.Play();
                break;
        }
    }
}
