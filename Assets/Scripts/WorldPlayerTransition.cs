using UnityEngine;

public class WorldPlayerTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameHandler.IsTransitioning)
        {
            return;
        }
        WorldMatter worldMatter = collision.gameObject.GetComponent<WorldMatter>();
        if (worldMatter != null)
        {
            if (worldMatter.IsTransition())
            {
                StartCoroutine(GameHandler.SceneBuilder.LoadScreen(worldMatter));
            }
        }
    }
}
