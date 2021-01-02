using UnityEngine;

namespace World
{
    public class Door : MonoBehaviour
    {
        public RectTransform HiddenDoor { get; set; }
        public SceneViewModel Transition { get; set; }

        public void Initialize(Color groundColor, SceneViewModel transition)
        {
            Transition = transition;
            HiddenDoor = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}DoorBlock"), transform);
            HiddenDoor.GetComponent<SpriteRenderer>().color = groundColor;
        }

        public void ToggleHiddenDoor(bool active)
        {
            HiddenDoor.gameObject.SetActive(active);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Manager.Game.IsTransitioning)
            {
                return;
            }
            if (collision.name == Constants.PLAYER_TRANSITION)
            {
                ToggleHiddenDoor(true);
                Manager.Game.Scene.StartEnterDoor(Transition);
            }
        }
    }
}
