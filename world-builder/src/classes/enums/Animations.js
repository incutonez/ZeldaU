import { EnumStore } from "@/classes/EnumStore.js";

/**
 * @property Default
 * @property ActionUp
 * @property ActionDown
 * @property ActionRight
 * @property ActionLeft
 * @property Entering
 * @property Exiting
 * @property IdleUp
 * @property IdleDown
 * @property IdleRight
 * @property IdleLeft
 * @property WalkUp
 * @property WalkDown
 * @property WalkRight
 * @property WalkLeft
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