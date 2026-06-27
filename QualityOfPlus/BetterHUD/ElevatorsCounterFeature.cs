using BepInEx.Configuration;
using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace QualityOfPlus.BetterHUD
{
    public class ElevatorsCounterFeature : QOPFeature, IToggleableFeature, IOnAPIStart
    {
        public override string ID => "QOP.FEATURE.HUD.ELEVATORS.COUNTER";

        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Elevators Counter", true, "Replaces the notebooks counter with the elevators counter after all notebooks are collected");
        }

        public override void PostInitialize(QOPCategory category) { }

        private Texture2D elevatorCounter, notebookCounter;
        public IEnumerator APIStartAction()
        {
            yield return "Creating elevator counter texture...";

            elevatorCounter = AssetLoader.TextureFromMod(BasePlugin.Instance, "ElevatorIconSheet.png");
            notebookCounter = Resources.FindObjectsOfTypeAll<Texture2D>().First(x => x.name == "NotebookIcon_Sheet").MakeReadableCopy(true);
        }
        public Texture2D CounterIcon(bool allNotebooks)
        {
            if (!this.IsEnabled() || !allNotebooks)
                return notebookCounter;
            return elevatorCounter;
        }
    }
}