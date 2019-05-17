﻿using BattleTech;
using BattleTech.UI;
using Harmony;

namespace IRTweaks {

    [HarmonyPatch(typeof(SelectionStateSensorLock))]
    [HarmonyPatch("ConsumesFiring", MethodType.Getter)]
    public static class SelectionStateSensorLock_ConsumesFiring {

        public static void Postfix(SelectionStateSensorLock __instance, ref bool __result) {
            Mod.Log.Debug("SSSL:CF:GET entered");
            __result = false;
        }
    }

    [HarmonyPatch(typeof(SelectionStateSensorLock))]
    [HarmonyPatch("ConsumesMovement", MethodType.Getter)]
    public static class SelectionStateSensorLock_ConsumesMovement {

        public static void Postfix(SelectionStateSensorLock __instance, ref bool __result) {
            Mod.Log.Debug("SSSL:CM:GET entered");
            __result = false;
        }
    }

    [HarmonyPatch(typeof(SensorLockSequence))]
    [HarmonyPatch("CompleteOrders")]
    public static class SensorLockSequence_CompleteOrders {

        public static bool Prefix(SensorLockSequence __instance, AbstractActor ___owningActor) {
            //Mod.Log.Debug("SLS:CO entered, aborting invocation");
            //Mod.Log.Debug($"  oa:{___owningActor.DisplayName}_{___owningActor.GetPilot().Name} hasFired:{___owningActor.HasFiredThisRound} hasMoved:{___owningActor.HasMovedThisRound} hasActivated:{___owningActor.HasActivatedThisRound}");
            return false;
        }
    }

    [HarmonyPatch(typeof(SensorLockSequence))]
    [HarmonyPatch("ConsumesFiring", MethodType.Getter)]
    public static class SensorLockSequence_ConsumesFiring {

        public static void Postfix(SensorLockSequence __instance, ref bool __result, AbstractActor ___owningActor) {
            Mod.Log.Debug("SLS:CF:GET entered.");
            Mod.Log.Debug($"    oa:{___owningActor.DisplayName}_{___owningActor.GetPilot().Name} hasFired:{___owningActor.HasFiredThisRound} hasMoved:{___owningActor.HasMovedThisRound} hasActivated:{___owningActor.HasActivatedThisRound}");
            __result = false;
        }
    }

    [HarmonyPatch(typeof(SensorLockSequence))]
    [HarmonyPatch("ConsumesMovement", MethodType.Getter)]
    public static class SensorLockSequence_ConsumesMovement {

        public static void Postfix(SensorLockSequence __instance, ref bool __result, AbstractActor ___owningActor) {
            Mod.Log.Debug("SLS:CM:GET entered.");
            Mod.Log.Debug($"    oa:{___owningActor.DisplayName}_{___owningActor.GetPilot().Name} hasFired:{___owningActor.HasFiredThisRound} hasMoved:{___owningActor.HasMovedThisRound} hasActivated:{___owningActor.HasActivatedThisRound}");
            __result = false;
        }
    }

    [HarmonyPatch(typeof(OrderSequence))]
    [HarmonyPatch("OnComplete")]
    public static class OrderSequence_OnComplete {

        public static bool Prefix(OrderSequence __instance, AbstractActor ___owningActor) {

            if (__instance is SensorLockSequence) {
                Mod.Log.Debug($"OS:OC entered, cm:{__instance.ConsumesMovement} cf:{__instance.ConsumesFiring}");
                Mod.Log.Debug($"    oa:{___owningActor.DisplayName}_{___owningActor.GetPilot().Name} hasFired:{___owningActor.HasFiredThisRound} hasMoved:{___owningActor.HasMovedThisRound} hasActivated:{___owningActor.HasActivatedThisRound}");
                Mod.Log.Debug($"    ca:{__instance.ConsumesActivation} fae:{__instance.ForceActivationEnd}");

                Mod.Log.Debug(" SensorLockSequence, skipping.");
                return true;
            } else {
                //Mod.Log.Debug(" Not SensorLockSequence, continuing.");
                return true;
            }

        }
    }

    [HarmonyPatch(typeof(OrderSequence))]
    [HarmonyPatch("ConsumesActivation", MethodType.Getter)]
    public static class OrderSequence_ConsumesActivation {

        public static void Postfix(OrderSequence __instance, ref bool __result, AbstractActor ___owningActor) {

            if (__instance is SensorLockSequence) {
                //Mod.Log.Debug($"SLS:CA entered, cm:{__instance.ConsumesMovement} cf:{__instance.ConsumesFiring}");
                //Mod.Log.Debug($"    oa:{___owningActor.DisplayName}_{___owningActor.GetPilot().Name} hasFired:{___owningActor.HasFiredThisRound} hasMoved:{___owningActor.HasMovedThisRound} hasActivated:{___owningActor.HasActivatedThisRound}");
                if (___owningActor.HasFiredThisRound && ___owningActor.HasMovedThisRound) {
                    Mod.Log.Debug(" Owner has moved and fired, returning true.");
                    __result = false;
                } else {
                    //Mod.Log.Debug(" Returning false");
                    __result = false;
                }
            } 

        }
    }

}
