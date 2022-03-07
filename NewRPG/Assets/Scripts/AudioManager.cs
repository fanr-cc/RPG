using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    private AudioSource audioSource;
    public AudioClip[] audioClip;
    void Awake()
    {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayAudioSource(string audio)
    {
        switch (audio)
        {
            case "Passive":
                audioSource.clip = audioClip[6];
                audioSource.Play();
                break;
            case "singleSkill":
                audioSource.clip = audioClip[5];
                audioSource.Play();
                break;
            case "MultiSkill":
                audioSource.clip = audioClip[1];
                audioSource.Play();
                break;

            case "attackSword":
                audioSource.clip = audioClip[0];
                audioSource.Play();
                break;
            case "attackMagician":
                audioSource.clip = audioClip[2];
                audioSource.Play();
                break;

            case "buffSword":
                audioSource.clip = audioClip[4];
                audioSource.Play();
                break;
            case "buffMagician":
                audioSource.clip = audioClip[3];
                audioSource.Play();
                break;

         
        }
    }
}
