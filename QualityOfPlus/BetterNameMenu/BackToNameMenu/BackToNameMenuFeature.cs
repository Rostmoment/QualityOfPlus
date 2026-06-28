using QualityOfPlus.Interfaces;

namespace QualityOfPlus.BetterNameMenu.BackToNameMenu
{
    public class BackToNameMenuFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.BACK.TO.NAME.MENU";

        protected override string EnabledConfigKey => "Back Button";
        protected override string EnabledConfigDescription => "Adds an button to return to the name entry menu from the main menu";

        public override void PostInitialize(QOPCategory category)
        {
        }

        internal void OnBackButtonPressed()
        {
        }
    }
}