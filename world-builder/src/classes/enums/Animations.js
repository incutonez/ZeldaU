import { EnumStore } from "@/classes/EnumStore.js";

/**
 * @property ActionDown
 * @property ActionLeft
 * @property ActionRight
 * @property ActionUp
 * @property Default
 * @property Entering
 * @property Exiting
 * @property IdleDown
 * @property IdleLeft
 * @property IdleRight
 * @property IdleUp
 * @property WalkDown
 * @property WalkLeft
 * @property WalkRight
 * @property WalkUp
 */
export const Animations = new EnumStore([
  "Default",
  "ActionUp",
  "ActionDown",
  "ActionRight",
  "ActionLeft",
  "Entering",
  "Exiting",
  "IdleUp",
  "IdleDown",
  "IdleRight",
  "IdleLeft",
  "WalkUp",
  "WalkDown",
  "WalkRight",
  "WalkLeft"
]);