using UnityEngine;

public class WorldPlayerTransition : MonoBehaviour
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
            if (worldMatter.CanEnter())
            {
                StartCoroutine(GameHandler.sceneBuilder.EnterDoor(worldMatter));
            }
            else if (worldMatter.IsTransition())
            {
                StartCoroutine(GameHandler.sceneBuilder.LoadScreen(worldMatter));
            }
        }
    }
}
