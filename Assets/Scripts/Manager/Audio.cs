using Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Audio : MonoBehaviour
    {
        public AudioSource AudioSource { get; set; }

        private Dictionary<string, AudioClip> fx = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
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
                    AudioSource.clip = clip;
                    AudioSource.loop = true;
                    AudioSource.Play();
                }
                else
                {
                    AudioSource.PlayOneShot(clip);
                }
                length = clip.length;
            }
            return length;
        }

        public void StopFX()
        {
            AudioSource.Stop();
        }
    }
}