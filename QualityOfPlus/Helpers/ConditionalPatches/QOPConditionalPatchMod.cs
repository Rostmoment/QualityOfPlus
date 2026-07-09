using BepInEx.Bootstrap;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.ConditionalPatches
{
    class QOPConditionalPatchMod : QOPConditionalPatch
    {
        private string Mod { get; }
        public QOPConditionalPatchMod(string mod) 
        { 
            Mod = mod;
        }

        public override bool ShouldPatch() => Chainloader.PluginInfos.ContainsKey(Mod);
    }
}
