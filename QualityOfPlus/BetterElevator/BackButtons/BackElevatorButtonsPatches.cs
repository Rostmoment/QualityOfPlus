using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace QualityOfPlus.BetterElevator.BackButtons
{
    [HarmonyPatch(typeof(ElevatorScreen))]
    internal class BackElevatorButtonsPatches
    {
        private static void ShowButtons(ElevatorScreen __instance)
        {
            if (!QOPManager.Instance.GetFeature<BackElevatorButtonsFeature>().ButtonsShouldAppear())
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
            MethodInfo showButton = AccessTools.Method(typeof(BackElevatorButtonsPatches), nameof(ShowButtons));

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
