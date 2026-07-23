using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QualityOfPlus.ConditionalPatches
{
    static class HarmonyExtension
    {
        private static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static void PatchAllConditionalFixed(this Harmony harmony)
        {
            Assembly assembly = typeof(BasePlugin).Assembly;

            foreach (Type type in assembly.GetTypes())
            {
                if (type == null)
                    continue;

                bool shouldPatchClass = false;
                List<QOPConditionalPatch> conditionsClass = new List<QOPConditionalPatch>();

                PatchInfoData classPatchData = new PatchInfoData();

                foreach (CustomAttributeData cad in type.CustomAttributes)
                {
                    if (Is<QOPConditionalPatch>(cad))
                    {
                        conditionsClass.Add(AttributeFrom<QOPConditionalPatch>(cad));
                        if (!AllConditions(conditionsClass))
                            goto continueTypes;
                    }

                    if (Is<HarmonyPatch>(cad))
                    {
                        shouldPatchClass = true;
                        HarmonyPatch patch = AttributeFrom<HarmonyPatch>(cad);
                        classPatchData.Merge(patch, type.FullName);
                    }
                }

                if (!shouldPatchClass)
                    continue;

                if (conditionsClass.Count > 0)
                    shouldPatchClass = AllConditions(conditionsClass);

                if (!shouldPatchClass)
                    continue;


                foreach (MethodInfo method in type.GetMethods(flags))
                {
                    if (method == null)
                        continue;

                    string memberName = $"{type.FullName}.{method.Name}";

                    bool shouldPatchMethod = false;
                    List<QOPConditionalPatch> conditionsMethod = new List<QOPConditionalPatch>();

                    List<PatchInfoData> methodPatchDatas = new List<PatchInfoData>();
                    bool isPrefix = false;
                    bool isPostfix = false;
                    bool isTranspiler = false;
                    bool isFinializer = false;
                    bool isILManipulator = false;

                    foreach (CustomAttributeData cad in method.CustomAttributes)
                    {

                        if (Is<QOPConditionalPatch>(cad))
                        {
                            conditionsMethod.Add(AttributeFrom<QOPConditionalPatch>(cad));
                            if (!AllConditions(conditionsMethod))
                                goto continueMethods;
                        }

                        if (Is<HarmonyPatch>(cad))
                        {
                            shouldPatchMethod = true;
                            HarmonyPatch patch = AttributeFrom<HarmonyPatch>(cad);
                            PatchInfoData data = new PatchInfoData();
                            data.Merge(patch, memberName);
                            methodPatchDatas.Add(data);
                        }

                        if (Is<HarmonyPrefix>(cad))
                            isPrefix = true;

                        if (Is<HarmonyPostfix>(cad))
                            isPostfix = true;

                        if (Is<HarmonyTranspiler>(cad))
                            isTranspiler = true;

                        if (Is<HarmonyFinalizer>(cad))
                            isFinializer = true;

                        if (Is<HarmonyILManipulator>(cad))
                            isILManipulator = true;
                    }

                    if (!shouldPatchMethod)
                        continue;

                    if (conditionsMethod.Count > 0)
                        shouldPatchMethod = AllConditions(conditionsMethod);

                    if (!shouldPatchMethod)
                        continue;

                    if (!isPrefix && !isPostfix && !isTranspiler && !isFinializer && !isILManipulator)
                        continue;

                    foreach (PatchInfoData data in methodPatchDatas)
                    {
                        PatchInfoData merged = PatchInfoData.Merge(data, classPatchData, memberName);
                        MethodInfo original = merged.GetMethod();
                        BasePlugin.Logger.LogInfo(merged);
                        Patch(harmony, original, method, isPrefix, isPostfix, isTranspiler, isFinializer, isILManipulator);
                    }
                    continueMethods:;
                }
                continueTypes:;
            }
        }

        private static void Patch(Harmony harmony, MethodInfo original, MethodInfo patch, bool isPrefix, bool isPostfix, bool isTranspiler, bool isFinializer, bool isILManipulator)
        {
            HarmonyMethod prefix = null;
            if (isPrefix)
                prefix = new HarmonyMethod(patch);

            HarmonyMethod postfix = null;
            if (isPostfix)
                postfix = new HarmonyMethod(patch);

            HarmonyMethod transpiler = null;
            if (isTranspiler)
                transpiler = new HarmonyMethod(patch);

            HarmonyMethod finializer = null;
            if (isFinializer)
                finializer = new HarmonyMethod(patch);

            HarmonyMethod ilManipulator = null;
            if (isILManipulator)
                ilManipulator = new HarmonyMethod(patch);

            harmony.Patch(original, prefix, postfix, transpiler, finializer, ilManipulator);
        }

        private static bool Is<T>(CustomAttributeData cad) where T : Attribute => typeof(T).IsAssignableFrom(cad.AttributeType);

        private static T AttributeFrom<T>(CustomAttributeData cad) where T : Attribute
        {
            if (!typeof(T).IsAssignableFrom(cad.AttributeType))
                throw new ArgumentException($"{cad.AttributeType.FullName} is not {typeof(T).FullName}");


            List<CustomAttributeTypedArgument> list = cad.ConstructorArguments.ToList();
            List<object> paramList = new List<object>();
            list.ForEach(arg =>
            {
                paramList.Add(arg.Value);
            });

            return (T)Activator.CreateInstance(cad.AttributeType, paramList.ToArray());
        }

        private static bool AllConditions(List<QOPConditionalPatch> patches)
        {
            foreach (QOPConditionalPatch patch in patches)
            {
                if (!patch.ShouldPatch())
                    return false;
            }
            return true;
        }
    }
}
