using NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Manager
{
    public class Sprites
    {
        public List<Sprite> Characters { get; set; }
        public List<Sprite> Items { get; set; }
        public List<Sprite> Tiles { get; set; }
        public List<Sprite> PlayerBase { get; set; }
        public Dictionary<Enemies, Dictionary<Animations, List<Sprite>>> EnemyAnimations { get; set; } = new Dictionary<Enemies, Dictionary<Animations, List<Sprite>>>();
        public Dictionary<Animations, List<Sprite>> PlayerAnimations { get; set; } = new Dictionary<Animations, List<Sprite>>();
        public List<AssetReference> AssetReferences { get; set; }
        public Dictionary<AssetReference, List<GameObject>> AssetSprites { get; set; } = new Dictionary<AssetReference, List<GameObject>>();
        public Dictionary<AssetReference, AsyncOperationHandle<GameObject>> OperationHandles { get; set; } = new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
        public Dictionary<AssetReference, Queue<Vector3>> AssetQueue { get; set; } = new Dictionary<AssetReference, Queue<Vector3>>();
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
            LoadSprites("character", (response) =>
            {
                PlayerAnimations = GetAnimations(response, "");
                PlayerAnimations[Animations.Entering] = PlayerAnimations[Animations.WalkUp];
                PlayerAnimations[Animations.Exiting] = PlayerAnimations[Animations.WalkDown];
            });
            LoadSprites("characterBase", (response) =>
            {
                PlayerBase = response;
            });
            LoadSprites("tiles", (response) =>
            {
                Tiles = response;
            });
            LoadSprites("items", (response) =>
            {
                Items = response;
            });
            LoadSprites("characters", (response) =>
            {
                Characters = response;
            });
            LoadSprites("enemies", (response) =>
            {
                List<Animations> animationsEnums = EnumExtensions.GetValues<Animations>();
                foreach (Enemies enemy in EnumExtensions.GetValues<Enemies>())
                {
                    string enemyName = enemy.GetDescription() + "_";
                    EnemyAnimations.Add(enemy, GetAnimations(response.Where(x => x.name.Contains(enemyName)).ToList(), enemyName));
                }
            });
        }

        public Dictionary<Animations, List<Sprite>> GetAnimations(List<Sprite> animations, string name)
        {
            Dictionary<Animations, List<Sprite>> dict = new Dictionary<Animations, List<Sprite>>();
            foreach (Animations anim in EnumExtensions.GetValues<Animations>())
            {
                dict[anim] = new List<Sprite>();
            }
            foreach (Sprite sprite in animations)
            {
                string spriteName = sprite.name;
                if (name != String.Empty)
                {
                    spriteName = spriteName.Replace(name, "");
                }
                Enum.TryParse(spriteName, out Animations value);
                switch (value)
                {
                    case Animations.ActionUp:
                        dict[value].Add(sprite);
                        break;
                    case Animations.ActionDown:
                        dict[value].Add(sprite);
                        break;
                    case Animations.ActionRight:
                        dict[value].Add(sprite);
                        break;
                    case Animations.ActionLeft:
                        dict[value].Add(sprite);
                        break;
                    case Animations.IdleUp:
                        dict[value].Add(sprite);
                        dict[Animations.WalkUp].Add(sprite);
                        break;
                    case Animations.IdleDown:
                        dict[value].Add(sprite);
                        dict[Animations.WalkDown].Add(sprite);
                        break;
                    case Animations.IdleRight:
                        dict[value].Add(sprite);
                        dict[Animations.WalkRight].Add(sprite);
                        break;
                    case Animations.IdleLeft:
                        dict[value].Add(sprite);
                        dict[Animations.WalkLeft].Add(sprite);
                        break;
                    case Animations.WalkUp:
                        dict[value].Add(sprite);
                        break;
                    case Animations.WalkDown:
                        dict[value].Add(sprite);
                        break;
                    case Animations.WalkRight:
                        dict[value].Add(sprite);
                        break;
                    case Animations.WalkLeft:
                        dict[value].Add(sprite);
                        break;
                }
            }
            return dict;
        }

        public void LoadSprites(string name, Action<List<Sprite>> callback)
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
            return Characters.Find(s => s.name == name);
        }

        public Sprite GetItem(string name)
        {
            return Items.Find(s => s.name == name);
        }

        public Sprite GetItem(Items type)
        {
            return GetItem(type.GetCustomAttr("Resource"));
        }
    }
}