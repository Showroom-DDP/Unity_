using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSoundManager : MonoBehaviour
{
    public static UiSoundManager instance;

    public AudioClip click;

    AudioSource audioSource;

    private void Awake()
    {
        //instance ���� null�̸�
        if (instance == null)
        {
            //�� ��ũ��Ʈ�� instance�� ����
            instance = this;

            //�� ��ȯ�ص� �����ϴ� �ڵ�
            DontDestroyOnLoad(gameObject);
        }
        //�̹� instance�� ���� ���� ����ִٸ�?
        else
        {
            //�ǵ�ġ ���� �ߺ� ������ �´� �� ���� ������Ʈ �ı�.
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip audios, float volume)
    {
        audioSource.PlayOneShot(audios, volume);
    }

    public void AudioClick()
    {
        PlaySound(click, 0.5f);
    }
}
