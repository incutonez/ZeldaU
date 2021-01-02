using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODOJEF: Come back to this https://www.youtube.com/watch?v=mJRc9kLxFSk 6 mins
public class NewBehaviourScript : MonoBehaviour
{
    public float Speed;
    private Vector3 Velocity { get; set; }

    public void SetVelocity(Vector3 velocity)
    {
        Velocity = velocity;
    }

    private void Update()
    {
        transform.position = Velocity * Speed * Time.deltaTime;
    }
}
