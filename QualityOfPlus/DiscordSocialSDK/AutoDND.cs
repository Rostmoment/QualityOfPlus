using BepInEx.DiscordSocialSDK;
using BepInEx.DiscordSocialSDK.Enums;
using HarmonyLib;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK
{
    [ConditionalPatchMod(BepInEx.DiscordSocialSDK.DiscordSocialSDKPlugin.GUID)]
    [HarmonyPatch]
    class AutoDND
    {
        private static StatusType? previousStatus;

        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.BeginPlay))]
        [HarmonyPostfix]
        private static void SetDND()
        {
            previousStatus = DiscordSocialSDKComponent.client.GetOnlineStatus();

            if (previousStatus == StatusType.Invisible || !DiscordSocialSDKComponent.AutoDND)
                return;

            DiscordSocialSDKComponent.client.SetOnlineStatus(StatusType.Dnd);
        }

        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void RemoveDND()
        {
            if (previousStatus == null)
                return;

            if (!DiscordSocialSDKComponent.AutoDND)
                return;

            if (DiscordSocialSDKComponent.client.GetOnlineStatus() == StatusType.Invisible)
                return;

            DiscordSocialSDKComponent.client.SetOnlineStatus(previousStatus.Value);
            previousStatus = null;
        }
    }
}
