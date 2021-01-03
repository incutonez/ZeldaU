using NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Manager
{
    public class Sprites
    {
        public List<Sprite> Characters { get; set; }
        public List<Sprite> Enemies { get; set; }
        public List<Sprite> Items { get; set; }
        public List<Sprite> Tiles { get; set; }
        public List<Sprite> Player { get; set; }
        public Dictionary<Enemies, List<Sprite>> EnemyAnimations { get; set; }
        public List<AssetReference> AssetReferences { get; set; }
        public Dictionary<string, List<Sprite>> LoadedSprites { get; set; } = new Dictionary<string, List<Sprite>>();
        public Dictionary<AssetReference, List<GameObject>> AssetSprites { get; set; } = new Dictionary<AssetReference, List<GameObject>>();
        public Dictionary<AssetReference, AsyncOperationHandle<GameObject>> OperationHandles = new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
        public Dictionary<AssetReference, Queue<Vector3>> AssetQueue = new Dictionary<AssetReference, Queue<Vector3>>();
        private int LoadCount { get; set; } = 0;

        // Taken from https://www.youtube.com/watch?v=uNpBS0LPhaU
        public void DoThing(int index)
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

        public void SpawnObject(AssetReference reference)
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

        private void Remove(AssetReference reference, NotifyOnDestroy obj)
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

        public Sprites()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            LoadSprites("character");
            LoadSprites("characterBase");
            LoadSprites("tiles");
            LoadSprites("items");
            LoadSprites("characters");
            LoadSpriteDir("Enemies");
            EnemyAnimations = new Dictionary<Enemies, List<Sprite>>();
        }

        public void LoadSpriteDir(string dirName)
        {
            DirectoryInfo dir = new DirectoryInfo($"Assets/Sprites/{dirName}");
            FileInfo[] info = dir.GetFiles("*.png");
            List<string> files = info.Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList();
            foreach (string file in files)
            {
                LoadSprites($"{dirName}/{file}");
            }
        }

        public void LoadSprites(string name)
        {
            LoadCount++;
            var operation = Addressables.LoadAssetAsync<Sprite[]>($"{Constants.PATH_SPRITES}{name}");
            operation.Completed += (response) =>
            {
                LoadCount--;
                switch (response.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        LoadedSprites.Add(name, response.Result.ToList());
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.LogError("Failed to load sprite.");
                        break;
                    default:
                        break;
                }
                if (LoadCount == 0)
                {
                    // All Assets loaded, so let's launch the game
                    GameObject.Find("GameHandler").GetComponent<Game>().Launch();
                }
            };
        }

        public Sprite GetCharacter(string name)
        {
            return LoadedSprites["characters"].Find(s => s.name == name);
        }

        //public Sprite GetEnemy(string name)
        //{
        //    return Enemies.Find(s => s.name == name);
        //}

        public Sprite GetItem(string name)
        {
            return LoadedSprites["items"].Find(s => s.name == name);
        }

        public Sprite GetItem(Items type)
        {
            return GetItem(type.GetCustomAttr("Resource"));
        }

        // TODOJEF: Need to fix this... there's some weird caching issue with Resources.LoadAll, and once I change the name,
        // it actually copies to the resource itself and saves even on next play... maybe need to use Texture2D instead of loading
        // sprites through the resources dir?  Might need to make a material?  Or potentially split the enemies into their own sprites?
        // Look at https://learn.unity.com/tutorial/introduction-to-assetbundles#5eb00e8cedbc2a098f879179
        public void GetEnemySprites(
            Enemies enemy,
            List<Sprite> actionUp,
            List<Sprite> actionDown,
            List<Sprite> actionRight,
            List<Sprite> actionLeft,
            List<Sprite> idleUp,
            List<Sprite> idleDown,
            List<Sprite> idleRight,
            List<Sprite> idleLeft,
            List<Sprite> walkUp,
            List<Sprite> walkDown,
            List<Sprite> walkRight,
            List<Sprite> walkLeft
        )
        {
            var enemyName = enemy.GetDescription();
            if (enemy == NPCs.Enemies.Octorok)
            {
                enemyName = "OctorokBase";
            }
            var sprites = LoadedSprites[$"Enemies/{enemyName}"];
            GetCharacterSprites(
                sprites,
                actionUp,
                actionDown,
                actionRight,
                actionLeft,
                idleUp,
                idleDown,
                idleRight,
                idleLeft,
                walkUp,
                walkDown,
                walkRight,
                walkLeft
            );
        }

        public void GetPlayerSprites(
            List<Sprite> actionUp,
            List<Sprite> actionDown,
            List<Sprite> actionRight,
            List<Sprite> actionLeft,
            List<Sprite> idleUp,
            List<Sprite> idleDown,
            List<Sprite> idleRight,
            List<Sprite> idleLeft,
            List<Sprite> walkUp,
            List<Sprite> walkDown,
            List<Sprite> walkRight,
            List<Sprite> walkLeft
        )
        {
            GetCharacterSprites(
                LoadedSprites["character"],
                actionUp,
                actionDown,
                actionRight,
                actionLeft,
                idleUp,
                idleDown,
                idleRight,
                idleLeft,
                walkUp,
                walkDown,
                walkRight,
                walkLeft
            );
        }

        public void GetCharacterSprites(
            List<Sprite> sprites,
            List<Sprite> actionUp,
            List<Sprite> actionDown,
            List<Sprite> actionRight,
            List<Sprite> actionLeft,
            List<Sprite> idleUp,
            List<Sprite> idleDown,
            List<Sprite> idleRight,
            List<Sprite> idleLeft,
            List<Sprite> walkUp,
            List<Sprite> walkDown,
            List<Sprite> walkRight,
            List<Sprite> walkLeft
        )
        {
            foreach (Sprite sprite in sprites)
            {
                Enum.TryParse(sprite.name, out Animations value);
                switch (value)
                {
                    case Animations.ActionUp:
                        actionUp.Add(sprite);
                        break;
                    case Animations.ActionDown:
                        actionDown.Add(sprite);
                        break;
                    case Animations.ActionRight:
                        actionRight.Add(sprite);
                        break;
                    case Animations.ActionLeft:
                        actionLeft.Add(sprite);
                        break;
                    case Animations.IdleUp:
                        walkUp.Add(sprite);
                        idleUp.Add(sprite);
                        break;
                    case Animations.IdleDown:
                        walkDown.Add(sprite);
                        idleDown.Add(sprite);
                        break;
                    case Animations.IdleRight:
                        walkRight.Add(sprite);
                        idleRight.Add(sprite);
                        break;
                    case Animations.IdleLeft:
                        walkLeft.Add(sprite);
                        idleLeft.Add(sprite);
                        break;
                    case Animations.WalkUp:
                        walkUp.Add(sprite);
                        break;
                    case Animations.WalkDown:
                        walkDown.Add(sprite);
                        break;
                    case Animations.WalkRight:
                        walkRight.Add(sprite);
                        break;
                    case Animations.WalkLeft:
                        walkLeft.Add(sprite);
                        break;
                }
            }
        }
    }
}