using BepInEx;
using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.RPC;
using CampfireFrenzy;
using HarmonyLib;
using MTM101BaldAPI;
using PicnicPanic;
using PlusLevelStudio;
using PlusLevelStudio.Editor;
using System;

namespace QualityOfPlus.DiscordSocialSDK
{
    [ConditionalPatchMod(BepInEx.DiscordSocialSDK.DiscordSocialSDKPlugin.GUID)]
    [HarmonyPatch]
    class DiscordRPC
    {
        private const string LOADING_MODS_ICON = "loading-icon";
        private const string PLUS_ICON = "plus-icon";
        private const string GREEN_YTP_ICON = "green-ytp-icon";
        private const string LEVEL_STUDIO_ICON = "editor-icon";
        private const string NOTEBOOK_ICON_LINK = "notebooks-icon";
        private const string ELEVATOR_ICON_LINK = "elevator-icon";
        private const string CAMPFIRE_ICON_LINK = "campfire-icon";
        private const string PICNIC_ICON_LINK = "apple-icon";

        private static void Clear() => DiscordSocialSDKComponent.rpc.Wipe();

        private static void SetMain()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.ClearButtons();
            DiscordSocialSDKComponent.rpc.SetActivityType(ActivityTypes.Playing);

            if (!DiscordSocialSDKComponent.ButtonOneText.IsNullOrWhiteSpace() && !DiscordSocialSDKComponent.ButtonOneUrl.IsNullOrWhiteSpace())
                DiscordSocialSDKComponent.rpc.AddButton(DiscordSocialSDKComponent.ButtonOneText, DiscordSocialSDKComponent.ButtonOneUrl);

            if (!DiscordSocialSDKComponent.ButtonTwoText.IsNullOrWhiteSpace() && !DiscordSocialSDKComponent.ButtonTwoUrl.IsNullOrWhiteSpace())
                DiscordSocialSDKComponent.rpc.AddButton(DiscordSocialSDKComponent.ButtonTwoText, DiscordSocialSDKComponent.ButtonTwoUrl);
        }

