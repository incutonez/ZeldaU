﻿using NPCs;
using System;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public RectTransform enemyPrefab;

    public void Awake()
    {
        LoadSprites($"{Constants.PATH_SPRITES}characters");
        LoadSprites($"{Constants.PATH_SPRITES}enemies");
        enemyPrefab = LoadPrefab($"{Constants.PATH_PREFABS}Enemy");
    }

    public TWorldCharacter Spawn<TWorldCharacter, TCharacter, TEnum>(Vector3 position, TEnum characterType, Transform parent, RectTransform prefab)
        where TWorldCharacter : WorldCharacter<TCharacter>
        where TCharacter : BaseCharacter, new()
        where TEnum : Enum
    {
        RectTransform transform = Instantiate(prefab);
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.rotation = Quaternion.identity;

        TWorldCharacter worldCharacter = transform.GetComponent<TWorldCharacter>();
        TCharacter character = new TCharacter { characterType = characterType };
        worldCharacter.SetCharacter(character, LoadSprite(character.GetSpriteName()));

        return worldCharacter;
    }

    public WorldPlayer SpawnPlayer(Vector3 position, Transform parent)
    {
        RectTransform transform = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}Character"));
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.rotation = Quaternion.identity;

        WorldPlayer worldCharacter = transform.GetComponent<WorldPlayer>();
        BaseCharacter character = new BaseCharacter { characterType = Characters.Link };
        worldCharacter.SetCharacter(character, LoadSprite(character.GetSpriteName()));
        worldCharacter.InitializedCharacter();

        return worldCharacter;
    }

    public void SpawnEnemy(Vector3 position, Enemies enemyType, Transform parent)
    {
        Spawn<WorldEnemy, Enemy, Enemies>(position, enemyType, parent, enemyPrefab);
    }
}
