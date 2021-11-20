using UnityEngine;

namespace World
{
    public class Door : MonoBehaviour
    {
        public RectTransform HiddenDoor { get; set; }
        public ViewModel.Grid Transition { get; set; }

        public void Initialize(Color groundColor, ViewModel.Grid transition)
        {
            Transition = transition;
            HiddenDoor = Instantiate(Manager.Game.Graphics.DoorBlock, transform);
            HiddenDoor.GetComponent<SpriteRenderer>().color = groundColor;
        }

        public void ToggleHiddenDoor(bool active)
        {
            HiddenDoor.gameObject.SetActive(active);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Manager.Game.IsPaused)
            {
                return;
            }
            if (collision.name == Constants.PlayerTransition)
            {
                ToggleHiddenDoor(true);
                Manager.Game.Scene.StartEnterDoor(Transition);
            }
        }
    }
}
