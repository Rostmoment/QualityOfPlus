using MTM101BaldAPI.AssetTools;
using QualityOfPlus.Interfaces;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace QualityOfPlus.BetterHUD
{
    public class ElevatorsCounterFeature : QOPToggleableFeature, IOnAPIStart
    {
        public override string ID => "QOP.FEATURE.HUD.ELEVATORS.COUNTER";

        protected override string EnabledConfigKey => "Elevators Counter";
        protected override string EnabledConfigDescription => "Replaces the notebooks counter with the elevators counter after all notebooks are collected";

        public override void PostInitialize(QOPCategory category)
        {
        }

        private Texture2D elevatorCounter;
        private Texture2D notebookCounter;

        public IEnumerator APIStartAction()
        {
            yield return "Creating elevator counter texture...";

            elevatorCounter = AssetLoader.TextureFromMod(BasePlugin.Instance, "ElevatorIconSheet.png");
            notebookCounter = Resources.FindObjectsOfTypeAll<Texture2D>().First(x => x.name == "NotebookIcon_Sheet").MakeReadableCopy(true);
        }

        public Texture2D CounterIcon(bool allNotebooks)
        {
            if (!IsEnabled() || !allNotebooks)
                return notebookCounter;

            return elevatorCounter;
        }
    }
}