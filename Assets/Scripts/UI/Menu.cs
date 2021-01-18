using System.Collections;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public bool IsTransitioning { get; set; }
        private RectTransform Renderer { get; set; }

        private void Awake()
        {
            Renderer = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Manager.Game.IsMenuShowing)
            {
                if (Controls.IsRightKey())
                {
                    Debug.Log("RIGHT");
                }
                else if (Controls.IsLeftKey())
                {
                    Debug.Log("LEFT");
                }
            }
        }

        public IEnumerator Pan(RectTransform hud)
        {
            Manager.Game.IsPaused = true;
            IsTransitioning = true;
            bool showMenu = !Manager.Game.IsMenuShowing;
            // Set right away to pause any movements
            if (showMenu)
            {
                // TODOJEF: Potentially change to use if the menu is active
                Manager.Game.IsMenuShowing = showMenu;
                gameObject.SetActive(true);
            }
            Vector2 inventoryDestination = showMenu ? new Vector2(Renderer.anchoredPosition.x, -176) : new Vector2(Renderer.anchoredPosition.x, 0);
            Vector2 hudDestination = showMenu ? new Vector2(hud.anchoredPosition.x, -240) : new Vector2(hud.anchoredPosition.x, -64);
            while (Renderer.anchoredPosition != inventoryDestination)
            {
                Renderer.anchoredPosition = Vector2.MoveTowards(Renderer.anchoredPosition, inventoryDestination, 0.5f);
                hud.anchoredPosition = Vector2.MoveTowards(hud.anchoredPosition, hudDestination, 0.5f);
                yield return null;
            }
            // If the menu is currently active, we want to keep IsTransitioning, so the player and enemies can't move
            if (!showMenu)
            {
                Manager.Game.IsPaused = false;
                Manager.Game.IsMenuShowing = false;
                gameObject.SetActive(false);
            }
            IsTransitioning = false;
        }
    }
}
