using System.Collections;
using Enums;
using UnityEngine;

namespace World {
  /// <summary>
  /// The camera is set to 7.5 for its size because it's Camera orthographic size = vertical resolution (240) / PPU (16) / 2 = 7.5
  /// per https://hackernoon.com/making-your-pixel-art-game-look-pixel-perfect-in-unity3d-3534963cad1d
  /// </summary>
  public class Builder : MonoBehaviour {
    private const int PanSpeed = 15;
    private const float TransitionPadding = 0.08f;

    private Transform ScreensContainer { get; set; }
    public bool InCastle { get; set; }
    private string CurrentCastle { get; set; }
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
      BuildScreen(transition);
      if (PreviousScreen != null) {
        var grid = PreviousScreen.Grid;
        var x = transition.X;
        var y = transition.Y;
        var currentTransform = CurrentScreen.transform;
        var previousTransform = PreviousScreen.transform;
        var player = Manager.Game.Player.transform;
        var previousX = 0f;
        var previousY = 0f;
        var position = player.position;
        var playerX = position.x;
        var playerY = position.y;
        // Moving to right screen
        if (x == 1) {
          previousX = -Constants.GridColumns;
          playerX = grid.GetWorldPositionX(TransitionPadding);
        }
        // Moving to left screen
        else if (x == -1) {
          previousX = Constants.GridColumns;
          playerX = grid.GetWorldPositionX(Constants.GridColumnsZero - TransitionPadding);
        }

        // Moving to top screen
        if (y == 1) {
          previousY = -Constants.GridRows;
          playerY = grid.GetWorldPositionY(TransitionPadding);
        }
        // Moving to bottom screen
        else if (y == -1) {
          previousY = Constants.GridRows;
          playerY = grid.GetWorldPositionY(Constants.GridRowsZero - TransitionPadding);
        }

        var previousDestination = new Vector3(previousX, previousY);
        var playerDestination = new Vector3(playerX, playerY);
        currentTransform.position = new Vector3(-previousX, -previousY);
        while (previousTransform.position != previousDestination && currentTransform.position != Vector3.zero) {
          player.position = Vector3.MoveTowards(player.position, playerDestination, Time.deltaTime * PanSpeed);
          previousTransform.position = Vector3.MoveTowards(previousTransform.position, previousDestination, Time.deltaTime * PanSpeed);
          currentTransform.position = Vector3.MoveTowards(currentTransform.position, Vector3.zero, Time.deltaTime * PanSpeed);
          yield return null;
        }

        PreviousScreen.ToggleActive();
      }

      CurrentScreen.SpawnEnemies();

      SetScreenLoading(false);
    }

    private Transform GetScreen(string screenId) {
      // TODO: Have to fix this... for castles, it generates a new one each time because the find is trying to find the slashes for
      // a nested child, but that's not how it is... need to figure out like a common name or something
      return screenId == null ? null : ScreensContainer.Find(screenId);
    }

    /// <summary>
    /// If a transition is passed in, then that means we have extra scene configurations to add... this is handy
    /// for when we're going into shops.  We have a default Shop file that has the border, but then the transition
    /// config contains what character is in there/what items are shown
    /// </summary>
    /// <param name="screenId"></param>
    /// <param name="transition"></param>
    private void BuildScreen(ViewModel.Grid transition) {
      var screenId = GetScreenId(transition);
      PreviousScreen = CurrentScreen;
      var parent = GetScreen(screenId);
      // Parent has not been built, so let's build and cache it
      if (parent == null) {
        parent = Instantiate(Manager.Game.Graphics.WorldScreen, ScreensContainer);
        CurrentScreen = parent.gameObject.GetComponent<Screen>();
        CurrentScreen.Initialize(screenId, transition);
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

    private void SetCastleMaterial() {
      // We have to copy the material here, so it doesn't overwrite the resource
      var material = new Material(Manager.Game.Graphics.CastleMaterials);
      var texture = Instantiate(material.mainTexture as Texture2D);
      Utilities.ReplaceColors(texture, new[] {
        // TODO: Use CurrentCastle to determine these colors
        WorldColors.RedPure.GetColor(), Utilities.HexToColor("008088"),
        WorldColors.BluePure.GetColor(), Utilities.HexToColor("183c5c"),
        WorldColors.WhitePure.GetColor(), Utilities.HexToColor("00e8d8")
      });
      material.mainTexture = texture;
      Manager.Game.Graphics.CurrentCastleMaterial = material;
    }

    private string GetScreenId(ViewModel.Grid transition) {
      var screenId = transition.Name;
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
        // screenId = $"{Constants.PathOverworld}{screenId}";
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

    private IEnumerator ExitDoor(ViewModel.Grid transition) {
      SetScreenLoading(true);
      BuildScreen(transition);
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

    private IEnumerator EnterDoor(ViewModel.Grid transition) {
      SetScreenLoading(true);
      Manager.Game.Audio.PlayFX(FX.Stairs);
      yield return StartCoroutine(Manager.Game.Player.AnimateEnter());
      // Save off the player's current position, so we can restore it later
      OverworldPosition = Manager.Game.Player.transform.position;
      BuildScreen(transition);
      PreviousScreen.ToggleActive();
      Manager.Game.Player.transform.position = CurrentScreen.Grid.GetWorldPosition(7f, TransitionPadding);
      SetScreenLoading(false);
    }

    private void SetScreenLoading(bool transitioning) {
      Manager.Game.IsPaused = transitioning;
    }

    public void StartPanScreen(ViewModel.Grid transition) {
      StartCoroutine(PanScreen(transition));
    }
  }
}
