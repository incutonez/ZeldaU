using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameHandler.isTransitioning)
        {
            return;
        }
        WorldMatter worldMatter = collision.gameObject.GetComponent<WorldMatter>();
        if (worldMatter != null)
        {
            if (worldMatter.GetMatter().CanEnter())
            {
                StartCoroutine(GameHandler.player.Enter(worldMatter));
            }
            else if (worldMatter.IsTransition())
            {
                GameHandler.sceneBuilder.LoadScreen(worldMatter);
            }
        }
    }
}
