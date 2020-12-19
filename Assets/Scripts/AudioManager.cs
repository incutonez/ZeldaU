using Audio;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static AudioManager Instance { get; private set; }

    private Hashtable fx = new Hashtable();

    private void Awake()
    {
        Instance = this;
        AudioClip[] items = Resources.LoadAll<AudioClip>("Audio/FX");
        foreach (AudioClip item in items)
        {
            fx.Add(item.name, item);
        }
    }

    public void PlayFX(FX fxType)
    {
        string key = fxType.GetDescription();
        AudioClip clip = (AudioClip) fx[key];
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
