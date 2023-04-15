using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using Enums;

namespace Manager {
  public static class FileSystem {
    private static int LoadCount { get; set; } = 0;
    private static List<AssetReference> AssetReferences { get; set; }
    private static Dictionary<AssetReference, List<GameObject>> AssetSprites { get; set; } = new();
    private static Dictionary<AssetReference, AsyncOperationHandle<GameObject>> OperationHandles { get; set; } = new();
    private static Dictionary<AssetReference, Queue<Vector3>> AssetQueue { get; set; } = new();

    public static void ShouldLaunch() {
      if (LoadCount == 0) {
        // All Assets loaded, so let's launch the game
        GameObject.Find("GameHandler").GetComponent<Game>().StartLaunch();
      }
    }

    public static IEnumerator LoadJson(string path, Action<string> callback) {
      var result = String.Empty;
      var handle = Addressables.LoadAssetAsync<TextAsset>(path);
      yield return handle;
      switch (handle.Status) {
        case AsyncOperationStatus.Succeeded:
          result = handle.Result.text;
          Addressables.Release(handle);
          break;
        case AsyncOperationStatus.Failed:
          Debug.LogError($"Failed to load json file. {path}");
          break;
        default:
          break;
      }

      callback(result);
    }

    // TODO: Need to figure out a better way of codifying all of these methods that are eerily similar
    /// <summary>
    /// To use this method, an address label must be created, and that can be done through selecting the
    /// directory in the Unity editor, making it Addressable, and then clicking the "Select" button,
    /// which will allow adding a new label.  Once that label is added, you can load the entire directory.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="callback"></param>
    public static void LoadJsonByLabel(string label, Action<Dictionary<string, string>> callback) {
      LoadCount++;
      var labelOperation = Addressables.LoadResourceLocationsAsync(label);
      var items = new Dictionary<string, string>();
      labelOperation.Completed += (labelResponse) => {
        var totalCount = labelResponse.Result.Count;
        foreach (var item in labelResponse.Result) {
          var resourceOperation = Addressables.LoadAssetAsync<TextAsset>(item.PrimaryKey);
          resourceOperation.Completed += (result) => {
            totalCount--;
            switch (labelResponse.Status) {
              case AsyncOperationStatus.Succeeded:
                items.Add(Path.GetFileNameWithoutExtension(item.PrimaryKey), result.Result.text);
                Addressables.Release(resourceOperation);
                break;
              case AsyncOperationStatus.Failed:
                Debug.LogError($"Failed to load json for {label}.");
                break;
              default:
                break;
            }

            // When we've finished loading all items in the directory, let's continue
            if (totalCount == 0) {
              LoadCount--;
              callback(items);
              ShouldLaunch();
            }
          };
        }
      };
    }

    public static void LoadAudioClips(string labelName, Action<Dictionary<FX, AudioClip>> callback) {
      LoadCount++;
      var clips = new Dictionary<FX, AudioClip>();
      var labelOperation = Addressables.LoadResourceLocationsAsync(labelName);
      labelOperation.Completed += (labelResponse) => {
        var totalCount = labelResponse.Result.Count;
        foreach (var item in labelResponse.Result) {
          var resourceOperation = Addressables.LoadAssetAsync<AudioClip>(item.PrimaryKey);
          resourceOperation.Completed += (result) => {
            totalCount--;
            switch (labelResponse.Status) {
              case AsyncOperationStatus.Succeeded:
                Enum.TryParse(result.Result.name, out FX audioType);
                clips.Add(audioType, result.Result);
                Addressables.Release(resourceOperation);
                break;
              case AsyncOperationStatus.Failed:
                Debug.LogError("Failed to load audio clips.");
                break;
              default:
                break;
            }

            // When we've finished loading all items in the directory, let's continue
            if (totalCount == 0) {
              LoadCount--;
              callback(clips);
              ShouldLaunch();
            }
          };
        }
      };
    }

    public static void LoadPrefab(string name, Action<RectTransform> callback) {
      LoadCount++;
      var operation = Addressables.LoadAssetAsync<GameObject>($"{Constants.PathPrefabs}{name}");
      operation.Completed += (response) => {
        LoadCount--;
        switch (response.Status) {
          case AsyncOperationStatus.Succeeded:
            callback(response.Result.GetComponent<RectTransform>());
            Addressables.Release(operation);
            break;
          case AsyncOperationStatus.Failed:
            Debug.LogError("Failed to load prefab.");
            break;
          default:
            break;
        }

        ShouldLaunch();
      };
    }

