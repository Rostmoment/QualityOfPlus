using BepInEx.DiscordSocialSDK.Enums;
using CampfireFrenzy;
using HarmonyLib;
using MTM101BaldAPI;
using PicnicPanic;
using PlusLevelStudio;
using QualityOfPlus.ConditionalPatches;
using QualityOfPlus.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK.RPC
{
    [QOPConditionalPatchMod(BepInEx.DiscordSocialSDK.DiscordSocialSDKPlugin.GUID)]
    [HarmonyPatch]
    internal class DiscordRPCPatches
    {
        private const string LOADING_MODS_ICON = "loading-icon";
        private const string PLUS_ICON = "plus-icon";
        private const string GREEN_YTP_ICON = "green-ytp-icon";
        private const string LEVEL_STUDIO_ICON = "editor-icon";
        private const string NOTEBOOK_ICON_LINK = "notebooks-icon";
        private const string ELEVATOR_ICON_LINK = "elevator-icon";
        private const string CAMPFIRE_ICON_LINK = "campfire-icon";
        private const string PICNIC_ICON_LINK = "apple-icon";


        private static bool SetMain(out DiscordRPCFeature feature)
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled<DiscordRPCFeature>(out feature)) 
            {
                feature.Clear();
                return false;
            }

            feature.RPC.ClearButtons();
            feature.RPC.SetActivityType(ActivityTypes.Playing);
            feature.RPC.AddButton("Steam", "https://store.steampowered.com/app/1275890/Baldis_Basics_Plus/");

            return true;
        }


        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Pause))]
        [HarmonyPostfix]
        private static void PauseGame(CoreGameManager __instance)
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            string playing = "QOP_RPC_PLAYING_PLUS";
            if (__instance.Paused)
                playing = "QOP_RPC_PLAYING_PLUS_PAUSE";

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText(playing),
                feature.RPC.CurrentDetails,
                feature.RPC.CurrentDetailsUrl,
                feature.RPC.CurrentState,
                feature.RPC.CurrentStateUrl
            );
        }


        [HarmonyPatch(typeof(ModLoadingScreenManager), nameof(ModLoadingScreenManager.Start))]
        [HarmonyPostfix]
        private static void LoadingMods()
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_LOADING_MODS")
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LOADING_MODS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_LOADING_MODS_DESC")
            );
        }


        [HarmonyPatch(typeof(ModLoadingScreenManager), nameof(ModLoadingScreenManager.LoadingEnded))]
        [HarmonyPostfix]
        private static void FinishedLoadingMods()
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_SELECT_ACCOUNT")
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS")
            );
        }


        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void InMenu()
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_MAIN_MENU")
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS")
            );
        }

        [QOPConditionalPatchMod(Compats.LEVEL_STUDIO_GUID)]
        [HarmonyPatch(typeof(LevelStudioPlugin), nameof(LevelStudioPlugin.GoToEditor))]
        [HarmonyPostfix]
        private static void InLevelStudioEditor()
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_LEVEL_EDITOR"),
                "https://gamebanana.com/mods/617567",
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_CREATING_LEVEL")
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LEVEL_STUDIO_ICON,
                "Using Level Studio"
            );
        }


        [HarmonyPatch(typeof(Elevator), nameof(Elevator.SetState))]
        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void InGame()
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            bool collectingNotebooks = BaseGameManager.Instance.FoundNotebooks < BaseGameManager.Instance.Ec.notebookTotal;
            bool isEditor = BaseGameManager.Instance.name.ToLower().Contains("editor");

            string details = isEditor
                ? LocalizationManager.Instance.GetLocalizedText("QOP_RPC_EDITOR")
                : string.Format(LocalizationManager.Instance.GetLocalizedText("QOP_RPC_SEED"), CoreGameManager.Instance?.Seed());

            string state;
            string smallIcon;
            string smallIconText;

            if (collectingNotebooks)
            {
                state = string.Format(
                    LocalizationManager.Instance.GetLocalizedText("QOP_RPC_NOTEBOOKS_PROGRESS"),
                    BaseGameManager.Instance.FoundNotebooks,
                    BaseGameManager.Instance.Ec.notebookTotal);
                smallIcon = NOTEBOOK_ICON_LINK;
                smallIconText = LocalizationManager.Instance.GetLocalizedText("QOP_RPC_COLLECTING_NOTEBOOKS");
            }
            else
            {
                state = string.Format(
                    LocalizationManager.Instance.GetLocalizedText("QOP_RPC_ELEVATORS_PROGRESS"),
                    BaseGameManager.Instance.ec.GetOutOfElevatorsCount(),
                    BaseGameManager.Instance.Ec.GetElevatorsCount());
                smallIcon = ELEVATOR_ICON_LINK;
                smallIconText = LocalizationManager.Instance.GetLocalizedText("QOP_RPC_CLOSING_ELEVATORS");
            }

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                details,
                state: state);

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                smallIcon,
                smallIconText);
        }


        [HarmonyPatch(typeof(PitstopGameManager), nameof(PitstopGameManager.Initialize))]
        [HarmonyPostfix]
        private static void InPitstop()
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PITSTOP"),
                state: string.Format(LocalizationManager.Instance.GetLocalizedText("QOP_RPC_YTPS"), CoreGameManager.Instance?.GetPoints(0))
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                GREEN_YTP_ICON
            );
        }


        [HarmonyPatch(typeof(EndlessGameManager), nameof(EndlessGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void InEndlessGame(EndlessGameManager __instance)
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                string.Format(LocalizationManager.Instance.GetLocalizedText("QOP_RPC_SEED"), CoreGameManager.Instance?.Seed()),
                state: string.Format(LocalizationManager.Instance.GetLocalizedText("QOP_RPC_NOTEBOOKS"), __instance.FoundNotebooks)
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                NOTEBOOK_ICON_LINK,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_ENDLESS_MODE")
            );
        }


        [HarmonyPatch(typeof(TutorialGameManager), nameof(TutorialGameManager.BeginPlay))]
        [HarmonyPatch(typeof(TutorialGameManager), nameof(TutorialGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void InTutorial(TutorialGameManager __instance)
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_TUTORIAL")
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                NOTEBOOK_ICON_LINK,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_COLLECTING_NOTEBOOKS")
            );

            if (__instance.NotebookTotal <= __instance.FoundNotebooks)
            {
                feature.RPC.SetAssets(
                    PLUS_ICON,
                    LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                    ELEVATOR_ICON_LINK,
                    LocalizationManager.Instance.GetLocalizedText("QOP_RPC_ELEVATOR_GO")
                );
            }
        }


        [HarmonyPatch(typeof(Minigame_Campfire), nameof(Minigame_Campfire.Initialize))]
        [HarmonyPatch(typeof(Minigame_Campfire), nameof(Minigame_Campfire.AddScore))]
        [HarmonyPostfix]
        private static void Campfire(Minigame_Campfire __instance)
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_CAMPFIRE"),
                state: string.Format(LocalizationManager.Instance.GetLocalizedText("QOP_RPC_CAMPFIRE_SCORE"), __instance.score)
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                CAMPFIRE_ICON_LINK,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_CAMPFIRE_DESC")
            );
        }


        [HarmonyPatch(typeof(Minigame_Picnic), nameof(Minigame_Picnic.Initialize))]
        [HarmonyPostfix]
        private static void Picnic(Minigame_Picnic __instance)
        {
            if (!SetMain(out DiscordRPCFeature feature))
                return;

            feature.RPC.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PICNIC")
            );

            feature.RPC.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("QOP_RPC_PLAYING_PLUS"),
                PICNIC_ICON_LINK
            );
        }
    }
}
