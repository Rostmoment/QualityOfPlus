using BepInEx.DiscordSocialSDK;
using BepInEx.DiscordSocialSDK.Enums;
using HarmonyLib;
using QualityOfPlus.ConditionalPatches;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.DiscordSocialSDK.AutoDND
{
    [QOPConditionalPatchMod(DiscordSocialSDKPlugin.GUID)]
    [HarmonyPatch]
    internal class AutoDNDPatches
    {

        private static StatusType? previousStatus;

        [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.BeginPlay))]
        [HarmonyPostfix]
        private static void SetDND()
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled<AutoDNDFeature>(out _) || !DiscordSocialSDKCategory.Client.IsReady)
                return;

            if (previousStatus != null)
                return;

            previousStatus = DiscordSocialSDKCategory.Client.GetOnlineStatus();

            if (previousStatus == StatusType.Invisible)
                return;

            DiscordSocialSDKCategory.Client.SetOnlineStatus(StatusType.Dnd);
        }

        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void RemoveDND()
        {
            if (!QOPManager.Instance.GetFeatureIfEnabled<AutoDNDFeature>(out _) || !DiscordSocialSDKCategory.Client.IsReady)
                return;

            if (previousStatus == null)
                return;

            if (DiscordSocialSDKCategory.Client.GetOnlineStatus() == StatusType.Invisible)
                return;

            DiscordSocialSDKCategory.Client.SetOnlineStatus(previousStatus.Value);
            previousStatus = null;
        }
    }
}
