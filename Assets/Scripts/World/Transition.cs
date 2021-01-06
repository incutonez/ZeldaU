using UnityEngine;

namespace World
{
    public class Transition : MonoBehaviour
    {
        public ViewModel.Grid ViewModel { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Manager.Game.IsTransitioning)
            {
                return;
            }
            if (collision.name == Constants.PLAYER_TRANSITION)
            {
                if (ViewModel.Name == Constants.TRANSITION_BACK)
                {
                    Manager.Game.Scene.StartExitDoor(ViewModel);
                }
                else
                {
                    Manager.Game.Scene.StartPanScreen(ViewModel);
                }
            }
        }

        public void Initialize(ViewModel.Grid transition)
        {
            ViewModel = transition;
            // We have to make sure our position gets the offset by the x and y transition values... this is because we want
            // the transition just outside of the world space, really in the "negative" world space
            transform.position += new Vector3(transition.X, transition.Y);
        }
    }
}
