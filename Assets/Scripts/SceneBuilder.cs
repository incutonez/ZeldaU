using System.Collections;
using UnityEngine;

/// <summary>
/// The camera is set to 7.5 for its size because it's Camera ortographic size = vertical resolution (240) / PPU (16) / 2 = 7.5
/// per https://hackernoon.com/making-your-pixel-art-game-look-pixel-perfect-in-unity3d-3534963cad1d
/// </summary>
public class SceneBuilder : BaseManager<SceneBuilder>
{
    public Transform screensContainer;
    public RectTransform worldMatterPrefab;

    private Vector3 overworldPosition = Vector3.zero;
    private string currentScreenId;
    private int currentX = 8;
    private int currentY = 0;
    private Animator animator;
    private const float TRANSITION_PADDING = 0.05f;
    private WorldScreen currentScreen;
    private Transform scenePrefab;

    public void Awake()
    {
        animator = transform.Find("Crossfade").GetComponent<Animator>();
        screensContainer = GameObject.Find("Screens").transform;
        scenePrefab = Resources.Load<Transform>($"{Constants.PATH_PREFABS}Scene");
        LoadSprites($"{Constants.PATH_SPRITES}worldMatters");
        worldMatterPrefab = LoadPrefab($"{Constants.PATH_PREFABS}WorldMatter");
    }

    public Transform GetScreen(string screenId)
    {
        return screenId != null ? screensContainer.Find(screenId) : null;
    }

    public void BuildScreen(string screenId)
    {
        Transform parent = GetScreen(screenId);
        // Parent has not been built, so let's build and cache it
        if (parent == null)
        {
            parent = Instantiate(scenePrefab);
            currentScreen = parent.gameObject.GetComponent<WorldScreen>().Initialize(screenId, screensContainer);
        }
        else
        {
            currentScreen = parent.gameObject.GetComponent<WorldScreen>();
            // Otherwise, the screen is in memory, so let's activate it
            parent.gameObject.SetActive(true);
        }
        Camera.main.backgroundColor = currentScreen.GroundColor;
    }

    public void ClearScreen(string screenId)
    {
        Transform screen = GetScreen(screenId);
        if (screen != null)
        {
            screen.gameObject.SetActive(false);
        }
    }

    public string GetScreenId(WorldMatter transitionMatter)
    {
        SceneViewModel transition = transitionMatter.transition;
        string screenId = transition.name;
        if (screenId == null)
        {
            currentX += transition.x;
            currentY += transition.y;
            screenId = $"{currentX}{currentY}";
        }
        else if (screenId == Constants.TRANSITION_BACK)
        {
            screenId = $"{currentX}{currentY}";
        }
        return screenId;
    }

    public IEnumerator EnterDoor(WorldMatter worldMatter)
    {
        GameHandler.IsTransitioning = true;
        currentScreen.ToggleDoor(true);
        yield return StartCoroutine(GameHandler.Player.characterAnimation.Enter());
        yield return StartCoroutine(LoadScreen(worldMatter));
        // TODOJEF: I don't like dipping into the animator like this... figure out a better way
        GameHandler.Player.characterAnimation.animator.SetBool("isEntering", false);
    }

    public IEnumerator LoadScreen(WorldMatter worldMatter)
    {
        GameHandler.IsTransitioning = true;
        // TODOJEF: need to pass in world matter's transition
        yield return StartCoroutine(LoadScreenRoutine(GetScreenId(worldMatter), GetPlayerTransitionPosition(GameHandler.Player.GetPosition(), worldMatter)));
        currentScreen.Build(worldMatter.transition);
        if (worldMatter.transition.name == Constants.TRANSITION_BACK)
        {
            currentScreen.ToggleDoor(true);
            yield return StartCoroutine(GameHandler.Player.characterAnimation.Exit());
            currentScreen.ToggleDoor(false);
        }
        GameHandler.IsTransitioning = false;
    }

    public IEnumerator LoadScreen(string screenId)
    {
        GameHandler.IsTransitioning = true;
        yield return StartCoroutine(LoadScreenRoutine(screenId, Constants.STARTING_POSITION));
        GameHandler.IsTransitioning = false;
    }

    public IEnumerator LoadScreenRoutine(string screenId, Vector3 playerPosition)
    {
        animator.SetBool("Start", true);

        yield return new WaitForSeconds(0.5f);

        // Clear the previous scene
        ClearScreen(currentScreenId);
        BuildScreen(screenId);
        GameHandler.Player.transform.localPosition = playerPosition;
        currentScreenId = screenId;

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Start", false);
    }

    public Vector3 GetPlayerTransitionPosition(Vector3 position, WorldMatter worldMatter)
    {
        SceneViewModel transition = worldMatter.transition;
        // Transitioning to a place not in overworld
        if (worldMatter.CanEnter())
        {
            // Save off the player's current position, so we can restore it later
            overworldPosition = position;
            position = new Vector3(7f, TRANSITION_PADDING);
        }
        // Need to transition back to overworld
        else if (transition.name == Constants.TRANSITION_BACK)
        {
            // Restore the player's previous position
            position = overworldPosition;
        }
        // Moving to the left screen
        else if (transition.x == -1)
        {
            position.x = Constants.GRID_COLUMNS_ZERO - TRANSITION_PADDING;
        }
        // Moving to the right screen
        else if (transition.x == 1)
        {
            position.x = TRANSITION_PADDING;
        }
        // Moving to the bottom screen
        if (transition.y == -1)
        {
            position.y = Constants.GRID_ROWS_ZERO - TRANSITION_PADDING;
        }
        // Moving to the top screen
        else if (transition.y == 1)
        {
            position.y = TRANSITION_PADDING;
        }
        return position;
    }
}
