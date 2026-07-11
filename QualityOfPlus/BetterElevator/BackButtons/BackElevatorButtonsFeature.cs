using QualityOfPlus.Interfaces;
using System;

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


        private int forceOnCount = 0;
        private int forceOffCount = 0;

        /// <summary>
        /// Adds a force override. Pass <c>true</c> to force buttons on, <c>false</c> to force them off.<br/>
        /// Each call must be paired with a matching <see cref="RemoveForce"/> call to release the override.
        /// </summary>
        public void AddForce(bool value)
        {
            if (value) 
                forceOnCount++;
            else 
                forceOffCount++;
        }

        /// <summary>Releases a previously added force override.</summary>
        public void RemoveForce(bool value)
        {
            if (value) 
                forceOnCount = Math.Max(0, forceOnCount - 1);
            else 
                forceOffCount = Math.Max(0, forceOffCount - 1);
        }

        /// <summary>
        /// Returns whether the old elevator buttons should be shown.<br/>
        /// Priority: force-on > force-off > config toggle.<br/>
        /// If any mod forced on — buttons shown. Else if any forced off — buttons hidden. Else config decides.
        /// </summary>
        public bool ButtonsShouldAppear()
        {
            if (forceOnCount > 0) 
                return true;

            if (forceOffCount > 0) 
                return false;

            return IsEnabled();
        }
    }
}