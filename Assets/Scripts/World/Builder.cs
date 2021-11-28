using System;
using Audio;
using System.Collections;
using UnityEngine;

namespace World {
  /// <summary>
  /// The camera is set to 7.5 for its size because it's Camera ortographic size = vertical resolution (240) / PPU (16) / 2 = 7.5
  /// per https://hackernoon.com/making-your-pixel-art-game-look-pixel-perfect-in-unity3d-3534963cad1d
  /// </summary>
  public class Builder : MonoBehaviour {
    public const int PAN_SPEED = 15;
    private const float TRANSITION_PADDING = 0.08f;

    public Transform ScreensContainer { get; set; }
    public bool InCastle { get; set; }
    public string CurrentCastle { get; set; }
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }

    private Vector3 OverworldPosition { get; set; } = Vector3.zero;
    private Screen CurrentScreen { get; set; }
    private Screen PreviousScreen { get; set; }

    public void Awake() {
      if (!Manager.Game.IsDebugMode) {
        ScreensContainer = GameObject.Find("Screens").transform;
      }
    }

    /// <summary>
    /// This is the transitioning effect that takes place between screens... it slides the next screen into view
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public IEnumerator PanScreen(ViewModel.Grid transition) {
      SetScreenLoading(true);
      yield return BuildScreen(transition);
      var grid = PreviousScreen.Grid;
      int x = transition.X;
      int y = transition.Y;
      Transform currentTransform = CurrentScreen.transform;
      Transform previousTransform = PreviousScreen.transform;
      Transform player = Manager.Game.Player.transform;
      float previousX = 0f;
      float previousY = 0f;
      float playerX = player.position.x;
      float playerY = player.position.y;
      // Moving to right screen
      if (x == 1) {
        previousX = -Constants.GridColumns;
        playerX = grid.GetWorldPositionX(TRANSITION_PADDING);
      }
      // Moving to left screen
      else if (x == -1) {
        previousX = Constants.GridColumns;
        playerX = grid.GetWorldPositionX(Constants.GridColumnsZero - TRANSITION_PADDING);
      }

      // Moving to top screen
      if (y == 1) {
        previousY = -Constants.GridRows;
        playerY = grid.GetWorldPositionY(TRANSITION_PADDING);
      }
      // Moving to bottom screen
      else if (y == -1) {
        previousY = Constants.GridRows;
        playerY = grid.GetWorldPositionY(Constants.GridRowsZero - TRANSITION_PADDING);
      }

      Vector3 previousDestination = new Vector3(previousX, previousY);
      Vector3 playerDestination = new Vector3(playerX, playerY);
      currentTransform.position = new Vector3(-previousX, -previousY);
      while (previousTransform.position != previousDestination && currentTransform.position != Vector3.zero) {
        player.position = Vector3.MoveTowards(player.position, playerDestination, Time.deltaTime * PAN_SPEED);
        previousTransform.position = Vector3.MoveTowards(previousTransform.position, previousDestination, Time.deltaTime * PAN_SPEED);
        currentTransform.position = Vector3.MoveTowards(currentTransform.position, Vector3.zero, Time.deltaTime * PAN_SPEED);
        yield return null;
      }

      CurrentScreen.SpawnEnemies();
      PreviousScreen.ToggleActive();
      SetScreenLoading(false);
    }

    public Transform GetScreen(string screenId) {
      // TODO: Have to fix this... for castles, it generates a new one each time because the find is trying to find the slashes for
      // a nested child, but that's not how it is... need to figure out like a common name or something
      return screenId != null ? ScreensContainer.Find(screenId) : null;
    }

    /// <summary>
    /// If a transition is passed in, then that means we have extra scene configurations to add... this is handy
    /// for when we're going into shops.  We have a default Shop file that has the border, but then the transition
    /// config contains what character is in there/what items are shown
    /// </summary>
    /// <param name="screenId"></param>
    /// <param name="transition"></param>
    public IEnumerator BuildScreen(ViewModel.Grid transition) {
      string screenId = GetScreenId(transition);
      PreviousScreen = CurrentScreen;
      Transform parent = GetScreen(screenId);
      // Parent has not been built, so let's build and cache it
      if (parent == null) {
        parent = Instantiate(Manager.Game.Graphics.WorldScreen);
        parent.SetParent(ScreensContainer);
        CurrentScreen = parent.gameObject.GetComponent<Screen>();
        yield return CurrentScreen.Initialize(screenId, transition);
      }
      else {
        CurrentScreen = parent.gameObject.GetComponent<Screen>();
      }

      if (CurrentScreen.GroundColor.HasValue) {
        Camera.main.backgroundColor = CurrentScreen.GroundColor.Value;
      }

      Manager.Game.Pathfinder.Grid = CurrentScreen.Grid;
      CurrentScreen.ToggleActive(true);
    }

