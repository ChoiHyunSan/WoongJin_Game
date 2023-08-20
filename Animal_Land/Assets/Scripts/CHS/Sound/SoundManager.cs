using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ �ش� ��ũ��Ʈ�� UIManager�� �߰��ϰ� ������ǰ� ������ �����Ѵ�.
public class SoundManager : MonoBehaviour
{
    [Header("���� Ʈ��")]
    public AudioClip BackGroundSound;

    [Header("����")]
    public Slider BGSound;
    public Slider EffectSound;

    private AudioSource audioSource;

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
        if (audioSource != null)
        {
            audioSource.volume = volume * 0.2f;
        }
    }
}
