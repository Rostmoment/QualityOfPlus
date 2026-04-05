using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace QualityOfPlus.BetterElevator
{
    [HarmonyPatch(typeof(ElevatorScreen))]
    class BackButtonsInElevator
    {
        private static void ShowButtons(ElevatorScreen __instance)
        {
            if (!BetterElevatorComponent.OldButtons)
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
