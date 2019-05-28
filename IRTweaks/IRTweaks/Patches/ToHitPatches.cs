﻿using BattleTech;
using BattleTech.UI;
using Harmony;
using Localize;

namespace IRTweaks {

    [HarmonyPatch(typeof(ToHit))]
    [HarmonyPatch("GetMoraleAttackModifier")]
    public static class ToHit_GetMoraleAttackModifier {

        public static void Postfix(ToHit __instance, ICombatant target, bool isMoraleAttack, ref float __result) {
            __result = 0;
        }
    }

   [HarmonyPatch(typeof(ToHit), "GetAllModifiers")]
    public static class ToHit_GetAllModifiers {

        public static void Postfix(ToHit __instance, ref float __result, bool isCalledShot, AbstractActor attacker, Weapon weapon, ICombatant target) {
            if (isCalledShot) {
                Mod.Log.Trace("TH:GAM entered.");

                // Calculate called shot modifier
                int pilotValue = PilotHelper.GetCalledShotModifier(attacker.GetPilot());
                // TODO: Find items that could apply a modifier
                //Statistic calledShotModStat = State.CurrentAttacker?.StatCollection.GetStatistic(ModStats.CalledShotMod);
                //int unitMod = calledShotModStat != null ? calledShotModStat.Value<int>() : 0;
                int calledShotMod = pilotValue;
                Mod.Log.Debug($" Called Shot from pilot:{attacker.GetPilot().Name} => pilotValue:{pilotValue} + unitMod:0 = calledShotMod:{calledShotMod}");

                __result = __result + calledShotMod;
            }
        }
    }

    [HarmonyPatch(typeof(ToHit), "GetAllModifiersDescription")]
    public static class ToHit_GetAllModifiersDescription {

        public static void Postfix(ToHit __instance, ref string __result, bool isCalledShot, AbstractActor attacker, Weapon weapon, ICombatant target) {
            if (isCalledShot) {
                Mod.Log.Trace("TH:GAMD entered.");

                // Calculate called shot modifier
                int pilotValue = PilotHelper.GetCalledShotModifier(attacker.GetPilot());
                // TODO: Find items that could apply a modifier
                //Statistic calledShotModStat = State.CurrentAttacker?.StatCollection.GetStatistic(ModStats.CalledShotMod);
                //int unitMod = calledShotModStat != null ? calledShotModStat.Value<int>() : 0;
                int calledShotMod = pilotValue;
                Mod.Log.Debug($" Called Shot from pilot:{attacker.GetPilot().Name} => pilotValue:{pilotValue} + unitMod:0 = calledShotMod:{calledShotMod}");

                if (calledShotMod != 0) {
                    __result = string.Format("{0}CALLED-SHOT {1:+#;-#}; ", __result, (int)calledShotMod);
                }
            }
        }
    }

    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "UpdateToolTipsFiring")]
    public static class CombatHUDWeaponSlot_UpdateToolTipsFiring {

        public static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target, CombatGameState ___Combat, CombatHUD ___HUD, int ___modifier) {
            if (___HUD.SelectionHandler.ActiveState.SelectionType == SelectionType.FireMorale) {
                Mod.Log.Trace("CHUDWS:UTTF:Post entered.");

                // Calculate called shot modifier
                int pilotValue = PilotHelper.GetCalledShotModifier(___HUD.SelectedActor.GetPilot());
                // TODO: Find items that could apply a modifier
                //Statistic calledShotModStat = State.CurrentAttacker?.StatCollection.GetStatistic(ModStats.CalledShotMod);
                //int unitMod = calledShotModStat != null ? calledShotModStat.Value<int>() : 0;
                int calledShotMod = pilotValue;
                Mod.Log.Debug($" Called Shot from pilot:{___HUD.SelectedActor.GetPilot().Name} => pilotValue:{pilotValue} + unitMod:0 = calledShotMod:{calledShotMod}");

                if (calledShotMod != 0) {
                    AddMoraleToolTip(__instance, ___Combat.Constants.CombatUIConstants.MoraleAttackDescription.Name, calledShotMod);
                }
            }
        }

        private static void AddMoraleToolTip(CombatHUDWeaponSlot instance, string description, int modifier) {
            Mod.Log.Trace($"CHUDWS:UTTF:AMTT - adding desc:{description} with modifier:{modifier}.");
            if (modifier < 0) {
                instance.ToolTipHoverElement.BuffStrings.Add(new Text("{0} {1:+0;-#}", new object[] { description, modifier }));
            } else if (modifier > 0) {
                instance.ToolTipHoverElement.DebuffStrings.Add(new Text("{0} {1:+0;-#}", new object[] { description, modifier }));
            }
        }
    }

    //// Cleanup at the end of combat
    //[HarmonyPatch(typeof(TurnDirector), "OnCombatGameDestroyed")]
    //public static class TurnDirector_OnCombatGameDestroyed {

    //    public static void Postfix(TurnDirector __instance) {
    //        Mod.Log.Trace("TD:OCGD entered");

    //        State.Reset();
    //    }
    //}
}
