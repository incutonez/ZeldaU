using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class Blink : MonoBehaviour
    {
        public float FrameRate { get; set; }
        public List<Sprite> Frames { get; set; }
        private int CurrentFrame { get; set; } = 0;
        private SpriteRenderer Renderer { get; set; }

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            StartCoroutine(StartBlink());
        }

        public void Initialize(List<Sprite> frames, float frameRate = 0.25f)
        {
            Frames = frames;
            FrameRate = frameRate;
        }

        private IEnumerator StartBlink()
        {
            Renderer.sprite = Frames[CurrentFrame];
            yield return new WaitForSeconds(FrameRate);
            StartCoroutine(StartBlink());
            CurrentFrame = ++CurrentFrame % Frames.Count;
        }
    }
}
