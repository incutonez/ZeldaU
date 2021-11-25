import { EnumStore } from "@/classes/EnumStore.js";

/**
 * @property Fairy
 * @property Link
 * @property Merchant
 * @property OldMan
 * @property OldMan2
 * @property OldWoman
 * @property Zelda
 */
export const Characters = new EnumStore({
  Fairy: 1,
  Link: 2,
  Merchant: 3,
  OldMan: 4,
  OldMan2: 5,
  OldWoman: 6,
  Zelda: 7
});

/**
 * @property Armos
 * @property Rock
 * @property Bubble
 * @property BubbleBlue
 * @property BubbleRed
 * @property Darknut
 * @property DarknutBlue
 * @property Gel
 * @property GelBlue
 * @property Ghini
 * @property Gibdo
 * @property GleeokHead
 * @property Goriya
 * @property GoriyaBlue
 * @property Keese
 * @property KeeseBlue
 * @property KeeseRed
 * @property Lanmola
 * @property LanmolaBlue
 * @property Leever
 * @property LeeverBlue
 * @property LikeLike
 * @property Lynel
 * @property LynelBlue
 * @property Moblin
 * @property MoblinBlue
 * @property Moldorm
 * @property Octorok
 * @property OctorokBlue
 * @property Patra
 * @property PatraHead
 * @property Peahat
 * @property PolsVoice
 * @property Rope
 * @property RopeBlue
 * @property Stalfos
 * @property Tektite
 * @property TektiteBlue
 * @property Trap
 * @property Vire
 * @property Wallmaster
 * @property Wizzrobe
 * @property WizzrobeBlue
 * @property Zol
 * @property ZolGray
 * @property ZolGreen
 * @property Zora
 */
export const Enemies = new EnumStore({
  Armos: 0,
  Rock: 1,
  Bubble: 2,
  BubbleBlue: 3,
  BubbleRed: 4,
  Darknut: 5,
  DarknutBlue: 6,
  Gel: 7,
  GelBlue: 77,
  Ghini: 8,
  Gibdo: 9,
  GleeokHead: 10,
  Goriya: 11,
  GoriyaBlue: 12,
  Keese: 13,
  KeeseBlue: 14,
  KeeseRed: 15,
  Lanmola: 16,
  LanmolaBlue: 17,
  Leever: 18,
  LeeverBlue: 19,
  LikeLike: 20,
  Lynel: 21,
  LynelBlue: 22,
  Moblin: 23,
  MoblinBlue: 24,
  Moldorm: 25,
  Octorok: 26,
  OctorokBlue: 27,
  Patra: 28,
  PatraHead: 200,
  Peahat: 29,
  PolsVoice: 30,
  Rope: 31,
  RopeBlue: 32,
  Stalfos: 33,
  Tektite: 34,
  TektiteBlue: 35,
  Trap: 36,
  Vire: 37,
  Wallmaster: 38,
  Wizzrobe: 39,
  WizzrobeBlue: 40,
  Zol: 41,
  ZolGray: 410,
  ZolGreen: 411,
  Zora: 42
});

/**
 * @property Aquamentus
 * @property Digdogger
 * @property Dodongo
 * @property Ganon
 * @property Gleeok
 * @property Gohma
 * @property GohmaBlue
 * @property Manhandla
 */
export const Bosses = new EnumStore([
  "Aquamentus",
  "Digdogger",
  "Dodongo",
  "Ganon",
  "Gleeok",
  "Gohma",
  "GohmaBlue",
  "Manhandla"
]);