    public void SetCastleMaterial() {
      // We have to copy the material here, so it doesn't overwrite the resource
      Material material = new Material(Manager.Game.Graphics.CastleMaterials);
      Texture2D texture = Instantiate(material.mainTexture as Texture2D);
      Utilities.ReplaceColors(texture, new Color[] {
        // TODO: Use CurrentCastle to determine these colors
        EnemyHelper.ACCENT_COLOR, Utilities.HexToColor("008088"),
        EnemyHelper.CASTLE_DOOR_COLOR, Utilities.HexToColor("183c5c"),
        EnemyHelper.BODY_COLOR, Utilities.HexToColor("00e8d8")
      });
      material.mainTexture = texture;
      Manager.Game.Graphics.CurrentCastleMaterial = material;
    }

    public string GetScreenId(ViewModel.Grid transition) {
      string screenId = transition.Name;
      if (transition.IsCastle) {
        InCastle = true;
        CurrentCastle = screenId;
        CurrentX = transition.X;
        CurrentY = transition.Y;
        screenId = $"{CurrentX}{CurrentY}";
        SetCastleMaterial();
      }

      if (string.IsNullOrEmpty(screenId)) {
        CurrentX += transition.X;
        CurrentY += transition.Y;
        screenId = $"{CurrentX}{CurrentY}";
      }
      else if (screenId == Constants.TransitionBack) {
        InCastle = false;
        if (CurrentScreen.ViewModel.IsFloating) {
          CurrentX = CurrentScreen.ViewModel.X;
          CurrentY = CurrentScreen.ViewModel.Y;
        }
        else {
          CurrentX = transition.X;
          CurrentY = transition.Y;
        }

        screenId = $"{CurrentX}{CurrentY}";
      }

      if (InCastle) {
        screenId = $"{Constants.PathCastle}{CurrentCastle}_{screenId}";
      }
      else {
        InCastle = false;
        screenId = $"{Constants.PathOverworld}{screenId}";
      }

      return screenId;
    }

    /// <summary>
    /// We have to have a "Start" method for our coroutine because if we're disabling the game object, we can't
    /// spawn the StartCoroutine from a game object that will be soon disabled, as we'll get some strange effects
    /// happening... per https://answers.unity.com/questions/1324429/coroutine-couldnt-be-started-because-the-the-game-4.html
    /// </summary>
    /// <param name="transition"></param>
    public void StartExitDoor(ViewModel.Grid transition) {
      StartCoroutine(ExitDoor(transition));
    }

    public IEnumerator ExitDoor(ViewModel.Grid transition) {
      SetScreenLoading(true);
      yield return BuildScreen(transition);
      PreviousScreen.ToggleActive();
      Manager.Game.Player.transform.position = OverworldPosition;
      Manager.Game.Audio.PlayFX(FX.Stairs);
      yield return StartCoroutine(Manager.Game.Player.AnimateExit());
      CurrentScreen.ToggleDoor(false);
      SetScreenLoading(false);
    }

    public void StartEnterDoor(ViewModel.Grid transition) {
      StartCoroutine(EnterDoor(transition));
    }

    public IEnumerator EnterDoor(ViewModel.Grid transition) {
      SetScreenLoading(true);
      Manager.Game.Audio.PlayFX(FX.Stairs);
      yield return StartCoroutine(Manager.Game.Player.AnimateEnter());
      // Save off the player's current position, so we can restore it later
      OverworldPosition = Manager.Game.Player.transform.position;
      yield return BuildScreen(transition);
      PreviousScreen.ToggleActive();
      Manager.Game.Player.transform.position = CurrentScreen.Grid.GetWorldPosition(7f, TRANSITION_PADDING);
      SetScreenLoading(false);
    }

    public void SetScreenLoading(bool transitioning) {
      Manager.Game.IsPaused = transitioning;
    }

    public void StartPanScreen(ViewModel.Grid transition) {
      StartCoroutine(PanScreen(transition));
    }
  }
}