    public static void LoadSpritesLabel(string label, Action<Dictionary<Enemies, List<Sprite>>> callback) {
      LoadCount++;
      var items = new Dictionary<Enemies, List<Sprite>>();
      var labelOperation = Addressables.LoadResourceLocationsAsync(label);
      labelOperation.Completed += (labelResponse) => {
        var totalCount = labelResponse.Result.Count;
        foreach (var item in labelResponse.Result) {
          var resourceOperation = Addressables.LoadAssetAsync<Sprite[]>(item.PrimaryKey);
          resourceOperation.Completed += (result) => {
            totalCount--;
            switch (labelResponse.Status) {
              case AsyncOperationStatus.Succeeded:
                Enum.TryParse(Path.GetFileNameWithoutExtension(item.PrimaryKey), out Enemies enemyType);
                items.Add(enemyType, result.Result.ToList());
                Addressables.Release(resourceOperation);
                break;
              case AsyncOperationStatus.Failed:
                Debug.LogError("Failed to load audio clips.");
                break;
              default:
                break;
            }

            // When we've finished loading all items in the directory, let's continue
            if (totalCount == 0) {
              LoadCount--;
              callback(items);
              ShouldLaunch();
            }
          };
        }
      };
    }

    public static void LoadMaterial(string name, Action<Material> callback) {
      LoadCount++;
      var operation = Addressables.LoadAssetAsync<Material>($"{Constants.PathMaterials}{name}");
      operation.Completed += (response) => {
        LoadCount--;
        switch (response.Status) {
          case AsyncOperationStatus.Succeeded:
            callback(response.Result);
            Addressables.Release(operation);
            break;
          case AsyncOperationStatus.Failed:
            Debug.LogError("Failed to load sprite.");
            break;
          default:
            break;
        }

        ShouldLaunch();
      };
    }

    public static void LoadSprites(string name, Action<List<Sprite>> callback) {
      LoadCount++;
      var operation = Addressables.LoadAssetAsync<Sprite[]>($"{Constants.PathSprites}{name}");
      operation.Completed += (response) => {
        LoadCount--;
        switch (response.Status) {
          case AsyncOperationStatus.Succeeded:
            callback(response.Result.ToList());
            Addressables.Release(operation);
            break;
          case AsyncOperationStatus.Failed:
            Debug.LogError("Failed to load sprite.");
            break;
          default:
            break;
        }

        ShouldLaunch();
      };
    }

    // TODO: Potentially implement this for perf... right now, we're just loading all into memory and using that, but this is more of an
    // on demand system, which would be more optimal
    // Taken from https://www.youtube.com/watch?v=uNpBS0LPhaU
    public static void LoadAsset(int index) {
      var reference = AssetReferences[index];
      if (!reference.RuntimeKeyIsValid()) {
        Debug.Log("DANGER!");
        return;
      }

      if (OperationHandles.ContainsKey(reference)) {
        if (OperationHandles[reference].IsDone) {
          SpawnObject(reference);
        }
        else {
          //AssetQueue.Add(reference, )
        }

        return;
      }

      var op = Addressables.LoadAssetAsync<GameObject>(reference);
      // TODO: Add to dictionary?
      op.Completed += (operation) => {
        if (AssetQueue.ContainsKey(reference)) {
          while (AssetQueue[reference]?.Any() == true) {
            Vector3 position = AssetQueue[reference].Dequeue();
            SpawnObject(reference);
          }
        }
      };
    }

    public static void SpawnObject(AssetReference reference) {
      reference.InstantiateAsync().Completed += (operation) => {
        if (!AssetSprites.ContainsKey(reference)) {
          AssetSprites.Add(reference, new List<GameObject>());
        }

        AssetSprites[reference].Add(operation.Result);
        var notify = operation.Result.AddComponent<NotifyOnDestroy>();
        notify.Destroyed += Remove;
        notify.AssetReference = reference;
      };
    }

    private static void Remove(AssetReference reference, NotifyOnDestroy obj) {
      Addressables.ReleaseInstance(obj.gameObject);
      AssetSprites[reference].Remove(obj.gameObject);
      if (AssetSprites[reference].Count == 0) {
        if (OperationHandles[reference].IsValid()) {
          Addressables.Release(OperationHandles[reference]);
        }

        OperationHandles.Remove(reference);
      }
    }
  }
}
