using UnityEngine;

public class WorldTransition : MonoBehaviour
{
    public SceneViewModel Transition { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameHandler.IsTransitioning)
        {
            return;
        }
        if (collision.name == Constants.PLAYER_TRANSITION)
        {
            if (Transition.Name == Constants.TRANSITION_BACK)
            {
                GameHandler.SceneBuilder.StartExitDoor(Transition);
            }
            else
            {
                GameHandler.SceneBuilder.StartPanScreen(Transition);
            }
        }
    }

    public void Initialize(SceneViewModel transition)
    {
        Transition = transition;
        // We have to make sure our position gets the offset by the x and y transition values... this is because we want
        // the transition just outside of the world space, really in the "negative" world space
        transform.position += new Vector3(transition.X, transition.Y);
    }
}
