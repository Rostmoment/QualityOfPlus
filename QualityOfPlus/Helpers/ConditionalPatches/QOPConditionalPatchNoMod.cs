using BepInEx.Bootstrap;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.ConditionalPatches
{
    class QOPConditionalPatchNoMod : QOPConditionalPatch
    {
        private string Mod { get; }
        public QOPConditionalPatchNoMod(string mod) 
        { 
            Mod = mod;
        }

        public override bool ShouldPatch() => !Chainloader.PluginInfos.ContainsKey(Mod);
    }
}
