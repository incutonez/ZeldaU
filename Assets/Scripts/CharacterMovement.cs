using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed;
    public bool CanAttack { get; set; } = true;
    public Vector3 Movement { get; set; }
    public World.PlayerBase PlayerBase { get; set; }

    private void Awake()
    {
        PlayerBase = GetComponent<World.PlayerBase>();
    }

    private void Update()
    {
        if (PlayerBase.BlockAnimations)
        {
            return;
        }
        PlayerBase.AnimateMove(Movement);
    }

    private void FixedUpdate()
    {
        if (Manager.Game.IsTransitioning || PlayerBase.BlockAnimations)
        {
            return;
        }
        // Good resource https://forum.unity.com/threads/the-proper-way-to-control-the-player.429459/
        transform.Translate(Movement * Time.deltaTime * Speed);
    }
}
