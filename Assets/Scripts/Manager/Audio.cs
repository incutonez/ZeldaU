using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Manager
{
    public class Audio : MonoBehaviour
    {
        public AudioSource AudioSource { get; set; }

        private Dictionary<FX, AudioClip> Effects = new Dictionary<FX, AudioClip>();

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            FileSystem.LoadAudioClips("FX", (response) =>
            {
                Effects = response;
            });
        }

        public float PlayFX(FX fxType, bool loop = false)
        {
            float length = 0f;
            AudioClip clip = Effects[fxType];
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