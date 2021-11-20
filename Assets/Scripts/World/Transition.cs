using UnityEngine;

namespace World
{
    public class Transition : MonoBehaviour
    {
        public ViewModel.Grid ViewModel { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Manager.Game.IsPaused)
            {
                return;
            }
            if (collision.name == Constants.PlayerTransition)
            {
                if (ViewModel.Name == Constants.TransitionBack)
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
            if (transition.Name != Constants.TransitionBack)
            {
                // We have to make sure our position gets the offset by the x and y transition values... this is because we want
                // the transition just outside of the world space, really in the "negative" world space
                transform.position += new Vector3(transition.X, transition.Y);
            }
        }
    }
}
