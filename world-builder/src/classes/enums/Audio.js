import { EnumStore } from "@/classes/EnumStore.js";

/**
 * @property ArrowBoomerang
 * @property BombBlow
 * @property BombDrop
 * @property BossManhandlaDigdoggerPatra
 * @property BossDragonGanon
 * @property BossDodongoGohma
 * @property BossHurt
 * @property DoorUnlock
 * @property EnemyDie
 * @property EnemyHurt
 * @property Fire
 * @property Flute
 * @property HealthLow
 * @property HeartPickup
 * @property ItemAppear
 * @property ItemFanfare
 * @property ItemPickup
 * @property Magic
 * @property Rupee
 * @property SecretAppear
 * @property Shield
 * @property Shore
 * @property Stairs
 * @property SwordShoot
 * @property SwordSlash
 * @property Text
 */
export const FX = new EnumStore([
  "ArrowBoomerang",
  "BombBlow",
  "BombDrop",
  "BossManhandlaDigdoggerPatra",
  "BossDragonGanon",
  "BossDodongoGohma",
  "BossHurt",
  "DoorUnlock",
  "EnemyDie",
  "EnemyHurt",
  "Fire",
  "Flute",
  "HealthLow",
  "HeartPickup",
  "ItemAppear",
  "ItemFanfare",
  "ItemPickup",
  "Magic",
  "Rupee",
  "SecretAppear",
  "Shield",
  "Shore",
  "Stairs",
  "SwordShoot",
  "SwordSlash",
  "Text"
]);

/**
 * @property Castle
 * @property Ending
 * @property FinalCastle
 * @property GameOver
 * @property GanonAppear
 * @property Intro
 * @property Overworld
 */
export const Music = new EnumStore([
  "Castle",
  "Ending",
  "FinalCastle",
  "GameOver",
  "GanonAppear",
  "Intro",
  "Overworld"
]);