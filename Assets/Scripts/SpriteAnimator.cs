using System;
using System.Collections.Generic;
using UnityEngine;

// Taken from https://www.youtube.com/watch?v=Ap8bGol7qBk&list=PLzDRvYVwl53vAkDpO6sH9aNLpu4lzE6U3&index=1
public class SpriteAnimator : MonoBehaviour
{
    public event EventHandler OnAnimationStop;
    public float FrameRate { get; set; }

    private List<Sprite> Frames;
    private int CurrentFrame { get; set; }
    private float Timer { get; set; }
    private SpriteRenderer Renderer { get; set; }
    private bool Loop { get; set; } = false;
    private bool IsPlaying { get; set; } = false;

    private void Awake()
    {
        Renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!IsPlaying)
        {
            return;
        }
        Timer += Time.deltaTime;

        if (Timer > FrameRate)
        {
            Timer -= FrameRate;
            CurrentFrame = (CurrentFrame + 1) % Frames.Count;
            bool isStartingFrame = CurrentFrame == 0;
            if (!Loop && isStartingFrame)
            {
                StopAnimation();
            }
            else
            {
                Renderer.sprite = Frames[CurrentFrame];
            }
        }
    }

    public void StopAnimation()
    {
        IsPlaying = false;
        OnAnimationStop?.Invoke(this, EventArgs.Empty);
    }

    public void PlayAnimation(List<Sprite> frames, float frameRate, bool loop = true)
    {
        if (frames == null)
        {
            StopAnimation();
            return;
        }
        IsPlaying = true;
        Frames = frames;
        CurrentFrame = 0;
        FrameRate = frameRate;
        Timer = 0f;
        Loop = loop;
        Renderer.sprite = Frames[CurrentFrame];
    }
}
