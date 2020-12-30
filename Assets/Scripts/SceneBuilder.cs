using System.Collections;
using UnityEngine;

/// <summary>
/// The camera is set to 7.5 for its size because it's Camera ortographic size = vertical resolution (240) / PPU (16) / 2 = 7.5
/// per https://hackernoon.com/making-your-pixel-art-game-look-pixel-perfect-in-unity3d-3534963cad1d
/// </summary>
public class SceneBuilder : BaseManager<SceneBuilder>
{
    public const int PAN_SPEED = 15;
    private const float TRANSITION_PADDING = 0.05f;

    public Transform ScreensContainer { get; set; }
    public RectTransform WorldMatterPrefab { get; set; }

    private Vector3 OverworldPosition { get; set; } = Vector3.zero;
    private int CurrentX { get; set; } = 8;
    private int CurrentY { get; set; } = 0;
    private Animator Animator { get; set; }
    private WorldScreenTile CurrentScreen { get; set; }
    public WorldScreenTile PreviousScreen { get; set; }
    private Transform ScenePrefab { get; set; }

    public new void Awake()
    {
        base.Awake();
        Animator = transform.Find("Crossfade").GetComponent<Animator>();
        ScreensContainer = GameObject.Find("Screens").transform;
        ScenePrefab = Resources.Load<Transform>($"{Constants.PATH_PREFABS}Scene");
        LoadSprites($"{Constants.PATH_SPRITES}worldMatters");
        WorldMatterPrefab = LoadPrefab($"{Constants.PATH_PREFABS}WorldMatter");
    }

    /// <summary>
    /// This is the transitioning effect that takes place between screens... it slides the next screen into view
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public IEnumerator PanScreen(SceneViewModel transition)
    {
        SetScreenLoading(true);
        var grid = PreviousScreen.Grid;
        int x = transition.X;
        int y = transition.Y;
        Transform currentTransform = CurrentScreen.transform;
        Transform previousTransform = PreviousScreen.transform;
        Transform player = GameHandler.Player.transform;
        float previousX = 0f;
        float previousY = 0f;
        float playerX = player.position.x;
        float playerY = player.position.y;
        // Moving to right screen
        if (x == 1)
        {
            previousX = -Constants.GRID_COLUMNS;
            playerX = grid.GetWorldPositionX(TRANSITION_PADDING);
        }
        // Moving to left screen
        else if (x == -1)
        {
            previousX = Constants.GRID_COLUMNS;
            playerX = grid.GetWorldPositionX(Constants.GRID_COLUMNS_ZERO - TRANSITION_PADDING);
        }
        // Moving to top screen
        if (y == 1)
        {
            previousY = -Constants.GRID_ROWS;
            playerY = grid.GetWorldPositionY(TRANSITION_PADDING);
        }
        // Moving to bottom screen
        else if (y == -1)
        {
            previousY = Constants.GRID_ROWS;
            playerY = grid.GetWorldPositionY(Constants.GRID_ROWS_ZERO - TRANSITION_PADDING);
        }
        Vector3 previousDestination = new Vector3(previousX, previousY);
        Vector3 playerDestination = new Vector3(playerX, playerY);
        currentTransform.position = new Vector3(-previousX, -previousY);
        while (previousTransform.position != previousDestination && currentTransform.position != Vector3.zero)
        {
            player.position = Vector3.MoveTowards(player.position, playerDestination, Time.deltaTime * PAN_SPEED);
            previousTransform.position = Vector3.MoveTowards(previousTransform.position, previousDestination, Time.deltaTime * PAN_SPEED);
            currentTransform.position = Vector3.MoveTowards(currentTransform.position, Vector3.zero, Time.deltaTime * PAN_SPEED);
            yield return null;
        }
        PreviousScreen.ToggleActive();
        SetScreenLoading(false);
    }

    public Transform GetScreen(string screenId)
    {
        return screenId != null ? ScreensContainer.Find(screenId) : null;
    }

    /// <summary>
    /// If a transition is passed in, then that means we have extra scene configurations to add... this is handy
    /// for when we're going into shops.  We have a default Shop file that has the border, but then the transition
    /// config contains what character is in there/what items are shown
    /// </summary>
    /// <param name="screenId"></param>
    /// <param name="transition"></param>
    public void BuildScreen(SceneViewModel transition)
    {
        string screenId = GetScreenId(transition);
        PreviousScreen = CurrentScreen;
        Transform parent = GetScreen(screenId);
        // Parent has not been built, so let's build and cache it
        if (parent == null)
        {
            parent = Instantiate(ScenePrefab);
            parent.SetParent(ScreensContainer);
            CurrentScreen = parent.gameObject.GetComponent<WorldScreenTile>();
            CurrentScreen.Initialize(screenId, transition);
        }
        else
        {
            CurrentScreen = parent.gameObject.GetComponent<WorldScreenTile>();
        }
        CurrentScreen.ToggleActive(true);
        Camera.main.backgroundColor = CurrentScreen.GroundColor;
    }

    public string GetScreenId(SceneViewModel transition)
    {
        string screenId = transition.Name;
        if (screenId == null)
        {
            CurrentX += transition.X;
            CurrentY += transition.Y;
            screenId = $"{CurrentX}{CurrentY}";
        }
        else if (screenId == Constants.TRANSITION_BACK)
        {
            screenId = $"{CurrentX}{CurrentY}";
        }
        return screenId;
    }

    /// <summary>
    /// We have to have a "Start" method for our coroutine because if we're disabling the game object, we can't
    /// spawn the StartCoroutine from a game object that will be soon disabled, as we'll get some strange effects
    /// happening... per https://answers.unity.com/questions/1324429/coroutine-couldnt-be-started-because-the-the-game-4.html
    /// </summary>
    /// <param name="transition"></param>
    public void StartExitDoor(SceneViewModel transition)
    {
        StartCoroutine(ExitDoor(transition));
    }

    public IEnumerator ExitDoor(SceneViewModel transition)
    {
        SetScreenLoading(true);
        BuildScreen(transition);
        PreviousScreen.ToggleActive();
        GameHandler.Player.transform.position = OverworldPosition;
        yield return StartCoroutine(GameHandler.Player.characterAnimation.Exit());
        CurrentScreen.ToggleDoor(false);
        SetScreenLoading(false);
    }

    public void StartEnterDoor(SceneViewModel transition)
    {
        StartCoroutine(EnterDoor(transition));
    }

    public IEnumerator EnterDoor(SceneViewModel transition)
    {
        SetScreenLoading(true);
        yield return StartCoroutine(GameHandler.Player.characterAnimation.Enter());
        // Save off the player's current position, so we can restore it later
        OverworldPosition = GameHandler.Player.transform.position;
        BuildScreen(transition);
        PreviousScreen.ToggleActive();
        GameHandler.Player.transform.position = CurrentScreen.Grid.GetWorldPosition(7f, TRANSITION_PADDING);
        SetScreenLoading(false);
    }

    public void SetScreenLoading(bool transitioning)
    {
        GameHandler.IsTransitioning = transitioning;
    }

    public void StartPanScreen(SceneViewModel transition)
    {
        BuildScreen(transition);
        StartCoroutine(PanScreen(transition));
    }
}
