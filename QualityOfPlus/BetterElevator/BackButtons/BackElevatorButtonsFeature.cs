using QualityOfPlus.Interfaces;

namespace QualityOfPlus.BetterElevator.BackButtons
{
    public class BackElevatorButtonsFeature : QOPToggleableFeature
    {
        public override string ID => "QOP.FEATURE.BACK.ELEVATOR.BUTTONS";

        protected override string EnabledConfigKey => "Old Buttons";
        protected override string EnabledConfigDescription => "Use old elevator buttons, just like before version 0.14";
        protected override bool DefaultValue => false;

        public override void PostInitialize(QOPCategory category)
        {
        }

        public bool ButtonsShouldAppear()
        {
            return IsEnabled(); // TODO: add methods for mods to lock/force buttons
        }
    }
}