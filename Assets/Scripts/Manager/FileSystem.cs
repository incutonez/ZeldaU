using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Manager
{
    public static class FileSystem
    {
        private static int LoadCount { get; set; } = 0;
        private static List<AssetReference> AssetReferences { get; set; }
        private static Dictionary<AssetReference, List<GameObject>> AssetSprites { get; set; } = new Dictionary<AssetReference, List<GameObject>>();
        private static Dictionary<AssetReference, AsyncOperationHandle<GameObject>> OperationHandles { get; set; } = new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
        private static Dictionary<AssetReference, Queue<Vector3>> AssetQueue { get; set; } = new Dictionary<AssetReference, Queue<Vector3>>();

        public static IEnumerator LoadJson(string path, Action<string> callback)
        {
            string result = String.Empty;
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(path);
            yield return handle;
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    result = handle.Result.text;
                    Addressables.Release(handle);
                    break;
                case AsyncOperationStatus.Failed:
                    Debug.LogError("Failed to load json file.");
                    break;
                default:
                    break;
            }
            callback(result);
        }

        public static void ShouldLaunch()
        {
            if (LoadCount == 0)
            {
                // All Assets loaded, so let's launch the game
                GameObject.Find("GameHandler").GetComponent<Game>().StartLaunch();
            }
        }

        public static void LoadAudioClips(string labelName, Action<Dictionary<string, AudioClip>> callback)
        {
            LoadCount++;
            Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
            AsyncOperationHandle<IList<IResourceLocation>> labelOperation = Addressables.LoadResourceLocationsAsync(labelName);
            labelOperation.Completed += (labelResponse) => {
                int totalCount = labelResponse.Result.Count;
                foreach (IResourceLocation item in labelResponse.Result)
                {
                    AsyncOperationHandle<AudioClip> resourceOperation = Addressables.LoadAssetAsync<AudioClip>(item.PrimaryKey);
                    resourceOperation.Completed += (result) =>
                    {
                        totalCount--;
                        switch (labelResponse.Status)
                        {
                            case AsyncOperationStatus.Succeeded:
                                clips.Add(result.Result.name, result.Result);
                                Addressables.Release(resourceOperation);
                                break;
                            case AsyncOperationStatus.Failed:
                                Debug.LogError("Failed to load audio clips.");
                                break;
                            default:
                                break;
                        }
                        // When we've finished loading all items in the directory, let's continue
                        if (totalCount == 0)
                        {
                            LoadCount--;
                            callback(clips);
                            ShouldLaunch();
                        }
                    };
                }
            };
        }

        public static void LoadPrefab(string name, Action<RectTransform> callback)
        {
            LoadCount++;
            var operation = Addressables.LoadAssetAsync<GameObject>($"{Constants.PATH_PREFABS}{name}");
            operation.Completed += (response) =>
            {
                LoadCount--;
                switch (response.Status)
                {
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

        public static void LoadSprites(string name, Action<List<Sprite>> callback)
        {
            LoadCount++;
            var operation = Addressables.LoadAssetAsync<Sprite[]>($"{Constants.PATH_SPRITES}{name}");
            operation.Completed += (response) =>
            {
                LoadCount--;
                switch (response.Status)
                {
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

        // TODOJEF: Potentially implement this for perf... right now, we're just loading all into memory and using that, but this is more of an
        // on demand system, which would be more optimal
        // Taken from https://www.youtube.com/watch?v=uNpBS0LPhaU
        public static void LoadAsset(int index)
        {
            AssetReference reference = AssetReferences[index];
            if (!reference.RuntimeKeyIsValid())
            {
                Debug.Log("DANGER!");
                return;
            }

            if (OperationHandles.ContainsKey(reference))
            {
                if (OperationHandles[reference].IsDone)
                {
                    SpawnObject(reference);
                }
                else
                {
                    //AssetQueue.Add(reference, )
                }
                return;
            }

            var op = Addressables.LoadAssetAsync<GameObject>(reference);
            // TODO: Add to dictionary?
            op.Completed += (operation) =>
            {
                if (AssetQueue.ContainsKey(reference))
                {
                    while (AssetQueue[reference]?.Any() == true)
                    {
                        Vector3 position = AssetQueue[reference].Dequeue();
                        SpawnObject(reference);
                    }
                }
            };
        }

        public static void SpawnObject(AssetReference reference)
        {
            reference.InstantiateAsync().Completed += (operation) =>
            {
                if (!AssetSprites.ContainsKey(reference))
                {
                    AssetSprites.Add(reference, new List<GameObject>());
                }
                AssetSprites[reference].Add(operation.Result);
                NotifyOnDestroy notify = operation.Result.AddComponent<NotifyOnDestroy>();
                notify.Destroyed += Remove;
                notify.AssetReference = reference;
            };
        }

        private static void Remove(AssetReference reference, NotifyOnDestroy obj)
        {
            Addressables.ReleaseInstance(obj.gameObject);
            AssetSprites[reference].Remove(obj.gameObject);
            if (AssetSprites[reference].Count == 0)
            {
                if (OperationHandles[reference].IsValid())
                {
                    Addressables.Release(OperationHandles[reference]);
                }
                OperationHandles.Remove(reference);
            }
        }
    }
}