        [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Pause))]
        [HarmonyPostfix]
        private static void PauseGame(CoreGameManager __instance)
        {
            string playing = "RpcPlayingPlus";
            if (__instance.Paused)
                playing = "RpcPlayingPlusPause";

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText(playing),
                DiscordSocialSDKComponent.rpc.CurrentDetails,
                DiscordSocialSDKComponent.rpc.CurrentDetailsUrl,
                DiscordSocialSDKComponent.rpc.CurrentState,
                DiscordSocialSDKComponent.rpc.CurrentStateUrl
            );

            SetMain();
        }


        [HarmonyPatch(typeof(ModLoadingScreenManager), nameof(ModLoadingScreenManager.Start))]
        [HarmonyPostfix]
        private static void LoadingMods()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcLoadingMods")
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LOADING_MODS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcLoadingModsDesc")
            );

            SetMain();
        }

        [HarmonyPatch(typeof(ModLoadingScreenManager), nameof(ModLoadingScreenManager.LoadingEnded))]
        [HarmonyPostfix]
        private static void FinishedLoadingMods()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcSelectAccount")
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus")
            );

            SetMain();
        }

        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void InMenu()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcMainMenu")
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus")
            );

            SetMain();
        }

        [ConditionalPatchMod(Compats.LEVEL_STUDIO_GUID)]
        [HarmonyPatch(typeof(LevelStudioPlugin), nameof(LevelStudioPlugin.GoToEditor))]
        [HarmonyPostfix]
        private static void InLevelStudioEditor()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcLevelEditor"),
                "https://gamebanana.com/mods/617567",
                LocalizationManager.Instance.GetLocalizedText("RpcCreatingLevel")
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LEVEL_STUDIO_ICON,
                "Using Level Studio"
            );

            SetMain();
        }

        [HarmonyPatch(typeof(Elevator), nameof(Elevator.SetState))]
        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void InGame()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            if (BaseGameManager.Instance.FoundNotebooks < BaseGameManager.Instance.Ec.notebookTotal)
            {
                string state = string.Format(
                    LocalizationManager.Instance.GetLocalizedText("RpcNotebooksProgress"),
                    BaseGameManager.Instance.FoundNotebooks,
                    BaseGameManager.Instance.Ec.notebookTotal
                );

                if (BaseGameManager.Instance.name.ToLower().Contains("editor"))
                    DiscordSocialSDKComponent.rpc.SetInfo(
                        LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                        LocalizationManager.Instance.GetLocalizedText("RpcEditor"),
                        state: state
                    );
                else
                    DiscordSocialSDKComponent.rpc.SetInfo(
                        LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                        string.Format(
                            LocalizationManager.Instance.GetLocalizedText("RpcSeed"),
                            CoreGameManager.Instance?.Seed()
                        ),
                        state: state
                    );

                DiscordSocialSDKComponent.rpc.SetAssets(
                    PLUS_ICON,
                    LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                    NOTEBOOK_ICON_LINK,
                    LocalizationManager.Instance.GetLocalizedText("RpcCollectingNotebooks")
                );
            }
            else
            {
                string state = string.Format(
                    LocalizationManager.Instance.GetLocalizedText("RpcElevatorsProgress"),
                    BaseGameManager.Instance.ec.GetOutOfElevatorsCount(),
                    BaseGameManager.Instance.Ec.GetElevatorsCount()
                );

                if (BaseGameManager.Instance.name.ToLower().Contains("editor"))
                    DiscordSocialSDKComponent.rpc.SetInfo(
                        LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                        LocalizationManager.Instance.GetLocalizedText("RpcEditor"),
                        state: state
                    );
                else
                    DiscordSocialSDKComponent.rpc.SetInfo(
                        LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                        string.Format(
                            LocalizationManager.Instance.GetLocalizedText("RpcSeed"),
                            CoreGameManager.Instance?.Seed()
                        ),
                        state: state
                    );

                DiscordSocialSDKComponent.rpc.SetAssets(
                    PLUS_ICON,
                    LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                    ELEVATOR_ICON_LINK,
                    LocalizationManager.Instance.GetLocalizedText("RpcClosingElevators")
                );
            }

            SetMain();
        }

        [HarmonyPatch(typeof(PitstopGameManager), nameof(PitstopGameManager.Initialize))]
        [HarmonyPostfix]
        private static void InPitstop()
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcPitstop"),
                state: string.Format(
                    LocalizationManager.Instance.GetLocalizedText("RpcYtps"),
                    CoreGameManager.Instance?.GetPoints(0)
                )
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                GREEN_YTP_ICON
            );

            SetMain();
        }

        [HarmonyPatch(typeof(EndlessGameManager), nameof(EndlessGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void InEndlessGame(EndlessGameManager __instance)
        {
            if (!DiscordSocialSDKComponent.EnableDiscordRPC)
            {
                Clear();
                return;
            }

            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                string.Format(
                    LocalizationManager.Instance.GetLocalizedText("RpcSeed"),
                    CoreGameManager.Instance?.Seed()
                ),
                state: string.Format(
                    LocalizationManager.Instance.GetLocalizedText("RpcNotebooks"),
                    __instance.FoundNotebooks
                )
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                NOTEBOOK_ICON_LINK,
                LocalizationManager.Instance.GetLocalizedText("RpcEndlessMode")
            );

            SetMain();
        }

        [HarmonyPatch(typeof(TutorialGameManager), nameof(TutorialGameManager.BeginPlay))]
        [HarmonyPatch(typeof(TutorialGameManager), nameof(TutorialGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void InTutorial(TutorialGameManager __instance)
        {
            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcTutorial")
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                NOTEBOOK_ICON_LINK,
                LocalizationManager.Instance.GetLocalizedText("RpcCollectingNotebooks")
            );

            if (__instance.NotebookTotal <= __instance.FoundNotebooks)
                DiscordSocialSDKComponent.rpc.SetAssets(
                    PLUS_ICON,
                    LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                    ELEVATOR_ICON_LINK,
                    LocalizationManager.Instance.GetLocalizedText("RpcElevatorGo")
                );

            SetMain();
        }

        [HarmonyPatch(typeof(Minigame_Campfire), nameof(Minigame_Campfire.Initialize))]
        [HarmonyPatch(typeof(Minigame_Campfire), nameof(Minigame_Campfire.AddScore))]
        [HarmonyPostfix]
        private static void Campfire(Minigame_Campfire __instance)
        {
            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcCampfire"),
                state: string.Format(
                    LocalizationManager.Instance.GetLocalizedText("RpcCampfireScore"),
                    __instance.score
                )
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                CAMPFIRE_ICON_LINK,
                LocalizationManager.Instance.GetLocalizedText("RpcCampfireDesc")
            );

            SetMain();
        }

        [HarmonyPatch(typeof(Minigame_Picnic), nameof(Minigame_Picnic.Initialize))]
        [HarmonyPostfix]
        private static void Picnic(Minigame_Picnic __instance)
        {
            DiscordSocialSDKComponent.rpc.SetInfo(
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                LocalizationManager.Instance.GetLocalizedText("RpcPicnic")
            );

            DiscordSocialSDKComponent.rpc.SetAssets(
                PLUS_ICON,
                LocalizationManager.Instance.GetLocalizedText("RpcPlayingPlus"),
                PICNIC_ICON_LINK
            );

            SetMain();
        }
    }
}