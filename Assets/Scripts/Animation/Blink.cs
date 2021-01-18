using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Animation
{
    public class Blink : MonoBehaviour
    {
        public float FrameRate;
        public List<Sprite> Frames;
        private int CurrentFrame { get; set; } = 0;
        private SpriteRenderer Renderer { get; set; }
        private Image Image { get; set; }

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            if (Renderer == null)
            {
                Image = GetComponent<Image>();
            }
        }

        public void Initialize(List<Sprite> frames, float frameRate = 0.25f)
        {
            Frames = frames;
            FrameRate = frameRate;
        }

        private IEnumerator StartBlink()
        {
            if (Renderer != null)
            {
                Renderer.sprite = Frames[CurrentFrame];
            }
            else if (Image != null)
            {
                Image.sprite = Frames[CurrentFrame];
            }
            yield return new WaitForSeconds(FrameRate);
            StartCoroutine(StartBlink());
            CurrentFrame = ++CurrentFrame % Frames.Count;
        }

        private void OnEnable()
        {
            StartCoroutine(StartBlink());
        }
    }
}
