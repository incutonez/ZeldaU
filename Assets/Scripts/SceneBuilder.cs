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
    private ScreenTileVisual CurrentScreen { get; set; }
    private ScreenTileVisual PreviousScreen { get; set; }
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
        int x = transition.X;
        int y = transition.Y;
        Transform currentTransform = CurrentScreen.transform;
        Transform previousTransform = PreviousScreen.transform;
        Transform player = GameHandler.Player.transform;
        float previousX = 0f;
        float previousY = 0f;
        float playerX = player.localPosition.x;
        float playerY = player.localPosition.y;
        if (x == 1)
        {
            previousX = -Constants.GRID_COLUMNS;
            playerX = TRANSITION_PADDING;
        }
        else if (x == -1)
        {
            previousX = Constants.GRID_COLUMNS;
            playerX = Constants.GRID_COLUMNS_ZERO - TRANSITION_PADDING;
        }
        if (y == 1)
        {
            previousY = -Constants.GRID_ROWS;
            playerY = TRANSITION_PADDING;
        }
        else if (y == -1)
        {
            previousY = Constants.GRID_ROWS;
            playerY = Constants.GRID_ROWS_ZERO - TRANSITION_PADDING;
        }
        Vector3 previousDestination = new Vector3(previousX, previousY);
        Vector3 playerDestination = new Vector3(playerX, playerY);
        currentTransform.localPosition = new Vector3(-previousX, -previousY);
        while (previousTransform.localPosition != previousDestination && currentTransform.localPosition != Vector3.zero)
        {
            player.localPosition = Vector3.MoveTowards(player.localPosition, playerDestination, Time.deltaTime * PAN_SPEED);
            previousTransform.localPosition = Vector3.MoveTowards(previousTransform.localPosition, previousDestination, Time.deltaTime * PAN_SPEED);
            currentTransform.localPosition = Vector3.MoveTowards(currentTransform.localPosition, Vector3.zero, Time.deltaTime * PAN_SPEED);
            yield return null;
        }
        PreviousScreen.gameObject.SetActive(false);
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
    public void BuildScreen(string screenId, SceneViewModel transition)
    {
        Transform parent = GetScreen(screenId);
        // Parent has not been built, so let's build and cache it
        if (parent == null)
        {
            parent = Instantiate(ScenePrefab);
            parent.SetParent(ScreensContainer);
            CurrentScreen = parent.gameObject.GetComponent<ScreenTileVisual>().Initialize(screenId);
            if (transition != null)
            {
                // TODOJEF: ADD TRANSITION LOGIC
                //CurrentScreen.Build(transition);
            }
        }
        else
        {
            CurrentScreen = parent.gameObject.GetComponent<ScreenTileVisual>();
        }
        Camera.main.backgroundColor = CurrentScreen.GroundColor;
    }

    public string GetScreenId(WorldMatter worldMatter)
    {
        SceneViewModel transition = worldMatter.Transition;
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

    public IEnumerator EnterDoor(WorldMatter worldMatter)
    {
        SetScreenLoading(true);
        // TODOJEF: IMPL
        //CurrentScreen.ToggleDoor(true);
        yield return StartCoroutine(GameHandler.Player.characterAnimation.Enter());
        yield return StartCoroutine(LoadScreen(worldMatter));
        // TODOJEF: I don't like dipping into the animator like this... figure out a better way
        GameHandler.Player.characterAnimation.animator.SetBool("isEntering", false);
        SetScreenLoading(false);
    }

    public void SetScreenLoading(bool transitioning)
    {
        GameHandler.IsTransitioning = transitioning;
    }

    public IEnumerator LoadScreen(WorldMatter worldMatter)
    {
        yield return StartCoroutine(LoadScreenRoutine(worldMatter));
    }

    public IEnumerator LoadScreen(string screenId)
    {
        yield return StartCoroutine(LoadScreenRoutine(new WorldMatter { Matter = new Matter { type = Matters.Transition }, Transition = new SceneViewModel { Name = screenId } }));
    }

    // TODOJEF: Next step is to get transitions working, then doors
    public IEnumerator LoadScreenRoutine(WorldMatter worldMatter)
    {
        SetScreenLoading(true);
        SceneViewModel transition = worldMatter.Transition;
        string screenId = GetScreenId(worldMatter);
        PreviousScreen = CurrentScreen;
        BuildScreen(screenId, transition);
        if (transition != null)
        {
            CurrentScreen.gameObject.SetActive(true);
            if (transition.Name == Constants.TRANSITION_BACK)
            {
                PreviousScreen.gameObject.SetActive(false);
                // Restore the player's previous position
                GameHandler.Player.transform.localPosition = OverworldPosition;
                // TODOJEF: IMPL
                //CurrentScreen.ToggleDoor(true);
                yield return StartCoroutine(GameHandler.Player.characterAnimation.Exit());
                //CurrentScreen.ToggleDoor(false);
            }
            else if (transition.Name == "Shop")
            {
                PreviousScreen.gameObject.SetActive(false);
                if (worldMatter.CanEnter())
                {
                    // Save off the player's current position, so we can restore it later
                    OverworldPosition = GameHandler.Player.transform.localPosition;
                    GameHandler.Player.transform.localPosition = new Vector3(7f, TRANSITION_PADDING);
                }
            }
            else if (PreviousScreen != null)
            {
                yield return StartCoroutine(PanScreen(transition));
            }
        }
        else
        {
            yield return null;
        }
        SetScreenLoading(false);
    }
}
