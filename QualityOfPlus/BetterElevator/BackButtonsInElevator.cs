using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace QualityOfPlus.BetterElevator
{
    [HarmonyPatch(typeof(ElevatorScreen))]
    public class BackButtonsInElevator
    {
        public static bool ForcedStart => ForceStartCounter > 0;
        public static int ForceStartCounter { get; private set; }
        public static void ForceStart() => ForceStartCounter++;
        public static void ReleaseStart()
        {
            if (ForceStartCounter > 0)
                ForceStartCounter--;
        }

        public static bool LockedStart => LockedStartCounter > 0;
        public static int LockedStartCounter { get; private set; }
        public static void LockStart() => LockedStartCounter++;
        public static void UnlockStart()
        {
            if (LockedStartCounter > 0)
                ForceStartCounter--;
        }

        private static void ShowButtons(ElevatorScreen __instance)

        {
            if ((!BetterElevatorComponent.OldButtons || ForcedStart) && !LockedStart)
            {
                __instance.StartGame();
                return;
            }
            
            __instance.skipButton.SetActive(CoreGameManager.Instance.sceneObject.skippable);

            __instance.buttonAnimator.Play("ButtonRise", -1, 0f);
            __instance.playButton.SetActive(true);

            __instance.transform.Find("ElevatorTransission").Find("Play").gameObject.SetActive(false);
        }

        [HarmonyPatch(nameof(ElevatorScreen.Update))]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ReplaceStartGame(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo startGame = AccessTools.Method(typeof(ElevatorScreen), nameof(ElevatorScreen.StartGame));
            MethodInfo showButton = AccessTools.Method(typeof(BackButtonsInElevator), nameof(ShowButtons));

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo mi && mi == startGame)
                    yield return new CodeInstruction(OpCodes.Call, showButton);
                else
                    yield return instruction;
            }
        }
    }
}
