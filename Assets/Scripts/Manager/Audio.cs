using Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Audio : MonoBehaviour
    {
        public AudioSource audioSource;
        public static Audio Instance { get; private set; }

        private Dictionary<string, AudioClip> fx = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            Instance = this;
            AudioClip[] items = Resources.LoadAll<AudioClip>("Audio/FX");
            foreach (AudioClip item in items)
            {

                fx.Add(item.name, item);
            }
        }

        public float PlayFX(FX fxType, bool loop = false)
        {
            float length = 0f;
            string key = fxType.GetDescription();
            AudioClip clip = fx[key];
            if (clip != null)
            {
                if (loop)
                {
                    audioSource.clip = clip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                else
                {
                    audioSource.PlayOneShot(clip);
                }
                length = clip.length;
            }
            return length;
        }

        public void StopFX()
        {
            audioSource.Stop();
        }
    }
}